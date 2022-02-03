// <copyright file="DatabaseVersion.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Text.RegularExpressions;
using APE.PostgreSQL.Teamwork.Model.Templates;

namespace APE.PostgreSQL.Teamwork.Model
{
    /// <summary>
    /// Represents one database version which contains of a main and minor version and one combined full version.
    /// </summary>
    public class DatabaseVersion : IComparable<DatabaseVersion>
    {
        /// <summary>
        /// The temporary name of an dump file which is used for undoing a version.
        /// </summary>
        public const string TempDumpName = "TempDump";

        /// <summary>
        /// The temporary name of a diff file.
        /// </summary>
        public const string TempUndoDiffName = "TempUndoDiff";

        private static readonly EqualsAndHashCode<DatabaseVersion> EqualsAndHashCode = new(
            v => v.Main,
            v => v.Minor);

        private static readonly Regex RegexDiffVersion = new(@"\\(?<Version>[0-9][0-9][0-9][0-9])(?<SubVersion>\..*)?\" + SQLTemplates.DiffFile);
        private static readonly Regex RegexDumpVersion = new(@"\\(?<Version>[0-9][0-9][0-9][0-9])(?<SubVersion>\..*)?\" + SQLTemplates.DumpFile);
        private static readonly Regex RegexUndoDiffFile = new(@"\\(?<Version>[0-9][0-9][0-9][0-9])(?<SubVersion>\..*)?\" + SQLTemplates.UndoDiffFile);

        /// <summary>
        /// Parses the given path to a <see cref="DatabaseVersion"/>.
        /// </summary>
        /// <exception cref="ArgumentException">Is thrown when no version could be parsed.</exception>
        /// <param name="path">File path with the version.</param>
        public DatabaseVersion(string path)
        {
            if (path.Contains(TempUndoDiffName) || path.Contains(TempDumpName))
            {
                this.Main = int.MaxValue; // check for test version
            }
            else
            {
                MatchCollection? collection = null;
                if (path.EndsWith(SQLTemplates.DiffFile))
                    collection = RegexDiffVersion.Matches(path);
                else if (path.EndsWith(SQLTemplates.DumpFile))
                    collection = RegexDumpVersion.Matches(path);
                else if (path.EndsWith(SQLTemplates.UndoDiffFile))
                    collection = RegexUndoDiffFile.Matches(path);

                if (collection != null && collection.Count != 0)
                {
                    this.Main = int.Parse(collection[0].Groups["Version"].Value);
                    this.Minor = collection[0].Groups["SubVersion"].Value;
                }
                else
                {
                    throw new ArgumentException("Version could not be parsed from path " + path);
                }
            }
        }

        /// <summary>
        /// Parses the given <see cref="ExecutedFile"/> to a <see cref="DatabaseVersion"/>.
        /// </summary>
        public DatabaseVersion(ExecutedFile file)
        {
            this.Main = int.Parse(file.Version[..4]);
            this.Minor = file.Version[4..];
        }

        /// <summary>
        /// Creates a new <see cref="DatabaseVersion"/> with the given <see cref="Main"/> version and no <see cref="Minor"/> version.
        /// </summary>
        public DatabaseVersion(int mainVersion) => this.Main = mainVersion;

        public DatabaseVersion(int main, string minor)
            : this(main) => this.Minor = minor.StartsWith(".") ? minor : $".{minor}";

        private DatabaseVersion()
        {
        }

        /// <summary>
        /// Returns the lowest <see cref="DatabaseVersion"/>.
        /// </summary>
        public static DatabaseVersion StartVersion => new(0);

        /// <summary>
        /// Gets the main part of the version.
        /// </summary>
        public int Main { get; private set; }

        /// <summary>
        /// Gets the minor part of the version. (e.g. ".a").
        /// </summary>
        public string Minor { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the full version which contains out of the <see cref="Main"/> and <see cref="Minor"/> version.
        /// </summary>
        public string Full =>
                // should look like 0187.a
                this.Main.ToString().PadLeft(4, '0') + this.Minor;

        /// <summary>
        /// Increases the <see cref="Main"/> version by 1.
        /// </summary>
        public static DatabaseVersion operator ++(DatabaseVersion v)
        {
            v.Main++;
            return v;
        }

        /// <summary>
        /// Checks if the version a is smaller than the version b and returns a boolean with the result.
        /// </summary>
        public static bool operator <(DatabaseVersion a, DatabaseVersion b)
        {
            if (a == b)
                return false;

            var versions = new List<DatabaseVersion> { a, b }
            .OrderBy(v => v.Full);

            return versions.First() == a;
        }

        /// <summary>
        /// Checks if the version a is bigger than the version b and returns a boolean with the result.
        /// </summary>
        public static bool operator >(DatabaseVersion a, DatabaseVersion b)
        {
            if (a == b)
                return false;

            var versions = new List<DatabaseVersion> { a, b }
            .OrderBy(v => v.Full);

            return versions.Last() == a;
        }

        /// <summary>
        /// Checks if the version a is smaller or equal than the version b and returns a boolean with the result.
        /// </summary>
        public static bool operator <=(DatabaseVersion a, DatabaseVersion b) => a == b || a < b;

        /// <summary>
        /// Checks if the version a is bigger or equal than the version b and returns a boolean with the result.
        /// </summary>
        public static bool operator >=(DatabaseVersion a, DatabaseVersion b) => a == b || a > b;

        /// <summary>
        /// Checks if the version a is equal to the version b and returns a boolean with the result.
        /// </summary>
        public static bool operator ==(DatabaseVersion? a, DatabaseVersion? b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
                return true;

            // If one is null, but not both, return false.
            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        /// <summary>
        /// Checks if the version a is not equal to the version b and returns a boolean with the result.
        /// </summary>
        public static bool operator !=(DatabaseVersion? a, DatabaseVersion? b) => !(a == b);

        public DatabaseVersion Next() => new(this.Main + 1);

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        public override bool Equals(object? obj) => EqualsAndHashCode.AreEqual(this, obj);

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => EqualsAndHashCode.GetHashCode(this);

        /// <summary>
        ///  Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
            => this.Full;

        public int CompareTo(DatabaseVersion? other)
        {
            if (other == null)
                return 1;

            return this.Full.CompareTo(other.Full);
        }

        public static DatabaseVersion CommandLineVersion(string version)
        {
            if (version.Length < 4)
                throw new ArgumentException($"Version could not be parsed from {version}");

            var retVal = new DatabaseVersion()
            {
                Main = int.Parse(version[..4]),
                Minor = version[4..],
            };
            return retVal;
        }
    }
}
