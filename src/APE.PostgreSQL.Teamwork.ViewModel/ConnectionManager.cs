// <copyright file="ConnectionManager.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APE.PostgreSQL.Teamwork.Model.Setting;
using APE.PostgreSQL.Teamwork.ViewModel;
using Dapper;
using log4net;
using Npgsql;

namespace APE.PostgreSQL.Teamwork
{
    /// <summary>
    /// Contains all methods to communicate with the database.
    /// </summary>
    public class ConnectionManager : IConnectionManager
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly object connectionLock = new object();

        private string id;
        private string host;
        private string password;
        private int port;

        /// <summary>
        /// Creates a new instance of the <see cref="ConnectionManager"/> class.
        /// </summary>
        public ConnectionManager()
        {
        }

        /// <summary>
        /// Indicates if <see cref="Initialize(string, string, string, int)"/> was called.
        /// </summary>
        public bool Initialized { get; set; }

        /// <summary>
        /// Initializes the <see cref="ConnectionManager"/> with the given parameters.
        /// </summary>
        /// <param name="id">The postgres id which is used as the username.</param>
        /// <param name="host">The host where the postgres server is running.</param>
        /// <param name="password">The password which is used to login.</param>
        /// <param name="port">The port for the postgres server.</param>
        public void Initialize(string id, string host, string password, int port)
        {
            this.id = id;
            this.host = host;
            this.port = port;
            this.password = password;
            this.Initialized = true;
        }

        /// <summary>
        /// Checks if a connection to the default postgres database can be established.
        /// </summary>
        /// <returns>A bool indicating if the connection worked.</returns>
        public bool CheckConnection()
        {
            return this.CheckConnection(Database.PostgresDefaultDatabaseName);
        }

        /// <summary>
        /// Checks if the given database exists.
        /// </summary>
        /// <param name="databaseName">Name of the database which gets checked.</param>
        /// <returns>True: connection established, false: connection not established.</returns>
        public bool CheckConnection(string databaseName)
        {
            try
            {
                var connectionString = this.GetConnectionString(databaseName);

                // check if connection can be established
                using (var connection = new NpgsqlConnection(connectionString.ConnectionString))
                {
                    connection.Open();
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Warn(string.Format("Connection to the database {0} could not be established", databaseName), ex);
                return false;
            }
        }

        /// <summary>
        /// Execute the given SQL command without a return value.
        /// </summary>
        /// <remarks>
        /// Max time is 10 minutes, after that a timeout exception is thrown.
        /// </remarks>
        public void ExecuteCommandNonQuery(IDatabase database, string sql)
        {
            this.ExecuteCommandNonQuery(database.Name, sql);
        }

        /// <summary>
        /// Executes the given SQL command without a return value on the default database defined in the settings.
        /// </summary>
        /// <remarks>
        /// Max time is 10 minutes, after that a timeout exception is thrown.
        /// </remarks>
        /// <param name="sql">SQL Command which is executed.</param>
        public void ExecuteCommandNonQuery(string sql)
        {
            var databaseName = SettingsManager.Get().Setting.Id;
            this.ExecuteCommandNonQuery(databaseName, sql);
        }

        /// <summary>
        /// Execute the given SQL command on the given <see cref="IDatabase"/>.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="database">The <see cref="IDatabase"/> on which the sql is executed.</param>
        /// <param name="sql">The SQL which is executed.</param>
        public List<T> ExecuteCommand<T>(IDatabase database, string sql)
        {
            return this.ExecuteCommand<T>(database.Name, sql);
        }

        /// <summary>
        /// Executes the given SQL command on the default database.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="sql">The SQL which is executed.</param>
        public List<T> ExecuteCommand<T>(string sql)
        {
            var databaseName = SettingsManager.Get().Setting.Id;
            return this.ExecuteCommand<T>(databaseName, sql);
        }

        /// <summary>
        /// Clear all <see cref="NpgsqlConnection"/> pools.
        /// </summary>
        public void ClearPools()
        {
            NpgsqlConnection.ClearAllPools();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "This is ok because the sql is no user input.")]
        private void ExecuteCommandNonQuery(string databaseName, string sql)
        {
            lock (this.connectionLock)
            {
                try
                {
                    using (var connection = new NpgsqlConnection(this.GetConnectionString(databaseName).ConnectionString))
                    {
                        connection.Open();

                        using (var command = new NpgsqlCommand(sql, connection))
                        {
                            // wait max 10 minutes!
                            command.CommandTimeout = 600;
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(string.Format("Exception while executing sql ({0}) in database {1}", sql, databaseName), ex);
                    throw;
                }
            }
        }

        private List<T> ExecuteCommand<T>(string databaseName, string sql, bool retry = true)
        {
            lock (this.connectionLock)
            {
                var connectionStringBuilder = this.GetConnectionString(databaseName);
                using (var connection = new NpgsqlConnection(connectionStringBuilder.ConnectionString))
                {
                    connection.Open();

                    try
                    {
                        return connection.Query<T>(sql).ToList();
                    }
                    catch (NpgsqlException ex)
                    {
                        Log.Error(ex);
                        if (Debugger.IsAttached)
                        {
                            Debugger.Break();
                        }

                        // retry connection once
                        if (retry)
                            return this.ExecuteCommand<T>(databaseName, sql, false);
                        else
                            throw;
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                        if (Debugger.IsAttached)
                        {
                            Debugger.Break();
                        }

                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Creates the connection string.
        /// </summary>
        /// <param name="databaseName">Database name which is used in the connection string.</param>
        /// <returns>The connection string.</returns>
        private NpgsqlConnectionStringBuilder GetConnectionString(string databaseName)
        {
            if (!this.Initialized)
            {
                throw new InvalidOperationException(string.Format("{0} was not initialized", typeof(ConnectionManager).Name));
            }

            var connectionString = SettingsManager.Get().Setting.ConnectionStringTemplate;
            connectionString = connectionString.Replace("[Id]", this.id);
            connectionString = connectionString.Replace("[Host]", this.host);
            connectionString = connectionString.Replace("[Database]", databaseName);
            connectionString = connectionString.Replace("[Password]", this.password);
            connectionString = connectionString.Replace("[Port]", this.port.ToString());

            return new NpgsqlConnectionStringBuilder(connectionString);
        }
    }
}
