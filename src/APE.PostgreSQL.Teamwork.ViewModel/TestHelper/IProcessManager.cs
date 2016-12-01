// <copyright file="IProcessManager.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APE.PostgreSQL.Teamwork.ViewModel.TestHelper
{
    /// <summary>
    /// Manages all processes.
    /// </summary>
    public interface IProcessManager
    {
        /// <summary>
        /// Executes a new process with the given <see cref="ProcessStartInfo"/>
        /// </summary>
        /// <param name="startInfo">Which is forwarded to the process.</param>
        void Execute(ProcessStartInfo startInfo);

        /// <summary>
        /// Executes a new process with the given <see cref="ProcessStartInfo"/>
        /// in the background.
        /// </summary>
        /// <param name="startInfo">Which is forwarded to the process.</param>
        void ExecuteAsync(ProcessStartInfo startInfo);

        /// <summary>
        /// Starts a new process of the default application to open this file.
        /// </summary>
        /// <param name="file">File which is opened.</param>
        void Start(string file);
    }
}
