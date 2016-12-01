// <copyright file="SearchPathHelper.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.IO;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres
{
    /// <summary>
    /// Helps to output search path only if it was not output yet.
    /// </summary>
    public class SearchPathHelper
    {
        /// <summary>
        /// Statement to output.
        /// </summary>
        private readonly string searchPath;

        /// <summary>
        /// Flag determining whether the statement was already output.
        /// </summary>
        private bool wasOutput;

        /// <summary>
        /// Creates new instance of SearchPathHelper.
        /// </summary>
        public SearchPathHelper(string searchPath)
        {
            this.searchPath = searchPath;
        }

        /// <summary>
        /// Outputs search path if it was not output yet.
        /// </summary>
        public void OutputSearchPath(StreamWriter writer)
        {
            if (!this.wasOutput && this.searchPath != null && this.searchPath.Length > 0)
            {
                writer.WriteLine();
                writer.WriteLine(this.searchPath);
                this.wasOutput = true;
            }
        }
    }
}