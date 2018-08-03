// <copyright file="IMainWindowViewModel.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;

namespace APE.PostgreSQL.Teamwork.ViewModel.ViewModels
{
    /// <summary>
    /// ViewModel for the MainWindowView which displays a list of
    /// databases.
    /// </summary>
    public interface IMainWindowViewModel
    {
        /// <summary>
        /// Is called when the view model is started.
        /// </summary>
        event EventHandler Started;

        /// <summary>
        /// Indicates if the view model is started.
        /// </summary>
        bool IsStarted { get; }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> indicating if the search bar should be shown.
        /// </summary>
        bool ShowSearch { get; set; }

        /// <summary>
        /// Starts the view model and lets it search for databases.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the async worker which updates and searches for databases.
        /// </summary>
        void Stop();

        /// <summary>
        /// Updates all databases.
        /// </summary>
        void UpdateDatabases();

        void ReorderDatabases(IEnumerable<DatabaseDisplayData> newDatabaseOrder);
    }
}
