// <copyright file="CommandLineArgs.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Linq;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace APE.PostgreSQL.Teamwork.CommandPrompt
{
    /// <summary>
    /// Contains all possible command line args.
    /// </summary>
    public class CommandLineArgs
    {
        /// <summary>
        /// Gets or sets the name of the Postgres SQL Database.
        /// </summary>
        [Option('d', "database", Required = true, HelpText = "Name of the PostgreSQL Database.")]
        public string DatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the name of the Postgres SQL user.
        /// </summary>
        [Option('u', "username", Required = true, HelpText = "Name of the PostgreSQL user.")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password for the database.
        /// </summary>
        [Option('a', "password", Required = true, HelpText = "Password for the database.")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the host (IP-Address) of the Postgres SQL database.
        /// </summary>
        [Option('h', "host", Required = true, HelpText = "The host (ip) of the PostgreSQL database.")]
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the port of the Postgres SQL database.
        /// </summary>
        [Option('p', "port", Required = true, HelpText = "The port of the PostgreSQL database.")]
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the path to the SQL files which will be executed.
        /// </summary>
        [Option('f', "filepath", Required = true, HelpText = "Path to the SQL files which will be executed.")]
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the target version to which the database will be upgraded or downgraded. This is a simple number. If not set the last applicable version will be set.
        /// </summary>
        [Option('v', "targetversion", Required = false, HelpText = "The target version to which the database will be upgraded or downgraded. This is a simple number. If not set the last applicable version will be set.")]
        [NullGuard.AllowNull]
        public int? TargetVersion { get; set; }

        /// <summary>
        /// Gets or sets the full target version to which the database will be upgraded or downgraded. Should look e.g. like \"0000.a\". If not set the last applicable version will be set.
        /// </summary>
        [Option('t', "fulltargetversion", Required = false, HelpText = "The full target version to which the database will be upgraded or downgraded. Should look e.g. like \"0000.a\". If not set the last applicable version will be set.")]
        [NullGuard.AllowNull]
        public string FullTargetVersion { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        [return: NullGuard.AllowNull]
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine("DatabaseName: " + this.DatabaseName);
            builder.AppendLine("Username: " + this.Username);
            builder.AppendLine("Password: ***HIDDEN***");
            builder.AppendLine("Host: " + this.Host);
            builder.AppendLine("Port: " + this.Port);
            builder.AppendLine("FilePath: " + this.FilePath);

            if (this.FullTargetVersion == null)
            {
                builder.AppendLine("TargetVersion: " + this.TargetVersion);
            }
            else
            {
                builder.AppendLine("FullTargetVersion: " + this.FullTargetVersion);
            }

            return builder.ToString();
        }
    }
}
