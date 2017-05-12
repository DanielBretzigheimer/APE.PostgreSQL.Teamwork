// <copyright file="SearchPathHelper.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
using APE.PostgreSQL.Teamwork.Model.Utils;
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

        public SearchPathHelper()
        {
            this.searchPath = null;
        }

        /// <summary>
        /// Creates new instance of SearchPathHelper with the given search path.
        /// </summary>
        public SearchPathHelper(string searchPath)
        {
            this.searchPath = searchPath;
        }

        /// <summary>
        /// Creates new instance of SearchPathHelper with the search path set to the given schema.
        /// </summary>
        public SearchPathHelper(PgSchema schema)
            : this("SET search_path = " + schema.Name.GetQuotedName(true) + ", pg_catalog;")
        {
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