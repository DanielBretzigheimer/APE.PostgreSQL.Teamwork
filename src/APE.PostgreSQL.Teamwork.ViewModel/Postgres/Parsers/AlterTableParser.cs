// <copyright file="AlterTableParser.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres.Parsers
{
    /// <summary>
    /// Parses ALTER TABLE statements.
    /// </summary>
    public class AlterTableParser
    {
        /// <summary>
        /// Creates a new instance of AlterTableParser.
        /// </summary>
        private AlterTableParser()
        {
        }

        /// <summary>
        /// Parses ALTER TABLE statement.
        /// </summary>
        public static void Parse(PgDatabase database, string statement, bool outputIgnoredStatements)
        {
            var parser = new Parser(statement);
            parser.Expect("ALTER", "TABLE");
            parser.ExpectOptional("ONLY");

            var tableName = parser.ParseIdentifier();

            var schemaName = ParserUtils.GetSchemaName(tableName, database);

            PgSchema schema = database.GetSchema(schemaName);

            if (schema == null)
            {
                throw new TeamworkParserException($"CannotFindSchema {schemaName} from {statement}");
            }

            var objectName = ParserUtils.GetObjectName(tableName);

            PgTable table = schema.GetTable(objectName);

            if (table == null)
            {
                PgView view = schema.GetView(objectName);

                if (view != null)
                {
                    ParseView(parser, view, outputIgnoredStatements, tableName, database);
                    return;
                }

                PgSequence sequence = schema.GetSequence(objectName);

                if (sequence != null)
                {
                    ParseSequence(parser, sequence, outputIgnoredStatements, tableName, database);
                    return;
                }

                throw new TeamworkParserException($"CannotFindObject in {tableName} from {statement}");
            }

            while (!parser.ExpectOptional(";"))
            {
                if (parser.ExpectOptional("ALTER"))
                {
                    ParseAlterColumn(parser, table);
                }
                else if (parser.ExpectOptional("CLUSTER", "ON"))
                {
                    table.ClusterIndexName = ParserUtils.GetObjectName(parser.ParseIdentifier());
                }
                else if (parser.ExpectOptional("OWNER", "TO"))
                {
                    // we do not parse this one so we just consume the identifier
                    if (outputIgnoredStatements)
                    {
                        database.AddIgnoredStatement("ALTER TABLE " + tableName + " OWNER TO " + parser.ParseIdentifier() + ';');
                    }
                    else
                    {
                        parser.ParseIdentifier();
                    }
                }
                else if (parser.ExpectOptional("ADD"))
                {
                    if (parser.ExpectOptional("FOREIGN", "KEY"))
                    {
                        ParseAddForeignKey(parser, table);
                    }
                    else if (parser.ExpectOptional("CONSTRAINT"))
                    {
                        ParseAddConstraint(parser, table, schema);
                    }
                    else
                    {
                        parser.ThrowUnsupportedCommand();
                    }
                }
                else if (parser.ExpectOptional("ENABLE"))
                {
                    ParseEnable(parser, outputIgnoredStatements, tableName, database);
                }
                else if (parser.ExpectOptional("DISABLE"))
                {
                    ParseDisable(parser, outputIgnoredStatements, tableName, database);
                }
                else
                {
                    parser.ThrowUnsupportedCommand();
                }

                if (parser.ExpectOptional(";"))
                {
                    break;
                }
                else
                {
                    parser.Expect(",");
                }
            }
        }

        /// <summary>
        /// Parses ENABLE statements.
        /// </summary>
        private static void ParseEnable(Parser parser, bool outputIgnoredStatements, string tableName, PgDatabase database)
        {
            if (parser.ExpectOptional("REPLICA"))
            {
                if (parser.ExpectOptional("TRIGGER"))
                {
                    if (outputIgnoredStatements)
                    {
                        database.AddIgnoredStatement("ALTER TABLE " + tableName + " ENABLE REPLICA TRIGGER " + parser.ParseIdentifier() + ';');
                    }
                    else
                    {
                        parser.ParseIdentifier();
                    }
                }
                else if (parser.ExpectOptional("RULE"))
                {
                    if (outputIgnoredStatements)
                    {
                        database.AddIgnoredStatement("ALTER TABLE " + tableName + " ENABLE REPLICA RULE " + parser.ParseIdentifier() + ';');
                    }
                    else
                    {
                        parser.ParseIdentifier();
                    }
                }
                else
                {
                    parser.ThrowUnsupportedCommand();
                }
            }
            else if (parser.ExpectOptional("ALWAYS"))
            {
                if (parser.ExpectOptional("TRIGGER"))
                {
                    if (outputIgnoredStatements)
                    {
                        database.AddIgnoredStatement("ALTER TABLE " + tableName + " ENABLE ALWAYS TRIGGER " + parser.ParseIdentifier() + ';');
                    }
                    else
                    {
                        parser.ParseIdentifier();
                    }
                }
                else if (parser.ExpectOptional("RULE"))
                {
                    if (outputIgnoredStatements)
                    {
                        database.AddIgnoredStatement("ALTER TABLE " + tableName + " ENABLE RULE " + parser.ParseIdentifier() + ';');
                    }
                    else
                    {
                        parser.ParseIdentifier();
                    }
                }
                else
                {
                    parser.ThrowUnsupportedCommand();
                }
            }
        }

        /// <summary>
        /// Parses DISABLE statements.
        /// </summary>
        private static void ParseDisable(Parser parser, bool outputIgnoredStatements, string tableName, PgDatabase database)
        {
            if (parser.ExpectOptional("TRIGGER"))
            {
                if (outputIgnoredStatements)
                {
                    database.AddIgnoredStatement("ALTER TABLE " + tableName + " DISABLE TRIGGER " + parser.ParseIdentifier() + ';');
                }
                else
                {
                    parser.ParseIdentifier();
                }
            }
            else if (parser.ExpectOptional("RULE"))
            {
                if (outputIgnoredStatements)
                {
                    database.AddIgnoredStatement("ALTER TABLE " + tableName + " DISABLE RULE " + parser.ParseIdentifier() + ';');
                }
                else
                {
                    parser.ParseIdentifier();
                }
            }
            else
            {
                parser.ThrowUnsupportedCommand();
            }
        }

        /// <summary>
        /// Parses ADD CONSTRAINT action.
        /// </summary>
        private static void ParseAddConstraint(Parser parser, PgTable table, PgSchema schema)
        {
            var constraintName = ParserUtils.GetObjectName(parser.ParseIdentifier());

            var constraint = new PgConstraint(constraintName)
            {
                TableName = table.Name,
            };
            table.AddConstraint(constraint);

            if (parser.ExpectOptional("PRIMARY", "KEY"))
            {
                schema.AddPrimaryKey(constraint);
                constraint.Definition = "PRIMARY KEY " + parser.Expression();
            }
            else
            {
                constraint.Definition = parser.Expression();
            }
        }

        /// <summary>
        /// Parses ALTER COLUMN action.
        /// </summary>
        private static void ParseAlterColumn(Parser parser, PgTable table)
        {
            parser.ExpectOptional("COLUMN");

            var columnName = ParserUtils.GetObjectName(parser.ParseIdentifier());

            if (parser.ExpectOptional("SET"))
            {
                if (parser.ExpectOptional("STATISTICS"))
                {
                    PgColumn column = table.GetColumn(columnName);

                    if (column == null)
                    {
                        throw new TeamworkParserException($"CannotFindTableColumn {columnName} in table {table.Name} from {parser.String}");
                    }

                    column.Statistics = parser.ParseInteger();
                }
                else if (parser.ExpectOptional("DEFAULT"))
                {
                    var defaultValue = parser.Expression();

                    if (table.ContainsColumn(columnName))
                    {
                        PgColumn column = table.GetColumn(columnName);

                        if (column == null)
                        {
                            throw new TeamworkParserException($"CannotFindTableColumn {columnName} in table {table.Name} from {parser.String}");
                        }

                        column.DefaultValue = defaultValue;
                    }
                    else
                    {
                        throw new TeamworkParserException($"CannotFindColumnInTable {columnName} in table {table.Name}");
                    }
                }
                else if (parser.ExpectOptional("STORAGE"))
                {
                    PgColumn column = table.GetColumn(columnName);

                    if (column == null)
                    {
                        throw new TeamworkParserException($"CannotFindTableColumn {columnName} in table {table.Name} from {parser.String}");
                    }

                    if (parser.ExpectOptional("PLAIN"))
                    {
                        column.Storage = "PLAIN";
                    }
                    else if (parser.ExpectOptional("EXTERNAL"))
                    {
                        column.Storage = "EXTERNAL";
                    }
                    else if (parser.ExpectOptional("EXTENDED"))
                    {
                        column.Storage = "EXTENDED";
                    }
                    else if (parser.ExpectOptional("MAIN"))
                    {
                        column.Storage = "MAIN";
                    }
                    else
                    {
                        parser.ThrowUnsupportedCommand();
                    }
                }
                else
                {
                    parser.ThrowUnsupportedCommand();
                }
            }
            else
            {
                parser.ThrowUnsupportedCommand();
            }
        }

        /// <summary>
        /// Parses ADD FOREIGN KEY action.
        /// </summary>
        private static void ParseAddForeignKey(Parser parser, PgTable table)
        {
            IList<string> columnNames = new List<string>(1);
            parser.Expect("(");

            while (!parser.ExpectOptional(")"))
            {
                columnNames.Add(ParserUtils.GetObjectName(parser.ParseIdentifier()));

                if (parser.ExpectOptional(")"))
                {
                    break;
                }
                else
                {
                    parser.Expect(",");
                }
            }

            var constraintName = ParserUtils.GenerateName(table.Name + "_", columnNames, "_fkey");

            var constraint = new PgConstraint(constraintName);
            table.AddConstraint(constraint);
            constraint.Definition = parser.Expression();
            constraint.TableName = table.Name;
        }

        /// <summary>
        /// Parses ALTER TABLE view.
        /// </summary>
        private static void ParseView(Parser parser, PgView view, bool outputIgnoredStatements, string viewName, PgDatabase database)
        {
            while (!parser.ExpectOptional(";"))
            {
                if (parser.ExpectOptional("ALTER"))
                {
                    parser.ExpectOptional("COLUMN");

                    var columnName = ParserUtils.GetObjectName(parser.ParseIdentifier());

                    if (parser.ExpectOptional("SET", "DEFAULT"))
                    {
                        var expression = parser.Expression();
                        view.AddColumnDefaultValue(columnName, expression);
                    }
                    else if (parser.ExpectOptional("DROP", "DEFAULT"))
                    {
                        view.RemoveColumnDefaultValue(columnName);
                    }
                    else
                    {
                        parser.ThrowUnsupportedCommand();
                    }
                }
                else if (parser.ExpectOptional("OWNER", "TO"))
                {
                    // we do not parse this one so we just consume the identifier
                    if (outputIgnoredStatements)
                    {
                        database.AddIgnoredStatement("ALTER TABLE " + viewName + " OWNER TO " + parser.ParseIdentifier() + ';');
                    }
                    else
                    {
                        parser.ParseIdentifier();
                    }
                }
                else
                {
                    parser.ThrowUnsupportedCommand();
                }
            }
        }

        /// <summary>
        /// Parses ALTER TABLE sequence.
        /// </summary>
        private static void ParseSequence(Parser parser, PgSequence sequence, bool outputIgnoredStatements, string sequenceName, PgDatabase database)
        {
            while (!parser.ExpectOptional(";"))
            {
                if (parser.ExpectOptional("OWNER", "TO"))
                {
                    // we do not parse this one so we just consume the identifier
                    if (outputIgnoredStatements)
                    {
                        database.AddIgnoredStatement("ALTER TABLE " + sequenceName + " OWNER TO " + parser.ParseIdentifier() + ';');
                    }
                    else
                    {
                        parser.ParseIdentifier();
                    }
                }
                else
                {
                    parser.ThrowUnsupportedCommand();
                }
            }
        }
    }
}