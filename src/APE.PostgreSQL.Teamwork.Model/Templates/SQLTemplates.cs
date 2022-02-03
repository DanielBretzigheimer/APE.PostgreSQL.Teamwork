// <copyright file="SQLTemplates.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>

using System.Reflection;

namespace APE.PostgreSQL.Teamwork.Model.Templates
{
    /// <summary>
    /// Contains predefined SQL Statements which are read from the file system and can be accessed via these functions.
    /// </summary>
    public static class SQLTemplates
    {
        /// <summary>
        /// The file extension for a dump file.
        /// </summary>
        public const string DumpFile = ".dump.sql";

        /// <summary>
        /// The file extension for a diff file.
        /// </summary>
        public const string DiffFile = ".diff.sql";

        /// <summary>
        /// The file extension for a undo diff file.
        /// </summary>
        public const string UndoDiffFile = ".undoDiff.sql";

        /// <summary>
        /// The postgres SQL string to start a comment.
        /// </summary>
        public const string Comment = "--";

        /// <summary>
        /// The name of the postgres SQL teamwork schema.
        /// </summary>
        public const string PostgreSQLTeamworkSchemaName = "APE.PostgreSQL.Teamwork";

        private static readonly string GetTables;
        private static readonly string createDatabase;
        private static readonly string createTeamworkSchema;
        private static readonly string disconnectDatabase;
        private static readonly string dropDatabase;
        private static readonly string getSchema;
        private static readonly string renameDatabase;
        private static readonly string getAppliedVersions;
        private static readonly string removeVersion;
        private static readonly string addExecutedFile;

        static SQLTemplates()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Templates";

            // load statements from files
            createDatabase = Properties.Resources.CreateDatabase;
            createTeamworkSchema = Properties.Resources.CreateTeamworkSchema;
            disconnectDatabase = Properties.Resources.DisconnectDatabase;
            dropDatabase = Properties.Resources.DropDatabase;
            getSchema = Properties.Resources.GetSchema;
            renameDatabase = Properties.Resources.RenameDatabase;
            getAppliedVersions = Properties.Resources.GetAppliedVersions;
            removeVersion = Properties.Resources.RemoveVersion;
            addExecutedFile = Properties.Resources.AddExecutedFile;
            GetTables = Properties.Resources.GetTables;

            if (createDatabase == null
                || createTeamworkSchema == null
                || disconnectDatabase == null
                || dropDatabase == null
                || getSchema == null
                || renameDatabase == null
                || getAppliedVersions == null
                || removeVersion == null
                || addExecutedFile == null
                || GetTables == null)
            {
                throw new FileNotFoundException("Properties.Resources not found");
            }
        }

        /// <summary>
        /// Gets SQL to disconnect a given database.
        /// </summary>
        public static string DisconnectDatabase(string database) => disconnectDatabase.Replace("[Database]", database);

        /// <summary>
        /// Gets SQL to drop database.
        /// <remarks>be sure to disconnect database before drop</remarks>
        /// </summary>
        public static string DropDatabase(string database) => dropDatabase.Replace("[Database]", database);

        /// <summary>
        /// Gets SQL to create a database with the given name.
        /// </summary>
        public static string CreateDatabase(string database) => createDatabase.Replace("[Database]", database);

        /// <summary>
        /// Gets a SQL statement which finds a schema in one database.
        /// </summary>
        public static string GetTeamworkSchemaSQL(string database)
        {
            return getSchema.Replace("[Schema]", PostgreSQLTeamworkSchemaName)
                .Replace("[Database]", database);
        }

        /// <summary>
        /// Creates a SQL statement which can create the teamwork schema.
        /// </summary>
        /// <returns>The SQL as string which can be executed on the database.</returns>
        public static string CreateTeamworkSchemaSQL()
        {
            return createTeamworkSchema
                .Replace("[Schema]", PostgreSQLTeamworkSchemaName);
        }

        /// <summary>
        /// Creates a SQL statement which can rename a database from a given name to another.
        /// </summary>
        /// <param name="newDB">The new name of the database.</param>
        /// <param name="oldDB">The old name of the database.</param>
        /// <returns>The SQL as string which can be executed on the database.</returns>
        public static string GetRenameDatabaseSQL(string newDB, string oldDB)
        {
            var retVal = renameDatabase.Replace("[NewDatabaseName]", newDB);
            retVal = retVal.Replace("[OldDatabaseName]", oldDB);
            return retVal;
        }

        /// <summary>
        /// Creates a SQL statement which can remove a version from the executed statements table in the teamwork schema.
        /// </summary>
        /// <param name="version">The version which is removed.</param>
        /// <returns>The SQL as string which can be executed on the database.</returns>
        public static string RemoveVersion(DatabaseVersion version)
        {
            return removeVersion
                .Replace("[LastAppliedVersion]", version.Full)
                .Replace("[Schema]", PostgreSQLTeamworkSchemaName);
        }

        /// <summary>
        /// Creates a SQL statement which gets the already applied versions.
        /// </summary>
        /// <returns>The SQL as string which can be executed on the database.</returns>
        public static string GetAppliedVersions()
        {
            return getAppliedVersions
                .Replace("[Schema]", PostgreSQLTeamworkSchemaName);
        }

        /// <summary>
        /// Creates a SQL statement which lets you add a row in the executed file table of the teamwork schema.
        /// </summary>
        /// <param name="version">Version of the SQL file.</param>
        /// <param name="fileType">The type of the file (diff, dump...).</param>
        /// <param name="message">An optional message which is added into the database table.</param>
        /// <returns>The SQL as string which can be executed on the database.</returns>
        public static string AddExecutedFileSql(DatabaseVersion version, FileType fileType, string message = "")
        {
            // ignores the whole statement which add the file to the ExecutedFiles if it is an undo diff
            var ignore = string.Empty;
            if (fileType == FileType.UndoDiff)
            {
                ignore = Comment;
            }

            return addExecutedFile
                .Replace("[Schema]", PostgreSQLTeamworkSchemaName)
                .Replace("[Version]", version.Full)
                .Replace("[Time]", DateTime.Now.ToString("dd.MM.yyyy hh:mm"))
                .Replace("[FileType]", fileType.ToString())
                .Replace("[Message]", message)
                .Replace("[Ignore]", ignore);
        }

        /// <summary>
        /// Gets a SQL string which can add a row into the execution history.
        /// </summary>
        /// <param name="version">Version of the SQL file.</param>
        /// <param name="fileType">The type of the file (diff, dump...).</param>
        /// <param name="message">An optional message which is added into the database table.</param>
        public static string AddExecutionHistorySql(DatabaseVersion version, FileType fileType, string message = "")
        {
            return addExecutedFile
                .Replace("[Schema]", PostgreSQLTeamworkSchemaName)
                .Replace("[Version]", version.Full)
                .Replace("[Time]", DateTime.Now.ToString("dd.MM.yyyy hh:mm"))
                .Replace("[FileType]", fileType.ToString())
                .Replace("[Message]", message)
                .Replace("[Ignore]", Comment);
        }

        /// <summary>
        /// Gets a SQL string which can request all tables from the database.
        /// </summary>
        public static string GetAllTables() => GetTables;
    }
}
