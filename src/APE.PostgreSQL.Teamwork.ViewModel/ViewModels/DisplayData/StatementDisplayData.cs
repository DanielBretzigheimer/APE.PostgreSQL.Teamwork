// <copyright file="StatementDisplayData.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Text;
using System.Windows;
using System.Windows.Input;
using APE.CodeGeneration.Attributes;
using Npgsql;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    /// <summary>
    /// Represents a <see cref="Statement"/> for the GUI and contains
    /// an additional bool for the edit mode.
    /// </summary>
    [NotifyProperty(typeof(IStatement), "Statement")]
    [NotifyProperty(typeof(bool), "EditMode")]
    [NotifyPropertySupport]
    public partial class StatementDisplayData
    {
        private string originalSQL;

        /// <summary>
        /// Initializes the StatementDisplayData.
        /// </summary>
        /// <param name="statement">The SQL statement which is displayed.</param>
        /// <param name="updateFile">Is called when the user changed the SQL statement and gets the old and new SQL as parameter.</param>
        public StatementDisplayData(IStatement statement)
        {
            this.CreateCommands();
            this.Statement = statement;
            this.originalSQL = this.Statement.SQL;
        }

        public ICommand RetryCommand { get; private set; }
        public ICommand CopyCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }

        private void CreateCommands()
        {
            this.RetryCommand = new RelayCommand(() =>
            {
                try
                {
                    this.Statement.Execute();
                }
                catch (NpgsqlException)
                {
                    // ignore because result is set in Execute method
                }
            });

            this.CopyCommand = new RelayCommand(this.CopyToClipboard);
        }

        private void CopyToClipboard()
        {
            var statementText = new StringBuilder();
            statementText.AppendLine(this.Statement.SearchPath);
            statementText.AppendLine();
            statementText.AppendLine(this.Statement.SQL);
            Clipboard.SetText(statementText.ToString());
        }
    }
}
