// <copyright file="ParserUtils.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Text;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres.Parsers
{
    /// <summary>
    /// Parser utilities.
    /// </summary>
    public class ParserUtils
    {
        /// <summary>
        /// Creates a new instance of ParserUtils.
        /// </summary>
        private ParserUtils()
        {
        }

        /// <summary>
        /// Returns object name from optionally schema qualified name.
        /// </summary>
        /// <param name="name">Optionally schema qualified name.</param>
        /// <returns>Name of the object.</returns>
        public static string GetObjectName(string name)
        {
            var names = SplitNames(name);

            return names[^1];
        }

        /// <summary>
        /// Returns second (from right) object name from optionally schema qualified name.
        /// </summary>
        /// <param name="name">Optionally schema qualified name.</param>
        /// <returns>Name of the object.</returns>
        public static string GetSecondObjectName(string name)
        {
            var names = SplitNames(name);

            return names[^2];
        }

        /// <summary>
        /// Returns third (from right) object name from optionally schema qualified name.
        /// </summary>
        /// <param name="name">Optionally schema qualified name.</param>
        /// <returns>Name of the object or null if there is no third object name.</returns>
        public static string? GetThirdObjectName(string name)
        {
            var names = SplitNames(name);

            return names.Length >= 3 ? names[^3] : null;
        }

        /// <summary>
        /// Returns schema name from optionally schema qualified name.
        /// </summary>
        public static string GetSchemaName(string name, PgDatabase database)
        {
            var names = SplitNames(name);

            if (names.Length < 2)
            {
                return database.DefaultSchema.Name;
            }
            else
            {
                return names[0];
            }
        }

        /// <summary>
        /// Generates unique name from the prefix, list of names, and postfix.
        /// </summary>
        public static string GenerateName(string? prefix, IList<string> names, string? postfix)
        {
            string adjName;

            if (names.Count == 1)
            {
                adjName = names[0];
            }
            else
            {
                var str = new StringBuilder(names.Count * 15);

                foreach (var name in names)
                {
                    if (str.Length > 0)
                    {
                        str.Append(',');
                    }

                    str.Append(name);
                }

                adjName = str.ToString().GetHashCode().ToString("x");
            }

            var result = new StringBuilder(30);
            if (prefix != null && prefix.Length > 0)
            {
                result.Append(prefix);
            }

            result.Append(adjName);

            if (postfix != null && postfix.Length > 0)
            {
                result.Append(postfix);
            }

            return result.ToString();
        }

        /// <summary>
        /// Splits qualified names by dots. If names are quoted then quotes are removed.
        /// </summary>
        private static string[] SplitNames(string namesString)
        {
            if (!namesString.Contains('"'))
            {
                var names = System.Text.RegularExpressions.Regex.Split(namesString, "\\.");
                if (names.Length <= 1)
                    return names;

                for (var i = names.Length; i > 0; i--)
                {
                    if (names[i - 1].Length > 0)
                    {
                        if (i < names.Length)
                            Array.Resize(ref names, i);

                        break;
                    }
                }

                return names;
            }
            else
            {
                var strings = new List<string>(2);
                var startPos = 0;

                while (true)
                {
                    if (namesString[startPos] == '"')
                    {
                        var endPos = namesString.IndexOf('"', startPos + 1);
                        strings.Add(namesString[(startPos + 1)..endPos]);

                        if (endPos + 1 == namesString.Length)
                            break;
                        else if (namesString[endPos + 1] == '.')
                            startPos = endPos + 2;
                        else
                            startPos = endPos + 1;
                    }
                    else
                    {
                        var endPos = namesString.IndexOf('.', startPos);

                        if (endPos == -1)
                        {
                            strings.Add(namesString[startPos..]);
                            break;
                        }
                        else
                        {
                            strings.Add(namesString[startPos..endPos]);
                            startPos = endPos + 1;
                        }
                    }
                }

                return strings.ToArray();
            }
        }
    }
}