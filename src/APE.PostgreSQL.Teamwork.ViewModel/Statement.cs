// <copyright file="Statement.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.IO;
using System.Text;
using APE.PostgreSQL.Teamwork.Model.Templates;
using APE.PostgreSQL.Teamwork.Model.Utils;
using Npgsql;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    /// <summary>
    /// Contains a statement, an error message and a result.
    /// </summary>
    public partial class Statement : IStatement
    {
        private readonly IDatabase database;

        /// <summary>
        /// Initializes the SQL statement.
        /// </summary>
        public Statement(string searchPath, string sql, IDatabase database)
        {
            this.SearchPath = searchPath;

            if (sql.Contains($"{SQLTemplates.PostgreSQLTeamworkSchemaName.QuoteName()}.")
                || searchPath.Contains(SQLTemplates.PostgreSQLTeamworkSchemaName.QuoteName()))
            {
                this.IsTeamworkSchema = true;
            }

            if (sql.ToLower().Contains("alter type")
                && sql.ToLower().Contains("add value"))
            {
                this.SupportsTransaction = false;
                var sb = new StringBuilder();
                sb.AppendLine(this.SearchPath);
                sb.AppendLine(sql);
                this.SQL = sb.ToString();
            }
            else
            {
                this.SupportsTransaction = true;
                this.SQL = sql;
            }

            this.Title = this.GetTitle(sql);
            this.database = database;
        }

        public string SearchPath { get; private set; }

        public bool SupportsTransaction { get; set; }

        public bool IsTeamworkSchema { get; private set; }

        /// <summary>
        /// Executes the SQL statement on the database which was set through the constructor.
        /// </summary>
        /// <exception cref="NpgsqlException">Is thrown when the statement contains an error.</exception>
        public void Execute() => this.database.ExecuteCommandNonQuery(this.SQL);

        private string GetTitle(string sql)
        {
            using var sr = new StringReader(sql);
            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                var isComment = line.Contains("--") || line.Contains("\\*");
                var isEmpty = string.IsNullOrWhiteSpace(line);

                if (line.Trim().EndsWith("("))
                    line = line.Trim()[..(line.Length - 1)];

                if (!isComment && !isEmpty)
                    return line;
            }

            return string.Empty;
        }
    }
}
