// <copyright file="Program.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.IO;
using APE.PostgreSQL.Teamwork.Model;
using log4net;

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
            Console.WriteLine("Start configuring Log4Net");
            ConfigureLog4Net();

            var commandLineInterpreter = new CommandLineExecutor();
            return commandLineInterpreter.Execute(args);
        }

        private static void ConfigureLog4Net()
        {
            ConfigureLog4Net(System.Reflection.Assembly.GetCallingAssembly().Location + ".log4net.config");
        }

        /// <summary>
        /// Configures Log4Net.
        /// Default is by using the config file Assembly.exe.log4net.config in the application directory.
        /// If a file 'Assembly.exe.MachineName.log4net.config' is found, this used in the first place.
        /// </summary>
        private static void ConfigureLog4Net(string log4NetConfigFile)
        {
            if (File.Exists(log4NetConfigFile + "." + Environment.MachineName))
            {
                log4NetConfigFile += "." + Environment.MachineName;
            }

            if (!File.Exists(log4NetConfigFile))
            {
                throw new Exception(log4NetConfigFile + " does not exist.");
            }

            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(log4NetConfigFile));

            var logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            logger.Info("Log4net is configured by using file: " + log4NetConfigFile);
        }
    }
}
