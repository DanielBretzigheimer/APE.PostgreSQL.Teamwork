// <copyright file="PgDiffStringExtension.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
namespace APE.PostgreSQL.Teamwork.Model.Utils
{
    /// <summary>
    /// Utilities for creation of diffs.
    /// </summary>
    public static class PgDiffStringExtension
    {
        /// <summary>
        /// Array of reserved keywords. Non-reserved keywords are excluded.
        /// Source: <c>http://www.postgresql.org/docs/9.0/static/sql-keywords-appendix.html</c>.
        /// </summary>
        private static readonly string[] Keywords = new string[] { "ABS", "ABSOLUTE", "ACTION", "ADD", "ADMIN", "AFTER", "AGGREGATE", "ALIAS", "ALL", "ALLOCATE", "ALTER", "ANALYSE", "ANALYZE", "AND", "ANY", "ARE", "ARRAY", "ARRAY_AGG", "AS", "ASC", "ASENSITIVE", "ASSERTION", "ASYMMETRIC", "AT", "ATOMIC", "AUTHORIZATION", "AVG", "BEFORE", "BEGIN", "BETWEEN", "BIGINT", "BINARY", "BIT", "BIT_LENGTH", "BLOB", "BOOLEAN", "BOTH", "BREADTH", "BY", "CALL", "CALLED", "CARDINALITY", "CASCADE", "CASCADED", "CASE", "CAST", "CATALOG", "CEIL", "CEILING", "CHAR", "CHARACTER", "CHARACTER_LENGTH", "CHAR_LENGTH", "CHECK", "CLASS", "CLOB", "CLOSE", "COALESCE", "COLLATE", "COLLATION", "COLLECT", "COLUMN", "COMMIT", "COMPLETION", "CONCURRENTLY", "CONDITION", "CONNECT", "CONNECTION", "CONSTRAINT", "CONSTRAINTS", "CONSTRUCTOR", "CONTINUE", "CONVERT", "CORR", "CORRESPONDING", "COUNT", "COVAR_POP", "COVAR_SAMP", "CREATE", "CROSS", "CUBE", "CUME_DIST", "CURRENT", "CURRENT_CATALOG", "CURRENT_DATE", "CURRENT_DEFAULT_TRANSFORM_GROUP", "CURRENT_PATH", "CURRENT_ROLE", "CURRENT_SCHEMA", "CURRENT_TIME", "CURRENT_TIMESTAMP", "CURRENT_TRANSFORM_GROUP_FOR_TYPE", "CURRENT_USER", "CURSOR", "CYCLE", "DATA", "DATALINK", "DATE", "DAY", "DEALLOCATE", "DEC", "DECIMAL", "DECLARE", "DEFAULT", "DEFERRABLE", "DEFERRED", "DELETE", "DENSE_RANK", "DEPTH", "DEREF", "DESC", "DESCRIBE", "DESCRIPTOR", "DESTROY", "DESTRUCTOR", "DETERMINISTIC", "DIAGNOSTICS", "DICTIONARY", "DISCONNECT", "DISTINCT", "DLNEWCOPY", "DLPREVIOUSCOPY", "DLURLCOMPLETE", "DLURLCOMPLETEONLY", "DLURLCOMPLETEWRITE", "DLURLPATH", "DLURLPATHONLY", "DLURLPATHWRITE", "DLURLSCHEME", "DLURLSERVER", "DLVALUE", "DO", "DOMAIN", "DOUBLE", "DROP", "DYNAMIC", "EACH", "ELEMENT", "ELSE", "END", "END-EXEC", "EQUALS", "ESCAPE", "EVERY", "EXCEPT", "EXCEPTION", "EXEC", "EXECUTE", "EXISTS", "EXP", "EXTERNAL", "EXTRACT", "FALSE", "FETCH", "FILTER", "FIRST", "FIRST_VALUE", "FLOAT", "FLOOR", "FOR", "FOREIGN", "FOUND", "FREE", "FREEZE", "FROM", "FULL", "FUNCTION", "FUSION", "GENERAL", "GET", "GLOBAL", "GO", "GOTO", "GRANT", "GROUP", "GROUPING", "HAVING", "HOLD", "HOST", "HOUR", "IDENTITY", "IGNORE", "ILIKE", "IMMEDIATE", "IMPORT", "IN", "INDICATOR", "INITIALIZE", "INITIALLY", "INNER", "INOUT", "INPUT", "INSENSITIVE", "INSERT", "INT", "INTEGER", "INTERSECT", "INTERSECTION", "INTERVAL", "INTO", "IS", "ISNULL", "ISOLATION", "ITERATE", "JOIN", "KEY", "LAG", "LANGUAGE", "LARGE", "LAST", "LAST_VALUE", "LATERAL", "LEAD", "LEADING", "LEFT", "LESS", "LEVEL", "LIKE", "LIKE_REGEX", "LIMIT", "LN", "LOCAL", "LOCALTIME", "LOCALTIMESTAMP", "LOCATOR", "LOWER", "MAP", "MATCH", "MAX", "MAX_CARDINALITY", "MEMBER", "MERGE", "METHOD", "MIN", "MINUTE", "MOD", "MODIFIES", "MODIFY", "MODULE", "MONTH", "MULTISET", "NAMES", "NATIONAL", "NATURAL", "NCHAR", "NCLOB", "NEW", "NEXT", "NO", "NONE", "NORMALIZE", "NOT", "NOTNULL", "NTH_VALUE", "NTILE", "NULL", "NULLIF", "NUMERIC", "OBJECT", "OCCURRENCES_REGEX", "OCTET_LENGTH", "OF", "OFF", "OFFSET", "OLD", "ON", "ONLY", "OPEN", "OPERATION", "OPTION", "OR", "ORDER", "ORDINALITY", "OUT", "OUTER", "OUTPUT", "OVER", "OVERLAPS", "OVERLAY", "PAD", "PARAMETER", "PARAMETERS", "PARTIAL", "PARTITION", "PATH", "PERCENTILE_CONT", "PERCENTILE_DISC", "PERCENT_RANK", "PLACING", "POSITION", "POSITION_REGEX", "POSTFIX", "POWER", "PRECISION", "PREFIX", "PREORDER", "PREPARE", "PRESERVE", "PRIMARY", "PRIOR", "PRIVILEGES", "PROCEDURE", "PUBLIC", "RANGE", "RANK", "READ", "READS", "REAL", "RECURSIVE", "REF", "REFERENCES", "REFERENCING", "REGR_AVGX", "REGR_AVGY", "REGR_COUNT", "REGR_INTERCEPT", "REGR_R2", "REGR_SLOPE", "REGR_SXX", "REGR_SXY", "REGR_SYY", "RELATIVE", "RELEASE", "RESTRICT", "RESULT", "RETURN", "RETURNING", "RETURNS", "REVOKE", "RIGHT", "ROLE", "ROLLBACK", "ROLLUP", "ROUTINE", "ROW", "ROWS", "ROW_NUMBER", "SAVEPOINT", "SCHEMA", "SCOPE", "SCROLL", "SEARCH", "SECOND", "SECTION", "SELECT", "SENSITIVE", "SEQUENCE", "SESSION", "SESSION_USER", "SET", "SETS", "SIMILAR", "SIZE", "SMALLINT", "SOME", "SPACE", "SPECIFIC", "SPECIFICTYPE", "SQL", "SQLCODE", "SQLERROR", "SQLEXCEPTION", "SQLSTATE", "SQLWARNING", "SQRT", "START", "STATE", "STATEMENT", "STATIC", "STDDEV_POP", "STDDEV_SAMP", "STRUCTURE", "SUBMULTISET", "SUBSTRING", "SUBSTRING_REGEX", "SUM", "SYMMETRIC", "SYSTEM", "SYSTEM_USER", "TABLE", "TABLESAMPLE", "TEMPORARY", "TERMINATE", "THAN", "THEN", "TIME", "TIMESTAMP", "TIMEZONE_HOUR", "TIMEZONE_MINUTE", "TO", "TRAILING", "TRANSACTION", "TRANSLATE", "TRANSLATE_REGEX", "TRANSLATION", "TREAT", "TRIGGER", "TRIM", "TRIM_ARRAY", "TRUE", "TRUNCATE", "UESCAPE", "UNDER", "UNION", "UNIQUE", "UNKNOWN", "UNNEST", "UPDATE", "UPPER", "USAGE", "USER", "USING", "VALUE", "VALUES", "VARBINARY", "VARCHAR", "VARIABLE", "VARIADIC", "VARYING", "VAR_POP", "VAR_SAMP", "VERBOSE", "VIEW", "WHEN", "WHENEVER", "WHERE", "WIDTH_BUCKET", "WINDOW", "WITH", "WITHIN", "WITHOUT", "WORK", "WRITE", "XML", "XMLAGG", "XMLATTRIBUTES", "XMLBINARY", "XMLCAST", "XMLCOMMENT", "XMLCONCAT", "XMLDOCUMENT", "XMLELEMENT", "XMLEXISTS", "XMLFOREST", "XMLITERATE", "XMLNAMESPACES", "XMLPARSE", "XMLPI", "XMLQUERY", "XMLROOT", "XMLSERIALIZE", "XMLTABLE", "XMLTEXT", "XMLVALIDATE", "YEAR", "ZONE" };

