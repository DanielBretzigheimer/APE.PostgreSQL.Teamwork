// <copyright file="IConnectionManager.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>

using System;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    public interface IConnectionManager : IDisposable
    {
        /// <summary>
        /// Checks if a connection to the default postgres database can be established.
        /// </summary>
        /// <returns>Result of the connection.</returns>
        bool CheckConnection();

        /// <summary>
        /// Checks if a connection to the given database name can be established.
        /// </summary>
        /// <param name="databaseName">Database name to which it tries to connect.</param>
        /// <returns>Result of the connection.</returns>
        bool CheckConnection(string databaseName);

        /// <summary>
        /// Executes the given SQL command on the given database.
        /// </summary>
        /// <typeparam name="T">Database type to which the returned data can be parsed.</typeparam>
        /// <param name="database">Database on which the SQL gets executed.</param>
        /// <param name="sql">SQL which is executed.</param>
        /// <returns>The parsed data from the database.</returns>
        System.Collections.Generic.List<T> ExecuteCommand<T>(IDatabase database, string sql);

        /// <summary>
        /// Executes the given SQL command on the default database which is set in the settings.
        /// </summary>
        /// <typeparam name="T">Database type to which the returned data can be parsed.</typeparam>
        /// <param name="sql">SQL which is executed.</param>
        /// <returns>The parsed data from the database.</returns>
        System.Collections.Generic.List<T> ExecuteCommand<T>(string sql);

        /// <summary>
        /// Executes the given command without requesting a return value.
        /// </summary>
        /// <param name="database">Database on which the SQL gets executed.</param>
        /// <param name="sql">SQL which is executed.</param>
        void ExecuteCommandNonQuery(IDatabase database, string sql);

        /// <summary>
        /// Executes the given SQL command without a return value on the default database defined in the settings.
        /// </summary>
        /// <remarks>Max time is 10 minutes, after that a timeout exception is thrown.</remarks>
        /// <param name="sql">SQL Command which is executed.</param>
        void ExecuteCommandNonQuery(string sql);

        /// <summary>
        /// Initializes the connection manager with the given data.
        /// </summary>
        void Initialize(string id, string host, string password, int port);

        /// <summary>
        /// This clears the NPGSQL pool so no stored connection is reused.
        /// </summary>
        /// <remarks>Only call this if the old connection was terminated.</remarks>
        void ClearPools();
    }
}
