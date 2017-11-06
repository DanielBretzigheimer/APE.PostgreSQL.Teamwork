// <copyright file="CreateMinorVersionViewModel.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using APE.CodeGeneration.Attributes;
using APE.PostgreSQL.Teamwork.Model;
using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;
using log4net;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    [NotifyProperty(typeof(DatabaseVersion), "NewVersion")]
    [NotifyProperty(typeof(bool), "Loading")]
    [NotifyProperty(typeof(bool), "ShowErrorMessage")]
    [NotifyProperty(typeof(bool), "ShowSuccessMessage")]
    [NotifyProperty(AccessModifier.PublicGetPrivateSet, typeof(string), "Message", "")]
    [CtorParameter(AccessModifier.Public, typeof(DatabaseDisplayData))]
    [NotifyPropertySupport]
    public partial class CreateMinorVersionViewModel
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string[] alphabet = new[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

        public ICommand CreateCommand { get; private set; }

        partial void CreateMinorVersionViewModelCtor()
        {
            this.NewVersion = this.GenerateNewVersion();
            this.CreateCommand = new RelayCommand(this.CreateMinorVersion);
        }

        private void CreateMinorVersion()
        {
            this.Loading = true;
            Task.Run(() =>
            {
                try
                {
                    this.DatabaseDisplayData.ExportWithoutErrorHandling(this.NewVersion);
                    this.ShowSuccessMessage = true;
                    this.Message = $"Export succesfully finished.";
                }
                catch (Exception ex)
                {
                    Log.Warn($"Error while exporting database {this.DatabaseDisplayData.Database.Name} for minor version ", ex);
                    this.ShowErrorMessage = true;

                    var message = ex.Message;
                    if (ex is FileNotFoundException)
                    {
                        message = $"{message}: {((FileNotFoundException)ex).FileName}";
                    }
                    else if (ex is TeamworkException && !((TeamworkException)ex).ShowAsError)
                    {
                        this.ShowSuccessMessage = true;
                        this.ShowErrorMessage = false;
                    }

                    this.Message = $"Error while exporting database: {message}";
                }
                finally
                {
                    this.Loading = false;
                }
            });
        }

        private DatabaseVersion GenerateNewVersion()
        {
            var baseVersion = this.DatabaseDisplayData.Database.CurrentVersion;

            var nextFile = this.DatabaseDisplayData.Database.DiffFiles
                .OrderBy(f => f.Version)
                .FirstOrDefault(f => f.Version > baseVersion);

            if (nextFile == null)
                throw new InvalidOperationException("No need for a minor version because no following major version does exist.");
            if (nextFile.Version.Minor != string.Empty)
                throw new InvalidOperationException($"Can't create minor version after {baseVersion.Full} and before a already existing minor version ({nextFile.Version.Full})");

            var nextMinorVersion = this.GetNextMinor(baseVersion);
            if (nextMinorVersion == null)
                throw new InvalidOperationException($"Can't create new minor version after {baseVersion.Full}.");

            return new DatabaseVersion(baseVersion.Main, nextMinorVersion);
        }

        [return: NullGuard.AllowNull]
        private string GetNextMinor(DatabaseVersion version)
        {
            if (version.Minor == string.Empty)
                return this.alphabet.First();

            // remove "." at the start
            var currentMinor = version.Minor.Substring(1);
            for (var i = 0; i < this.alphabet.Length - 1; i++)
            {
                if (this.alphabet[i] == currentMinor)
                    return this.alphabet[i + 1];
            }

            return null;
        }
    }
}