        /// <summary>
        /// If name contains only lower case characters and digits and is not keyword, it is returned not quoted,
        /// otherwise the string is returned quoted.
        /// </summary>
        /// <param name="name">The name which gets quoted.</param>
        /// <param name="excludeKeywords">Whether check against keywords should be skipped.</param>
        /// <returns>Quoted string if needed, otherwise not quoted string.</returns>
        public static string GetQuotedName(this string name, bool excludeKeywords)
        {
            if (name.IndexOf('-') != -1 || name.IndexOf('.') != -1)
            {
                return '"' + name + '"';
            }

            for (var i = 0; i < name.Length; i++)
            {
                var chr = name[i];
                if (char.IsUpper(chr))
                {
                    return '"' + name + '"';
                }
            }

            if (excludeKeywords)
            {
                return name;
            }

            var upperName = name.ToUpper(new System.Globalization.CultureInfo("en"));

            foreach (var keyword in Keywords)
            {
                if (keyword.Equals(upperName))
                {
                    return '"' + name + '"';
                }
            }

            return name;
        }

        /// <summary>
        /// If name contains only lower case characters and digits and is not
        /// keyword, it is returned not quoted, otherwise the string is returned
        /// quoted.
        /// </summary>
        /// <returns>Quoted string if needed, otherwise not quoted string.</returns>
        public static string QuoteName(this string name)
        {
            return name.GetQuotedName(false);
        }
    }
}