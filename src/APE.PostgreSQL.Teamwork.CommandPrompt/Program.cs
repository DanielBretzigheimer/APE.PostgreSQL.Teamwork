// <copyright file="Program.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>

using Serilog;

namespace APE.PostgreSQL.Teamwork.CommandPrompt
{
    /// <summary>
    /// Allows the user to execute the tool with the command prompt.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Starting point when executing the application via a command prompt.
        /// </summary>
        /// <param name="args">Should be parse able by the <see cref="CommandLineArgs"/>.</param>
        /// <returns>1 if the operation failed or 0 if no error occurred.</returns>
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                   .MinimumLevel.Debug()
                   .WriteTo.File("logs/APE.PostgreSQL.Teamwork.txt", rollingInterval: RollingInterval.Day)
                   .CreateLogger();

            Log.Information("Application starting");

            var commandLineInterpreter = new CommandLineExecutor();
            return commandLineInterpreter.Execute(args);
        }
    }
}
