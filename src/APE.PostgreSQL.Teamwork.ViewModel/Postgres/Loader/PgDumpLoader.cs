// <copyright file="PgDumpLoader.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema.Enums;
using APE.PostgreSQL.Teamwork.ViewModel.Postgres.Parsers;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres.Loader
{
    /// <summary>
    /// Loads Postgres SQL dump into classes.
    /// </summary>
    public class PgDumpLoader
    {
        private const string DefaultEncoding = "UTF-8";

        /// <summary>
        /// Regex for testing whether it is CREATE SCHEMA statement.
        /// </summary>
        private static readonly Regex PatternCreateSchema = new Regex("^CREATE[\\s]+SCHEMA[\\s]+.*$", RegexOptions.Singleline);

        /// <summary>
        /// Regex for parsing default schema (search_path).
        /// </summary>
        private static readonly Regex PatternDefaultSchema = new Regex("^SET[\\s]+search_path[\\s]*=[\\s]*\"?([^,\\s\"]+)\"?" + "(?:,[\\s]+.*)?;$", RegexOptions.Singleline);

        /// <summary>
        /// Regex for testing whether it is CREATE TABLE statement.
        /// </summary>
        private static readonly Regex PatternCreateTable = new Regex("^CREATE[\\s]+TABLE[\\s]+.*$", RegexOptions.Singleline);

        /// <summary>
        /// Regex for testing whether it is CREATE VIEW statement.
        /// </summary>
        private static readonly Regex PatternCreateView = new Regex("^CREATE[\\s]+(?:OR[\\s]+REPLACE[\\s]+)?VIEW[\\s]+.*$", RegexOptions.Singleline);

        /// <summary>
        /// Regex for testing whether it is ALTER TABLE statement.
        /// </summary>
        private static readonly Regex PatternAlterTable = new Regex("^ALTER[\\s]+TABLE[\\s]+.*$", RegexOptions.Singleline);

        /// <summary>
        /// Regex for testing whether it is CREATE SEQUENCE statement.
        /// </summary>
        private static readonly Regex PatternCreateSequence = new Regex("^CREATE[\\s]+SEQUENCE[\\s]+.*$", RegexOptions.Singleline);

        /// <summary>
        /// Regex for testing whether it is ALTER SEQUENCE statement.
        /// </summary>
        private static readonly Regex PatternAlterSequence = new Regex("^ALTER[\\s]+SEQUENCE[\\s]+.*$", RegexOptions.Singleline);

        /// <summary>
        /// Regex for testing whether it is CREATE INDEX statement.
        /// </summary>
        private static readonly Regex PatternCreateIndex = new Regex("^CREATE[\\s]+(?:UNIQUE[\\s]+)?INDEX[\\s]+.*$", RegexOptions.Singleline);

        /// <summary>
        /// Regex for testing whether it is SELECT statement.
        /// </summary>
        private static readonly Regex PatternSelect = new Regex("^SELECT[\\s]+.*$", RegexOptions.Singleline);

        /// <summary>
        /// Regex for testing whether it is INSERT INTO statement.
        /// </summary>
        private static readonly Regex PatternInsertInto = new Regex("^INSERT[\\s]+INTO[\\s]+.*$", RegexOptions.Singleline);

        /// <summary>
        /// Regex for testing whether it is UPDATE statement.
        /// </summary>
        private static readonly Regex PatternUpdate = new Regex("^UPDATE[\\s].*$", RegexOptions.Singleline);

        /// <summary>
        /// Regex for testing whether it is DELETE FROM statement.
        /// </summary>
        private static readonly Regex PatternDeleteFrom = new Regex("^DELETE[\\s]+FROM[\\s]+.*$", RegexOptions.Singleline);

        /// <summary>
        /// Regex for testing whether it is CREATE TRIGGER statement.
        /// </summary>
        private static readonly Regex PatternCreateTrigger = new Regex("^CREATE[\\s]+TRIGGER[\\s]+.*$", RegexOptions.Singleline);

        /// <summary>
        /// Regex for testing whether it is CREATE FUNCTION or CREATE OR REPLACE
        /// FUNCTION statement.
        /// </summary>
        private static readonly Regex PatternCreateFunction = new Regex("^CREATE[\\s]+(?:OR[\\s]+REPLACE[\\s]+)?FUNCTION[\\s]+.*$", RegexOptions.Singleline);

        /// <summary>
        /// Regex for testing whether it is a GRANT PRIVILEGE statement
        /// </summary>
        private static readonly Regex PatternPrivilegeGrant = new Regex("GRANT.+?TO.+?;", RegexOptions.Singleline);

        /// <summary>
        /// Regex for testing whether it is a REVOKE PRIVILEGE statement
        /// </summary>
        private static readonly Regex PatternPrivilegeRevoke = new Regex("REVOKE.+?FROM.+?;", RegexOptions.Singleline);

        /// <summary>
        /// Regex for testing whether it is CREATE AGGREGATE or CREATE OR REPLACE
        /// AGGREGATE statement.
        /// </summary>
        private static readonly Regex PatternCreateAggregate = new Regex("^CREATE[\\s]+(?:OR[\\s]+REPLACE[\\s]+)?AGGREGATE[\\s]+.*$", RegexOptions.Singleline);

        /// <summary>
        /// Regex for testing whether it is ALTER VIEW statement.
        /// </summary>
        private static readonly Regex PatternAlterView = new Regex("^ALTER[\\s]+VIEW[\\s]+.*$", RegexOptions.Singleline);

        /// <summary>
        /// Regex for testing whether it is COMMENT statement.
        /// </summary>
        private static readonly Regex PatternComment = new Regex("^COMMENT[\\s]+ON[\\s]+.*$", RegexOptions.Singleline);

        /// <summary>
        /// Regex for testing whether it is CREATE TYPE statement.
        /// </summary>
        private static readonly Regex PatternCreateType = new Regex("^CREATE[\\s]+(?:OR[\\s]+REPLACE[\\s]+)?TYPE[\\s]+.*$", RegexOptions.Singleline);

        /// <summary>
        /// Storage of unprocessed line part.
        /// </summary>
        private static string lineBuffer;

        private PgDumpLoader()
        {
        }

        /// <summary>
        /// Loads database schema from dump file.
        /// </summary>
        /// <param name="inputStream">Input stream that should be read.</param>
        /// <param name="outputIgnoredStatements">Whether ignored statements should be included in the output.</param>
        /// <returns>Database schema from dump file.</returns>
        public static PgDatabase LoadDatabaseSchema(string file, string databaseName, bool outputIgnoredStatements, bool ignoreSlonyTriggers)
        {
            return PgDumpLoader.LoadDatabaseSchema(file, DefaultEncoding, databaseName, outputIgnoredStatements, ignoreSlonyTriggers);
        }

        /// <summary>
        /// Loads database schema from dump file.
        /// </summary>
        /// <param name="inputStream">Input stream that should be read.</param>
        /// <param name="encodingName">Charset that should be used to read the file.</param>
        /// <param name="outputIgnoredStatements">Whether ignored statements should be included in the output.</param>
        /// <returns>Database schema from dump file.</returns>
        public static PgDatabase LoadDatabaseSchema(string file, string encodingName, string databaseName, bool outputIgnoredStatements, bool ignoreSlonyTriggers)
        {
            Encoding encoding = Encoding.GetEncoding(encodingName);
            PgDatabase database = new PgDatabase(databaseName);
            StreamReader reader = null;

            using (reader = new StreamReader(file, encoding))
            {
                string statement = GetWholeStatement(reader);

                while (statement != null)
                {
                    if (PatternCreateSchema.Matches(statement).Count != 0)
                        CreateSchemaParser.Parse(database, statement);
                    else if (PatternDefaultSchema.Matches(statement).Count != 0)
                    {
                        PatternDefaultSchema.Matches(statement);
                        database.SetDefaultSchema(PatternDefaultSchema.Matches(statement)[0].Groups[1].ToString());
                    }
                    else if (PatternCreateTable.Matches(statement).Count != 0)
                        CreateTableParser.Parse(database, statement);
                    else if (PatternAlterTable.Matches(statement).Count != 0)
                        AlterTableParser.Parse(database, statement, outputIgnoredStatements);
                    else if (PatternCreateSequence.Matches(statement).Count != 0)
                        CreateSequenceParser.Parse(database, statement);
                    else if (PatternAlterSequence.Matches(statement).Count != 0)
                        AlterSequenceParser.Parse(database, statement);
                    else if (PatternCreateIndex.Matches(statement).Count != 0)
                        CreateIndexParser.Parse(database, statement);
                    else if (PatternCreateView.Matches(statement).Count != 0)
                        CreateViewParser.Parse(database, statement);
                    else if (PatternAlterView.Matches(statement).Count != 0)
                        AlterViewParser.Parse(database, statement, outputIgnoredStatements);
                    else if (PatternCreateTrigger.Matches(statement).Count != 0)
                        CreateTriggerParser.Parse(database, statement, ignoreSlonyTriggers);
                    else if (PatternCreateFunction.Matches(statement).Count != 0)
                        CreateFunctionParser.Parse(database, statement);
                    else if (PatternPrivilegeGrant.Matches(statement).Count != 0)
                        PrivilegeParser.Parse(database, statement, PgPrivilegeCommand.Grant);
                    else if (PatternPrivilegeRevoke.Matches(statement).Count != 0)
                        PrivilegeParser.Parse(database, statement, PgPrivilegeCommand.Revoke);
                    else if (PatternCreateAggregate.Matches(statement).Count != 0)
                        CreateAggregateParser.Parse(database, statement);
                    else if (PatternComment.Matches(statement).Count != 0)
                        CommentParser.Parse(database, statement, outputIgnoredStatements);
                    else if (PatternCreateType.Matches(statement).Count != 0)
                        CreateTypeParser.Parse(database, statement);
                    else if (PatternSelect.Matches(statement).Count != 0 || PatternInsertInto.Matches(statement).Count != 0 || PatternUpdate.Matches(statement).Count != 0 || PatternDeleteFrom.Matches(statement).Count != 0)
                    {
                        // these statements are ignored
                    }
                    else if (outputIgnoredStatements)
                        database.AddIgnoredStatement(statement);
                    else
                    {
                        // these statements are ignored if outputIgnoredStatements is false
                    }

                    statement = GetWholeStatement(reader);
                }
            }

            return database;
        }

        /// <summary>
        /// Reads whole statement from the reader into single-line string.
        /// </summary>
        /// <returns>Whole statement from the reader into single-line string.</returns>
        private static string GetWholeStatement(StreamReader reader)
        {
            StringBuilder statement = new StringBuilder(1024);

            if (lineBuffer != null)
            {
                statement.Append(lineBuffer);
                lineBuffer = null;
                StripComment(statement);
            }

            int pos = statement.ToString().IndexOf(";");

            while (true)
            {
                if (pos == -1)
                {
                    string newLine = reader.ReadLine();

                    if (newLine == null)
                    {
                        if (statement.ToString().Trim().Length == 0)
                            return null;
                        else
                            throw new Exception(string.Format("EndOfStatementNotFound", statement.ToString()));
                    }

                    if (statement.Length > 0)
                        statement.Append('\n');

                    pos = statement.Length;
                    statement.Append(newLine);
                    StripComment(statement);

                    pos = statement.ToString().IndexOf(";", pos);
                }
                else
                {
                    if (!IsQuoted(statement, pos))
                    {
                        if (pos == statement.Length - 1)
                            lineBuffer = null;
                        else
                        {
                            lineBuffer = statement.ToString().Substring(pos + 1);
                            statement.Length = pos + 1;
                        }

                        return statement.ToString().Trim();
                    }

                    pos = statement.ToString().IndexOf(";", pos + 1);
                }
            }
        }

        /// <summary>
        /// Strips comment from statement line.
        /// </summary>
        private static void StripComment(StringBuilder statement)
        {
            int pos = statement.ToString().IndexOf("--");

            while (pos >= 0)
            {
                if (pos == 0)
                {
                    statement.Length = 0;
                    return;
                }
                else
                {
                    if (!IsQuoted(statement, pos))
                    {
                        statement.Length = pos;
                        return;
                    }
                }

                pos = statement.ToString().IndexOf("--", pos + 1);
            }
        }

        /// <summary>
        /// Checks whether specified position in the string builder is quoted. It
        /// might be quoted either by single quote or by dollar sign quoting.
        /// </summary>
        /// <param name="stringBuilder">String builder.</param>
        /// <param name="pos">Position to be checked.</param>
        /// <returns>True if the specified position is quoted, otherwise false.</returns>
        private static bool IsQuoted(StringBuilder stringBuilder, int pos)
        {
            bool isQuoted = false;

            for (int curPos = 0; curPos < pos; curPos++)
            {
                if (stringBuilder[curPos] == '\'')
                {
                    isQuoted = !isQuoted;

                    // if quote was escaped by backslash, it's like double quote
                    if (pos > 0 && stringBuilder[pos - 1] == '\\')
                        isQuoted = !isQuoted;
                }
                else if (stringBuilder[curPos] == '$' && !isQuoted)
                {
                    int endPos = stringBuilder.ToString().IndexOf("$", curPos + 1);

                    if (endPos == -1)
                        return true;

                    string tag = stringBuilder.ToString().Substring(curPos, endPos + 1 - curPos);

                    int endTagPos = stringBuilder.ToString().IndexOf(tag, endPos + 1);

                    // if end tag was not found or it was found after the checked
                    // position, it's quoted
                    if (endTagPos == -1 || endTagPos > pos)
                        return true;

                    curPos = endTagPos + tag.Length - 1;
                }
            }

            return isQuoted;
        }
    }
}