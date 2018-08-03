// <copyright file="CommandLineExecutor.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using APE.PostgreSQL.Teamwork.Model;
using APE.PostgreSQL.Teamwork.ViewModel;
using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;
using APE.PostgreSQL.Teamwork.ViewModel.Postgres;
using CommandLine;

namespace APE.PostgreSQL.Teamwork.CommandPrompt
{
    /// <summary>
    /// Parses the command line args and executes all files for the given database.
    /// </summary>
    public class CommandLineExecutor
    {
        /// <summary>
        /// Updates the database in the <see cref="CommandLineArgs"/> to the given version with the given parameters.
        /// </summary>
        /// <returns>An <see cref="int"/> which indicates if the operation was successful. "1" means an error occurred.</returns>
        public int Execute(CommandLineArgs commandLineArgs)
        {
            Console.WriteLine("Given parameters:");
            Console.WriteLine(commandLineArgs);

            var connectionManager = new ConnectionManager();
            var fileSystemAccess = new FileSystemAccess();
            var processManager = new ViewModel.TestHelper.ProcessManager();
            var diffCreator = new DifferenceCreator();
            var sqlFileTester = new SQLFileTester();

            // initilialize connection and database
            connectionManager.Initialize(commandLineArgs.Username, commandLineArgs.Host, commandLineArgs.Password, commandLineArgs.Port);
            var database = new Database(
                commandLineArgs.DatabaseName,
                commandLineArgs.FilePath,
                DatabaseSetting.DefaultIgnoredSchemas,
                connectionManager,
                fileSystemAccess,
                processManager,
                diffCreator,
                sqlFileTester,
                true);
            var oldVersion = database.CurrentVersion;
            Console.WriteLine(string.Format("{0}Old Version was {1}", Environment.NewLine, oldVersion));

            database.UpdateData();

            try
            {
                var targetVersion = database.LastApplicableVersion;
                if (commandLineArgs.FullTargetVersion != null)
                {
                    targetVersion = DatabaseVersion.CommandLineVersion(commandLineArgs.FullTargetVersion);
                }
                else if (commandLineArgs.TargetVersion != null)
                {
                    targetVersion = new DatabaseVersion((int)commandLineArgs.TargetVersion);
                }

                if (targetVersion == database.CurrentVersion)
                {
                    Console.WriteLine(string.Format("Database is already on target version {0}", targetVersion));
                    return 0;
                }

                Console.WriteLine("Start Executing...");

                // execute the diffs
                database.UpdateToVersion(targetVersion);
                database.UpdateData();
                Console.WriteLine(string.Format("Finished Executing. New Version: {0}", database.CurrentVersion));

                // 0 means no error for the setup
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Exception occured: {0}", ex.Message));

                if (ex is TeamworkConnectionException teamworkException)
                {
                    if (teamworkException.File != null)
                    {
                        Console.WriteLine(string.Format("In file: {0}", teamworkException.File.Path));
                    }
                }

                try
                {
                    Console.WriteLine(string.Format("Start rollback to version {0}", oldVersion));

                    // executes the undo diffs to go back to the previous version
                    database.UpdateToVersion(oldVersion);
                    Console.WriteLine(string.Format("Rollback finished with 0 errors"));
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(string.Format("FATAL: Error while rollback was running. {0}", e.Message));
                    Console.ResetColor();
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(string.Format("Error while executing diffs. {0}", ex.Message));
                Console.ResetColor();
            }

            return 1;
        }

        /// <summary>
        /// Parses the args and executes the SQL statements of the database specified in them.
        /// </summary>
        /// <param name="args">Args which contain the database and additional info.</param>
        public int Execute(string[] args)
        {
            // 1 means an error occured for the setup
            var result = 1;
            Parser.Default.ParseArguments<CommandLineArgs>(args)
                .WithParsed(commandLineArgs => this.Execute(commandLineArgs))
                .WithNotParsed((errs) => this.HandleParseError(errs));

            return result;
        }

        private void HandleParseError(IEnumerable<Error> errors)
        {
            Console.WriteLine("Could not parse arguments");
        }
    }
}
