// <copyright file="Statement.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.IO;
using APE.CodeGeneration.Attributes;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    /// <summary>
    /// Contains a statement, an error message and a result.
    /// </summary>
    [NotifyProperty(AccessModifier.PublicGetPrivateSet, typeof(string), "SQL")]
    [NotifyProperty(AccessModifier.PublicGetPrivateSet, typeof(string), "Title")]
    [NotifyPropertySupport]
    public partial class Statement : IStatement
    {
        private IDatabase database = null;

        /// <summary>
        /// Initializes the SQL statement.
        /// </summary>
        public Statement(string sql, IDatabase database)
        {
            if (sql.ToLower().Contains("alter type")
                && sql.ToLower().Contains("add value"))
                this.SupportsTransaction = false;
            else
                this.SupportsTransaction = true;

            this.SQL = sql;
            this.Title = this.GetTitle(sql);
            this.database = database;
        }

        public bool SupportsTransaction { get; set; }

        /// <summary>
        /// Executes the SQL statement on the database which was set 
        /// through the constructor.
        /// </summary>
        /// <param name="preExecutedSql">Additional SQL which is executed before the statement.</param>
        /// <exception cref="NpgsqlException">Is thrown when the statement contains an error.</exception>
        public void Execute()
        {
            this.database.ExecuteCommandNonQuery(this.SQL);
        }

        private string GetTitle(string sql)
        {
            using (var sr = new StringReader(sql))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var isComment = line.Contains("--") || line.Contains("\\*");
                    var isEmpty = string.IsNullOrWhiteSpace(line);

                    if (line.Trim().EndsWith("("))
                        line = line.Trim().Substring(0, line.Length - 1);

                    if (!isComment
                        && !isEmpty)
                        return line;
                }
            }

            return string.Empty;
        }
    }
}