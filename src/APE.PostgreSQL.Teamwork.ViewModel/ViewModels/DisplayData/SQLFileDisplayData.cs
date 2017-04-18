// <copyright file="SQLFileDisplayData.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using APE.CodeGeneration.Attributes;
using APE.PostgreSQL.Teamwork.Model;
using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;
using APE.PostgreSQL.Teamwork.ViewModel.Postgres;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    /// <summary>
    /// Displays an SQL File and contains a list of <see cref="StatementDisplayData"/>
    /// </summary>
    [NotifyProperty(typeof(ObservableCollection<StatementDisplayData>), "SQLStatements")]
    [NotifyProperty(typeof(SQLFile), "SQLFile")]
    [NotifyProperty(AccessModifier.PublicGetPrivateSet, typeof(ErrorStatus), "Status")]
    [NotifyProperty(typeof(bool), "ShowWarning")]
    [NotifyProperty(typeof(string), "WarningText")]
    [NotifyPropertySupport]
    public partial class SQLFileDisplayData
    {
        public SQLFileDisplayData(SQLFile file)
        {
            this.SQLFile = file;

            this.WarningText = string.Empty;
            this.Status = ErrorStatus.Unknown;
            this.SQLStatements = new ObservableCollection<StatementDisplayData>();
            this.RefreshStatements();
        }

        /// <summary>
        /// Indicates if the file is selected.
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Refreshes the SQL Files SQL Statements.
        /// </summary>
        public void Refresh()
        {
            this.SQLFile.Refresh();
            this.SQLStatements.Clear();
            this.RefreshStatements();
        }

        public void ExecuteInTransaction()
        {
            try
            {
                this.SQLFile.ExecuteInTransaction();
                this.Status = ErrorStatus.Successful;
            }
            catch (TeamworkConnectionException)
            {
                this.Status = ErrorStatus.Error;
                throw;
            }
        }

        /// <summary>
        ///  Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return this.SQLFile.ToString();
        }

        private void RefreshStatements()
        {
            foreach (var statement in this.SQLFile.SQLStatements)
            {
                this.SQLStatements.Add(new StatementDisplayData(statement));
            }

            var statementsWithWarning = this.SQLStatements.Where(s => s.Statement.SQL.Contains(DifferenceCreator.WarningMessagePrefix));
            var newWarningText = new StringBuilder();
            if (statementsWithWarning.Count() > 0)
            {
                this.ShowWarning = true;

                foreach (var statementWithWarning in statementsWithWarning)
                {
                    var sql = statementWithWarning.Statement.SQL;
                    var warningStart = sql.IndexOf(DifferenceCreator.WarningMessagePrefix);
                    var warningLength = sql.Substring(warningStart).IndexOf(Environment.NewLine);

                    var warning = sql.Substring(warningStart + DifferenceCreator.WarningMessagePrefix.Length, warningLength - DifferenceCreator.WarningMessagePrefix.Length);
                    if (!newWarningText.ToString().Contains(warning))
                    {
                        newWarningText.AppendLine(warning);
                    }
                }
            }

            this.WarningText = newWarningText.ToString();
        }
    }
}
