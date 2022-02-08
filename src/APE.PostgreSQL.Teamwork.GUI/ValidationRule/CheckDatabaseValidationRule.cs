// <copyright file="CheckDatabaseValidationRule.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Windows.Controls;
using APE.PostgreSQL.Teamwork.Model.Setting;
using APE.PostgreSQL.Teamwork.ViewModel;

namespace APE.PostgreSQL.Teamwork.View
{
    public partial class CheckDatabaseValidationRule : ValidationRule
    {
        private readonly IConnectionManager connectionManager = new ConnectionManager();

        public CheckDatabaseValidationRule()
        {
            var setting = SettingsManager.Get().Setting;
            this.connectionManager.Initialize(
                setting.Id,
                setting.Host,
                setting.Password,
                setting.Port);
        }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value is string databaseName)
            {
                // check connection
                if (this.connectionManager.CheckConnection(databaseName))
                    return ValidationResult.ValidResult;
                else
                    return new ValidationResult(false, "Database was not found.");
            }
            else
            {
                return new ValidationResult(false, "Input has to be a text.");
            }
        }

        partial void Dispose(bool disposing)
        {
            if (disposing)
                this.connectionManager.Dispose();
        }
    }
}
