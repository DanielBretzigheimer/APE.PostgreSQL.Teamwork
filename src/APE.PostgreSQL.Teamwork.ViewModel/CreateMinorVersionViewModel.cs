// <copyright file="CreateMinorVersionViewModel.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>

using System.IO;
using System.Windows.Input;
using APE.PostgreSQL.Teamwork.Model;
using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;
using Serilog;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    public partial class CreateMinorVersionViewModel
    {
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
                    Log.Warning($"Error while exporting database {this.DatabaseDisplayData.Name} for minor version ", ex);
                    this.ShowErrorMessage = true;

                    var message = ex.Message;
                    if (ex is FileNotFoundException fileNotFoundException)
                    {
                        message = $"{message}: {fileNotFoundException.FileName}";
                    }
                    else if (ex is TeamworkException teamworkException && !teamworkException.ShowAsError)
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
            var baseVersion = this.DatabaseDisplayData.Database?.CurrentVersion
                ?? throw new InvalidOperationException($"The Database {this.DatabaseDisplayData.Name} is not connected.");

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

        private string? GetNextMinor(DatabaseVersion version)
        {
            if (version.Minor == string.Empty)
                return this.alphabet.First();

            // remove "." at the start
            var currentMinor = version.Minor[1..];
            for (var i = 0; i < this.alphabet.Length - 1; i++)
            {
                if (this.alphabet[i] == currentMinor)
                    return this.alphabet[i + 1];
            }

            return null;
        }
    }
}
