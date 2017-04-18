// <copyright file="ImportWindowViewModel.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using APE.CodeGeneration.Attributes;
using APE.PostgreSQL.Teamwork.Model;
using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;
using APE.PostgreSQL.Teamwork.ViewModel.TestHelper;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    /// <summary>
    /// ViewModel for the ImportWindowView which shows a list with all
    /// <see cref="SQLFileDisplayData"/> and another list with all <see cref="StatementDisplayData"/>
    /// which the user can execute.
    /// </summary>
    [NotifyProperty(typeof(List<SQLFileDisplayData>), "DiffFiles")]
    [NotifyProperty(typeof(SQLFileDisplayData), "SelectedDiffFile")]
    [NotifyProperty(typeof(bool), "ExecuteButtonEnabled")]
    [NotifyProperty(AccessModifier.Public, typeof(bool), "ExecuteButtonVisible", true)]
    [NotifyProperty(AccessModifier.Public, typeof(bool), "Loading", false)]
    [CtorParameter(typeof(IProcessManager))]
    [CtorParameter(AccessModifier.Public, typeof(DatabaseDisplayData), "SelectedDatabase")]
    public partial class ImportWindowViewModel : BaseViewModel
    {
        /// <summary>
        /// DESIGN TIME CONSTRUCTOR.
        /// </summary>
        public ImportWindowViewModel()
        {
        }

        /// <summary>
        /// Gets a <see cref="ICommand"/> which will execute all <see cref="SQLFileDisplayData"/> where <see cref="SQLFileDisplayData.IsSelected"/>
        /// is set.
        /// </summary>
        public ICommand ExecuteFileCommand { get; private set; }

        /// <summary>
        /// Gets a <see cref="ICommand"/> which will open all <see cref="SQLFileDisplayData"/> where <see cref="SQLFileDisplayData.IsSelected"/>
        /// is set in the default program to open SQL files.
        /// </summary>
        public ICommand OpenFileCommand { get; private set; }

        /// <summary>
        /// Gets a <see cref="ICommand"/> which will refresh all <see cref="SQLFileDisplayData"/>.
        /// </summary>
        public ICommand RefreshFileCommand { get; set; }

        /// <summary>
        /// Gets a <see cref="ICommand"/> which will execute all <see cref="SQLFileDisplayData"/>.
        /// </summary>
        public ICommand ExecuteCommand { get; private set; }

        /// <summary>
        /// Gets a <see cref="ICommand"/> which will mark all <see cref="SQLFileDisplayData"/> where <see cref="SQLFileDisplayData.IsSelected"/>
        /// is set as executed which will mean they will disappear from the list of not executed files.
        /// </summary>
        public ICommand MarkAsExecutedCommand { get; private set; }

        partial void ImportWindowViewModelCtor()
        {
            var uiDispatcher = Dispatcher.CurrentDispatcher;

            this.CreateCommands(uiDispatcher);

            // update data
            this.ExecuteInTask(() =>
            {
                this.Loading = true;
                this.SelectedDatabase.UpdateData(force: true);

                foreach (var file in this.SelectedDatabase.ApplicableSQLFiles)
                    uiDispatcher.Invoke(() => file.Refresh());

                this.SelectedDatabase.ResetTargetVersion();
                this.Loading = false;
                this.ExecuteButtonEnabled = true;
            });
        }

        private void CreateCommands(Dispatcher uiDispatcher)
        {
            this.ExecuteCommand = new RelayCommand(() =>
            {
                this.ExecuteInTask(
                    () =>
                    {
                        this.ExecuteButtonEnabled = false;
                        this.ExecuteFiles();
                        this.ExecuteButtonEnabled = true;
                    },
                    (loading) => this.Loading = loading);
            });

            this.ExecuteFileCommand = new RelayCommand(() =>
            {
                this.ExecuteInTask(
                    () =>
                    {
                        this.ExecuteButtonEnabled = false;
                        this.SelectedDatabase.UpdateData();

                        var selectedFiles = this.SelectedDatabase.ApplicableSQLFiles.Where(f => f.IsSelected);
                        foreach (var file in selectedFiles)
                        {
                            try
                            {
                                file.ExecuteInTransaction();
                            }
                            catch (TeamworkConnectionException ex)
                            {
                                ShowDialog(GetMessageBox(string.Format("Error while executing file {0}", ex.File.Path), "Execution failed", MessageBoxButton.OK)).Wait();
                                Log.Warn(string.Format("Error while executing file {0}", ex.File.Path), ex);
                            }
                        }

                        this.SelectedDatabase.UpdateData();
                        this.ExecuteButtonEnabled = true;
                    },
                    (loading) => this.Loading = loading);
            });

            this.OpenFileCommand = new RelayCommand(() =>
            {
                var selectedFiles = this.SelectedDatabase.ApplicableSQLFiles.Where(f => f.IsSelected);
                foreach (var file in selectedFiles)
                {
                    this.processManager.Start(file.SQLFile.Path);
                }
            });

            this.RefreshFileCommand = new RelayCommand(() =>
            {
                this.ExecuteInTask(() =>
                {
                    var selectedFiles = this.SelectedDatabase.ApplicableSQLFiles.Where(f => f.IsSelected);
                    foreach (var file in selectedFiles)
                    {
                        uiDispatcher.Invoke(() => file.Refresh());
                    }
                });
            });

            this.MarkAsExecutedCommand = new RelayCommand(() =>
            {
                var selectedFiles = this.SelectedDatabase.ApplicableSQLFiles.Where(f => f.IsSelected);
                foreach (var file in selectedFiles)
                {
                    file.SQLFile.MarkAsExecuted();
                }

                this.SelectedDatabase.UpdateData();
            });
        }

        /// <summary>
        /// Executes all statements in all files.
        /// </summary>
        private async void ExecuteFiles()
        {
            this.SelectedDatabase.UpdateData();

            if (this.SelectedDatabase.ApplicableSQLFiles.Count <= 0)
            {
                await BaseViewModel.ShowDialog(BaseViewModel.GetMessageBox(
                    @"All found files were already executed and database is on the selected Version. Try changing the target version to go to another version.",
                    "Database up-to-date",
                    MessageBoxButton.OK));
                return;
            }

            var oldVersion = this.SelectedDatabase.Database.CurrentVersion;

            try
            {
                this.SelectedDatabase.UpdateToVersion(this.SelectedDatabase.TargetVersion);
                await BaseViewModel.ShowDialogWithCloseHandler(BaseViewModel.GetMessageBox("All SQL Files succesfully executed! Do you want to close the window?", "Succesfully Executed", MessageBoxButton.YesNo), this.SuccesfullyExecutedMessageBoxClosing);
                Log.Info("All sql files succesfully executed");
            }
            catch (Exception ex)
            {
                var errorFile = this.SelectedDatabase.ApplicableSQLFiles.FirstOrDefault(f => f.Status == ErrorStatus.Error);
                var path = "unknown";
                if (errorFile != null)
                {
                    path = errorFile.SQLFile.Path;
                }

                var message = string.Format("Error in file {0}: {1}\n\nStart rolling back to version {2}", path, ex.Message, oldVersion);
                BaseViewModel.ShowDialog(BaseViewModel.GetMessageBox(message, "Execution failed", MessageBoxButton.OK)).Wait();
                Log.Warn(string.Format("Error while executing files. Rolling back to version {0}", oldVersion), ex);

                try
                {
                    this.SelectedDatabase.UpdateToVersion(oldVersion);
                }
                catch (Exception rollbackEx)
                {
                    var rollbackMessage = $"Rollback to version {oldVersion} did not work. Error in file {path}: {rollbackEx.Message}";
                    BaseViewModel.ShowDialog(BaseViewModel.GetMessageBox(rollbackMessage, "Rollback failed", MessageBoxButton.OK)).Wait();
                    Log.Fatal("Rollback did not work. This means the database is in an unknown status! This can only be solved by manually checking the database!");
                }
            }

            this.SelectedDatabase.UpdateData();
        }

        private void SuccesfullyExecutedMessageBoxClosing(MaterialMessageBoxResult result)
        {
            if (result == MaterialMessageBoxResult.Yes)
            {
                ImportWindowViewModel.CloseView();
            }
        }
    }
}
