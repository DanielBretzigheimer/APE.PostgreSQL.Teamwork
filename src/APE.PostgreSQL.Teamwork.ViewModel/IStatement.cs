// <copyright file="IStatement.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
namespace APE.PostgreSQL.Teamwork.ViewModel
{
    /// <summary>
    /// Contains a statement, an error message and a result.
    /// </summary>
    public interface IStatement
    {
        /// <summary>
        /// The SQL of the statement.
        /// </summary>
        string SQL { get; }

        /// <summary>
        /// Gets a bool indicating that the statement contains an alter type
        /// which means it cannot be executed in transaction.
        /// </summary>
        bool SupportsTransaction { get; }

        /// <summary>
        /// Executes the SQL Statement.
        /// </summary>
        void Execute();
    }
}
