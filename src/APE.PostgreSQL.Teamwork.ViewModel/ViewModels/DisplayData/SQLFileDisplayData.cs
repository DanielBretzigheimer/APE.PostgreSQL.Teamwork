// <copyright file="sqlfiledisplaydata.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Collections.ObjectModel;
using APE.CodeGeneration.Attributes;
using APE.PostgreSQL.Teamwork.Model;
using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    /// <summary>
    /// Displays an SQL File and contains a list of <see cref="StatementDisplayData"/>
    /// </summary>
    [NotifyProperty(typeof(ObservableCollection<StatementDisplayData>), "SQLStatements")]
    [NotifyProperty(typeof(SQLFile), "SQLFile")]
    [NotifyProperty(AccessModifier.PublicGetPrivateSet, typeof(ErrorStatus), "Status")]
    [NotifyPropertySupport]
    public partial class SQLFileDisplayData
    {
        public SQLFileDisplayData(SQLFile file)
        {
            this.SQLFile = file;

            this.Status = ErrorStatus.Unknown;
            this.SQLStatements = new ObservableCollection<StatementDisplayData>();
            foreach (var statement in file.SQLStatements)
                this.SQLStatements.Add(new StatementDisplayData(statement));
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
            foreach (var statement in this.SQLFile.SQLStatements)
                this.SQLStatements.Add(new StatementDisplayData(statement));
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
    }
}
