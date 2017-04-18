// <copyright file="CheckDatabaseValidationRule.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using APE.PostgreSQL.Teamwork.Model.Setting;
using APE.PostgreSQL.Teamwork.ViewModel;

namespace APE.PostgreSQL.Teamwork.View
{
    public class CheckDatabaseValidationRule : ValidationRule
    {
        private IConnectionManager connectionManager = new ConnectionManager();

        public CheckDatabaseValidationRule()
        {
            this.connectionManager.Initialize(
                SettingsManager.Get().Setting.Id,
                SettingsManager.Get().Setting.Host,
                SettingsManager.Get().Setting.Password,
                SettingsManager.Get().Setting.Port);
        }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value is string)
            {
                var databaseName = value as string;

                // check connection
                if (this.connectionManager.CheckConnection(databaseName))
                {
                    return ValidationResult.ValidResult;
                }
                else
                {
                    return new ValidationResult(false, "Database was not found.");
                }
            }
            else
            {
                return new ValidationResult(false, "Input has to be a text.");
            }
        }
    }
}
