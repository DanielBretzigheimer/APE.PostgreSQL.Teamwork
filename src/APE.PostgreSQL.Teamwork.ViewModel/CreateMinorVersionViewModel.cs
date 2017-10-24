// <copyright file="CreateMinorVersionViewModel.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APE.CodeGeneration.Attributes;
using APE.PostgreSQL.Teamwork.Model;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    [NotifyProperty(typeof(DatabaseVersion), "NewVersion")]
    [NotifyProperty(typeof(bool), "Loading")]
    [CtorParameter(AccessModifier.Public, typeof(Database))]
    [NotifyPropertySupport]
    public partial class CreateMinorVersionViewModel
    {
        private readonly string[] preAlphabet = new[] { "9", "8", "7", "6", "5", "4", "3", "2", "1" };
        private readonly string[] alphabet = new[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

        partial void CreateMinorVersionViewModelCtor()
        {
            this.NewVersion = this.GenerateNewVersion();
        }

        private DatabaseVersion GenerateNewVersion()
        {
            var baseVersion = this.Database.CurrentVersion;
            var nextVersion = this.Database.DiffFiles
                .OrderByDescending(f => f.Version)
                .FirstOrDefault(f => f.Version > this.Database.CurrentVersion)
                .Version;

            var minor = this.alphabet.First();
            


            return new DatabaseVersion(baseVersion.Main, minor);
        }

        private string GetNextMinor(DatabaseVersion version)
        {
            if (version.Minor == string.Empty)
                return this.alphabet.First();

            // remove "." at the start
            var currentMinor = version.Minor.Substring(1);
        }

        private string GetNextLevelMinor(DatabaseVersion version)
        {

        }
    }
}
