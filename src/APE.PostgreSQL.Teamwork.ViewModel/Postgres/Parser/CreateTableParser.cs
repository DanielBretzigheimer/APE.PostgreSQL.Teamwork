// <copyright file="CreateTableParser.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using APE.PostgreSQL.Teamwork;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres.Parsers
{
    /// <summary>
    /// Parses CREATE TABLE statements.
    /// </summary>
    public class CreateTableParser
    {
        /// <summary>
        /// Parses CREATE TABLE statement.
        /// </summary>
        public static void Parse(PgDatabase database, string statement)
        {
            Parser parser = new Parser(statement);
            parser.Expect("CREATE", "TABLE");

            // Optional IF NOT EXISTS, irrelevant for our purposes
            parser.ExpectOptional("IF", "NOT", "EXISTS");

            string tableName = parser.ParseIdentifier();

            PgTable table = new PgTable(ParserUtils.GetObjectName(tableName));

            string schemaName = ParserUtils.GetSchemaName(tableName, database);

            PgSchema schema = database.GetSchema(schemaName);

            if (schema == null)
                throw new Exception(string.Format("CannotFindSchema {0}. Statement {1}", schemaName, statement));

            schema.AddTable(table);

            parser.Expect("(");

            while (!parser.ExpectOptional(")"))
            {
                if (parser.ExpectOptional("CONSTRAINT"))
                    ParseConstraint(parser, table);
                else
                    ParseColumn(parser, table);

                if (parser.ExpectOptional(")"))
                    break;
                else
                    parser.Expect(",");
            }

            while (!parser.ExpectOptional(";"))
            {
                if (parser.ExpectOptional("INHERITS"))
                    ParseInherits(parser, table);
                else if (parser.ExpectOptional("WITHOUT"))
                    table.With = "OIDS=false";
                else if (parser.ExpectOptional("WITH"))
                {
                    if (parser.ExpectOptional("OIDS") || parser.ExpectOptional("OIDS=true"))
                        table.With = "OIDS=true";
                    else if (parser.ExpectOptional("OIDS=false"))
                        table.With = "OIDS=false";
                    else
                        table.With = parser.Expression;
                }
                else if (parser.ExpectOptional("TABLESPACE"))
                    table.Tablespace = parser.ParseString();
                else
                    parser.ThrowUnsupportedCommand();
            }
        }

        /// <summary>
        /// Parses INHERITS.
        /// </summary>
        private static void ParseInherits(Parser parser, PgTable table)
        {
            parser.Expect("(");

            while (!parser.ExpectOptional(")"))
            {
                table.AddInherits(ParserUtils.GetObjectName(parser.ParseIdentifier()));

                if (parser.ExpectOptional(")"))
                    break;
                else
                    parser.Expect(",");
            }
        }

        /// <summary>
        /// Parses CONSTRAINT definition.
        /// </summary>
        private static void ParseConstraint(Parser parser, PgTable table)
        {
            PgConstraint constraint = new PgConstraint(ParserUtils.GetObjectName(parser.ParseIdentifier()));
            table.AddConstraint(constraint);
            constraint.Definition = parser.Expression;
            constraint.TableName = table.Name;
        }

        /// <summary>
        /// Parses column definition.
        /// </summary>
        private static void ParseColumn(Parser parser, PgTable table)
        {
            PgColumn column = new PgColumn(ParserUtils.GetObjectName(parser.ParseIdentifier()));
            table.AddColumn(column);
            column.ParseDefinition(parser.Expression);
        }
    }
}