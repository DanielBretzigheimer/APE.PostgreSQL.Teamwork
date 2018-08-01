// <copyright file="ParserUtils.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Collections.Generic;
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

            return names[names.Length - 1];
        }

        /// <summary>
        /// Returns second (from right) object name from optionally schema qualified name.
        /// </summary>
        /// <param name="name">Optionally schema qualified name.</param>
        /// <returns>Name of the object.</returns>
        public static string GetSecondObjectName(string name)
        {
            var names = SplitNames(name);

            return names[names.Length - 2];
        }

        /// <summary>
        /// Returns third (from right) object name from optionally schema qualified name.
        /// </summary>
        /// <param name="name">Optionally schema qualified name.</param>
        /// <returns>Name of the object or null if there is no third object name.</returns>
        [return: NullGuard.AllowNull]
        public static string GetThirdObjectName(string name)
        {
            var names = SplitNames(name);

            return names.Length >= 3 ? names[names.Length - 3] : null;
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
        public static string GenerateName([NullGuard.AllowNull] string prefix, IList<string> names, [NullGuard.AllowNull] string postfix)
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

        private static string[] Split(string self, string regexDelimiter, bool trimTrailingEmptyStrings)
        {
            var splitArray = System.Text.RegularExpressions.Regex.Split(self, regexDelimiter);

            if (trimTrailingEmptyStrings)
            {
                if (splitArray.Length > 1)
                {
                    for (var i = splitArray.Length; i > 0; i--)
                    {
                        if (splitArray[i - 1].Length > 0)
                        {
                            if (i < splitArray.Length)
                            {
                                System.Array.Resize(ref splitArray, i);
                            }

                            break;
                        }
                    }
                }
            }

            return splitArray;
        }

        /// <summary>
        /// Splits qualified names by dots. If names are quoted then quotes are removed.
        /// </summary>
        private static string[] SplitNames(string @string)
        {
            if (@string.IndexOf('"') == -1)
            {
                var names = System.Text.RegularExpressions.Regex.Split(@string, "\\.");
                if (names.Length <= 1)
                {
                    return names;
                }

                for (var i = names.Length; i > 0; i--)
                {
                    if (names[i - 1].Length > 0)
                    {
                        if (i < names.Length)
                        {
                            System.Array.Resize(ref names, i);
                        }

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
                    if (@string[startPos] == '"')
                    {
                        var endPos = @string.IndexOf('"', startPos + 1);
                        strings.Add(@string.Substring(startPos + 1, endPos - (startPos + 1)));

                        if (endPos + 1 == @string.Length)
                        {
                            break;
                        }
                        else if (@string[endPos + 1] == '.')
                        {
                            startPos = endPos + 2;
                        }
                        else
                        {
                            startPos = endPos + 1;
                        }
                    }
                    else
                    {
                        var endPos = @string.IndexOf('.', startPos);

                        if (endPos == -1)
                        {
                            strings.Add(@string.Substring(startPos));
                            break;
                        }
                        else
                        {
                            strings.Add(@string.Substring(startPos, endPos - startPos));
                            startPos = endPos + 1;
                        }
                    }
                }

                return strings.ToArray();
            }
        }
    }
}