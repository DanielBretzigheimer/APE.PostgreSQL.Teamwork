// <copyright file="ISQLFile.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using APE.PostgreSQL.Teamwork.Model;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    /// <summary>
    /// Contains Properties for SQL Files and the SQL Statements from it.
    /// </summary>
    public interface ISQLFile
    {
        /// <summary>
        /// The name of the file.
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// The path to the file.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// The type of the file (diff, dump, undo diff...).
        /// </summary>
        FileType FileType { get; }

        /// <summary>
        /// List with all SQL statements.
        /// </summary>
        IEnumerable<IStatement> SQLStatements { get; }

        /// <summary>
        /// Database version of this file.
        /// </summary>
        DatabaseVersion Version { get; }

        /// <summary>
        /// Executes the file in one transaction.
        /// </summary>
        void ExecuteInTransaction();

        /// <summary>
        /// Marks the file as executed.
        /// </summary>
        /// <remarks>Should only be called when user exports its own changes.</remarks>
        void MarkAsExecuted();
    }
}
