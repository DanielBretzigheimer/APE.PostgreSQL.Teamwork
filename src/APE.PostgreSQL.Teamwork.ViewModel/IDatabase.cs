// <copyright file="IDatabase.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using APE.PostgreSQL.Teamwork.Model;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    public interface IDatabase
    {
        /// <summary>
        /// Gets the name of the database.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the path of the databases diff and dump files.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Gets the progress which indicates the status of the database while executing
        /// things in the background.
        /// </summary>
        double Progress { get; }

        /// <summary>
        /// Gets the current version of the database which is read from the postgres schema.
        /// </summary>
        DatabaseVersion CurrentVersion { get; }

        /// <summary>
        /// Gets the highest applicable version for this database.
        /// </summary>
        DatabaseVersion LastApplicableVersion { get; }

        /// <summary>
        /// Gets a list of all undo diff files.
        /// </summary>
        System.Collections.ObjectModel.ObservableCollection<SQLFile> UndoDiffFiles { get; }

        /// <summary>
        /// Gets a list of all diff files.
        /// </summary>
        System.Collections.ObjectModel.ObservableCollection<SQLFile> DiffFiles { get; }

        /// <summary>
        /// Creates a new dump of the database.
        /// </summary>
        /// <param name="dumpCreatorPath">Path to the postgres dump exe with which a dump is created.</param>
        /// <param name="host">Server host to which is connected.</param>
        /// <param name="id">Server id for the dump creation.</param>
        /// <param name="password">Server password for the dump creation.</param>
        /// <returns>The created dump as <see cref="SQLFile"/>.</returns>
        SQLFile CreateDump(string path, string dumpCreatorPath, string host, string id, string password);

        /// <summary>
        /// Gets all <see cref="SQLFile"/>s which needs to be applied to get to a given version.
        /// </summary>
        /// <param name="version">The target version.</param>
        /// <returns>A list of all files which needs to be executed to get to the target version.</returns>
        IEnumerable<SQLFile> GetToBeAppliedSQLFiles(DatabaseVersion version);

        /// <summary>
        /// Creates the teamwork schema if it does not exist already.
        /// </summary>
        void CreateTeamworkSchema();

        /// <summary>
        /// Exports the changes of the given database.
        /// </summary>
        /// <param name="dumpCreatorPath">Path to the postgres dump exe.</param>
        /// <param name="host">Server host to which is connected.</param>
        /// <param name="id">Server id for the diff creation.</param>
        /// <param name="password">Server password for the dump creation.</param>
        void Export(string dumpCreatorPath, string host, string id, string password);

        /// <summary>
        /// Reduces the version by 1.
        /// </summary>
        void ReduceVersion();

        /// <summary>
        /// Clears all data and structure from the database.
        /// </summary>
        void Reset();

        /// <summary>
        /// Tests all SQL files.
        /// </summary>
        void TestSQLFiles();

        /// <summary>
        /// Updates the version of the database and scans the path for new files.
        /// </summary>
        void UpdateData(bool force = false);

        /// <summary>
        /// Up or downgrades the database to the given version by executing
        /// diffs or undo diffs.
        /// </summary>
        /// <param name="version">To which the database gets up or downgraded.</param>
        /// <param name="afterFileExecution">Action which is called after a file was executed with a List of all SQLFiles 
        /// (<see cref="IEnumerable{SQLFile}"/>) and the index of the currently executed one.</param>
        /// <exception cref="TeamworkConnectionException">Is thrown when an error occurred while executing the SQL Statements.</exception>
        void UpdateToVersion(DatabaseVersion version, Action<IEnumerable<SQLFile>, SQLFile> afterFileExecution = null);

        /// <summary>
        /// Executes the given SQL command on the given database.
        /// </summary>
        /// <typeparam name="T">Database type to which the returned data can be parsed.</typeparam>
        /// <param name="database">Database on which the SQL gets executed.</param>
        /// <param name="sql">SQL which is executed.</param>
        /// <returns>The parsed data from the database.</returns>
        List<T> ExecuteCommand<T>(string sql);

        /// <summary>
        /// Executes the given command without requesting a return value.
        /// </summary>
        /// <param name="database">Database on which the SQL gets executed.</param>
        /// <param name="sql">SQL which is executed.</param>
        void ExecuteCommandNonQuery(string sql);
    }
}
