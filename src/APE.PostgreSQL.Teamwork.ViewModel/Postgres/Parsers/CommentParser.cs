// <copyright file="CommentParser.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres.Parsers
{
    /// <summary>
    /// COMMENT parser.
    /// </summary>
    public class CommentParser
    {
        /// <summary>
        /// Creates new instance of CommentParser.
        /// </summary>
        private CommentParser()
        {
        }

        /// <summary>
        /// Parses COMMENT statements.
        /// </summary>
        public static void Parse(PgDatabase database, string statement, bool outputIgnoredStatements)
        {
            var parser = new Parser(statement);
            parser.Expect("COMMENT", "ON");

            if (parser.ExpectOptional("TABLE"))
            {
                ParseTable(parser, database);
            }
            else if (parser.ExpectOptional("COLUMN"))
            {
                ParseColumn(parser, database);
            }
            else if (parser.ExpectOptional("CONSTRAINT"))
            {
                ParseConstraint(parser, database);
            }
            else if (parser.ExpectOptional("DATABASE"))
            {
                ParseDatabase(parser, database);
            }
            else if (parser.ExpectOptional("FUNCTION"))
            {
                ParseFunction(parser, database);
            }
            else if (parser.ExpectOptional("INDEX"))
            {
                ParseIndex(parser, database);
            }
            else if (parser.ExpectOptional("SCHEMA"))
            {
                ParseSchema(parser, database);
            }
            else if (parser.ExpectOptional("SEQUENCE"))
            {
                ParseSequence(parser, database);
            }
            else if (parser.ExpectOptional("TRIGGER"))
            {
                ParseTrigger(parser, database);
            }
            else if (parser.ExpectOptional("VIEW"))
            {
                ParseView(parser, database);
            }
            else if (outputIgnoredStatements)
            {
                database.AddIgnoredStatement(statement);
            }
        }

        /// <summary>
        /// Parses COMMENT ON TABLE.
        /// </summary>
        private static void ParseTable(Parser parser, PgDatabase database)
        {
            var tableName = parser.ParseIdentifier();

            var objectName = ParserUtils.GetObjectName(tableName);

            var schemaName = ParserUtils.GetSchemaName(tableName, database);

            PgTable table = database.GetSchema(schemaName).GetTable(objectName);

            parser.Expect("IS");
            table.Comment = GetComment(parser);
            parser.Expect(";");
        }

        /// <summary>
        /// Parses COMMENT ON CONSTRAINT.
        /// </summary>
        private static void ParseConstraint(Parser parser, PgDatabase database)
        {
            var constraintName = ParserUtils.GetObjectName(parser.ParseIdentifier());

            parser.Expect("ON");

            var tableName = parser.ParseIdentifier();
            var objectName = ParserUtils.GetObjectName(tableName);
            var schemaName = ParserUtils.GetSchemaName(constraintName, database);
            PgConstraint constraint = database.GetSchema(schemaName).GetTable(objectName).GetConstraint(constraintName);

            parser.Expect("IS");
            constraint.Comment = GetComment(parser);
            parser.Expect(";");
        }

        /// <summary>
        /// Parses COMMENT ON DATABASE.
        /// </summary>
        private static void ParseDatabase(Parser parser, PgDatabase database)
        {
            parser.ParseIdentifier();
            parser.Expect("IS");
            database.Comment = GetComment(parser);
            parser.Expect(";");
        }

        /// <summary>
        /// Parses COMMENT ON INDEX.
        /// </summary>
        private static void ParseIndex(Parser parser, PgDatabase database)
        {
            var indexName = parser.ParseIdentifier();

            var objectName = ParserUtils.GetObjectName(indexName);

            var schemaName = ParserUtils.GetSchemaName(indexName, database);

            PgSchema schema = database.GetSchema(schemaName);

            PgIndex index = schema.GetIndex(objectName);

            if (index == null)
            {
                PgConstraint primaryKey = schema.GetPrimaryKey(objectName);
                parser.Expect("IS");
                primaryKey.Comment = GetComment(parser);
                parser.Expect(";");
            }
            else
            {
                parser.Expect("IS");
                index.Comment = GetComment(parser);
                parser.Expect(";");
            }
        }

        /// <summary>
        /// Parses COMMENT ON SCHEMA.
        /// </summary>
        private static void ParseSchema(Parser parser, PgDatabase database)
        {
            var schemaName = ParserUtils.GetObjectName(parser.ParseIdentifier());

            PgSchema schema = database.GetSchema(schemaName);

            parser.Expect("IS");
            schema.Comment = GetComment(parser);
            parser.Expect(";");
        }

        /// <summary>
        /// Parses COMMENT ON SEQUENCE.
        /// </summary>
        private static void ParseSequence(Parser parser, PgDatabase database)
        {
            var sequenceName = parser.ParseIdentifier();

            var objectName = ParserUtils.GetObjectName(sequenceName);

            var schemaName = ParserUtils.GetSchemaName(sequenceName, database);

            PgSequence sequence = database.GetSchema(schemaName).GetSequence(objectName);

            parser.Expect("IS");
            sequence.Comment = GetComment(parser);
            parser.Expect(";");
        }

        /// <summary>
        /// Parses COMMENT ON TRIGGER.
        /// </summary>
        private static void ParseTrigger(Parser parser, PgDatabase database)
        {
            var triggerName = ParserUtils.GetObjectName(parser.ParseIdentifier());

            parser.Expect("ON");

            var tableName = parser.ParseIdentifier();

            var objectName = ParserUtils.GetObjectName(tableName);

            var schemaName = ParserUtils.GetSchemaName(triggerName, database);

            PgTrigger trigger = database.GetSchema(schemaName).GetTable(objectName).GetTrigger(triggerName);

            parser.Expect("IS");
            trigger.Comment = GetComment(parser);
            parser.Expect(";");
        }

        /// <summary>
        /// Parses COMMENT ON VIEW.
        /// </summary>
        private static void ParseView(Parser parser, PgDatabase database)
        {
            var viewName = parser.ParseIdentifier();

            var objectName = ParserUtils.GetObjectName(viewName);

            var schemaName = ParserUtils.GetSchemaName(viewName, database);

            PgView view = database.GetSchema(schemaName).GetView(objectName);

            parser.Expect("IS");
            view.Comment = GetComment(parser);
            parser.Expect(";");
        }

        /// <summary>
        /// Parses COMMENT ON COLUMN.
        /// </summary>
        private static void ParseColumn(Parser parser, PgDatabase database)
        {
            var columnName = parser.ParseIdentifier();

            var objectName = ParserUtils.GetObjectName(columnName);

            var tableName = ParserUtils.GetSecondObjectName(columnName);

            var schemaName = ParserUtils.GetThirdObjectName(columnName);

            PgSchema schema = database.GetSchema(schemaName);

            PgTable table = schema.GetTable(tableName);

            if (table == null)
            {
                PgView view = schema.GetView(tableName);
                parser.Expect("IS");

                var comment = GetComment(parser);

                if (comment == null)
                {
                    view.RemoveColumnComment(objectName);
                }
                else
                {
                    view.AddColumnComment(objectName, comment);
                }

                parser.Expect(";");
            }
            else
            {
                PgColumn column = table.GetColumn(objectName);

                if (column == null)
                {
                    throw new TeamworkParserException($"CannotFindColumnInTable {columnName} from table {table.Name}");
                }

                parser.Expect("IS");
                column.Comment = GetComment(parser);
                parser.Expect(";");
            }
        }

        /// <summary>
        /// Parses COMMENT ON FUNCTION.
        /// </summary>
        private static void ParseFunction(Parser parser, PgDatabase database)
        {
            var functionName = parser.ParseIdentifier();

            var objectName = ParserUtils.GetObjectName(functionName);

            var schemaName = ParserUtils.GetSchemaName(functionName, database);

            PgSchema schema = database.GetSchema(schemaName);

            parser.Expect("(");

            var tmpFunction = new PgFunction()
            {
                Name = objectName,
            };
            while (!parser.ExpectOptional(")"))
            {
                string mode;

                if (parser.ExpectOptional("IN"))
                {
                    mode = "IN";
                }
                else if (parser.ExpectOptional("OUT"))
                {
                    mode = "OUT";
                }
                else if (parser.ExpectOptional("INOUT"))
                {
                    mode = "INOUT";
                }
                else if (parser.ExpectOptional("VARIADIC"))
                {
                    mode = "VARIADIC";
                }
                else
                {
                    mode = null;
                }

                var position = parser.Position;
                string argumentName = null;
                var dataType = parser.ParseDataType();

                var position2 = parser.Position;

                if (!parser.ExpectOptional(")") && !parser.ExpectOptional(","))
                {
                    parser.Position = position;
                    argumentName = ParserUtils.GetObjectName(parser.ParseIdentifier());
                    dataType = parser.ParseDataType();
                }
                else
                {
                    parser.Position = position2;
                }

                var argument = new PgFunction.Argument()
                {
                    DataType = dataType,
                    Mode = mode,
                    Name = argumentName,
                };
                tmpFunction.AddArgument(argument);

                if (parser.ExpectOptional(")"))
                {
                    break;
                }
                else
                {
                    parser.Expect(",");
                }
            }

            PgFunction function = schema.GetFunction(tmpFunction.Signature);

            parser.Expect("IS");
            function.Comment = GetComment(parser);
            parser.Expect(";");
        }

        /// <summary>
        /// Parses comment from parser. If comment is "null" string then null is
        /// returned, otherwise the parsed string is returned.
        /// </summary>
        [return: NullGuard.AllowNull]
        private static string GetComment(Parser parser)
        {
            var comment = parser.ParseStringCompat();

            if ("null".Equals(comment, StringComparison.CurrentCultureIgnoreCase))
            {
                return null;
            }

            return comment;
        }
    }
}