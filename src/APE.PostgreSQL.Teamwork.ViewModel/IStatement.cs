// <copyright file="IStatement.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    /// <summary>
    /// Contains a statement, an error message and a result.
    /// </summary>
    public interface IStatement
    {
        /// <summary>
        /// Gets the SQL of the <see cref="IStatement"/>.
        /// </summary>
        string SQL { get; }

        /// <summary>
        /// Gets the search path for this <see cref="IStatement"/>.
        /// </summary>
        string SearchPath { get; }

        /// <summary>
        /// Gets a bool, indicating that this <see cref="IStatement"/> contains an alter type which means it cannot
        /// be executed in transaction.
        /// </summary>
        bool SupportsTransaction { get; }

        /// <summary>
        /// Executes the <see cref="SQL"/> of this <see cref="IStatement"/>.
        /// </summary>
        void Execute();
    }
}
