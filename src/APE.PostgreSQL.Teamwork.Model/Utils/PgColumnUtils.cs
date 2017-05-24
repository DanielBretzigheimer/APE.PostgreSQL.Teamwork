// <copyright file="PgColumnUtils.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Globalization;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;

namespace APE.PostgreSQL.Teamwork.Model.Utils
{
    /// <summary>
    /// Utilities for <seealso cref="PgColumn"/>.
    /// </summary>
    public class PgColumnUtils
    {
        /// <summary>
        /// Creates a new <see cref="PgColumnUtils"/> object.
        /// </summary>
        private PgColumnUtils()
        {
        }

        /// <summary>
        /// Returns default value for given column type. If no default value is specified then null is returned.
        /// </summary>
        /// <param name="type">Column type.</param>
        /// <returns>Found default value or null.</returns>
        [return: NullGuard.AllowNull]
        public static string GetDefaultValue(string type)
        {
            string defaultValue;

            var adjType = type.ToLower(new CultureInfo("en"));

            if ("smallint".Equals(adjType)
                || "integer".Equals(adjType)
                || "bigint".Equals(adjType)
                || adjType.StartsWith("decimal")
                || adjType.StartsWith("numeric")
                || "real".Equals(adjType)
                || "double precision".Equals(adjType)
                || "int2".Equals(adjType)
                || "int4".Equals(adjType)
                || "int8".Equals(adjType)
                || adjType.StartsWith("float")
                || "double".Equals(adjType)
                || "money".Equals(adjType))
            {
                defaultValue = "0";
            }
            else if (adjType.StartsWith("character varying")
                || adjType.StartsWith("varchar")
                || adjType.StartsWith("character")
                || adjType.StartsWith("char")
                || "text".Equals(adjType))
            {
                defaultValue = "''";
            }
            else if ("boolean".Equals(adjType))
            {
                defaultValue = "false";
            }
            else
            {
                defaultValue = null;
            }

            return defaultValue;
        }
    }
}