﻿// <copyright file="ProcessManager.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APE.PostgreSQL.Teamwork.ViewModel.TestHelper
{
    public class ProcessManager : IProcessManager
    {
        public void Execute(ProcessStartInfo startInfo)
        {
            this.Execute(startInfo, false);
        }

        public void ExecuteAsync(ProcessStartInfo startInfo)
        {
            this.Execute(startInfo, true);
        }

        public void Start(string file)
        {
            Process.Start(file);
        }

        private void Execute(ProcessStartInfo startInfo, bool async)
        {
            var process = new Process();

            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            process.StartInfo = startInfo;
            process.Start();

            if (!async)
            {
                process.WaitForExit();
                process.Close();
            }
        }
    }
}
