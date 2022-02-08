// <copyright file="CreateTableParser.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;

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
            var parser = new Parser(statement);
            parser.Expect("CREATE", "TABLE");

            // Optional IF NOT EXISTS, irrelevant for our purposes
            parser.ExpectOptional("IF", "NOT", "EXISTS");

            var tableIdentifier = parser.ParseIdentifier();
            var table = new PgTable(ParserUtils.GetObjectName(tableIdentifier));
            var schemaName = ParserUtils.GetSchemaName(tableIdentifier, database);

            if (database.SchemaIsIgnored(schemaName))
                return;

            var schema = database.GetSchema(schemaName);
            if (schema == null)
                throw new Exception(string.Format("CannotFindSchema {0}. Statement {1}", schemaName, statement));

            schema.Add(table);

            parser.Expect("(");

            while (!parser.ExpectOptional(")"))
            {
                if (parser.ExpectOptional("CONSTRAINT"))
                    ParseConstraint(parser, table);
                else
                    ParseColumn(parser, table);

                if (parser.ExpectOptional(")"))
                {
                    break;
                }
                else
                {
                    parser.Expect(",");
                }
            }

            while (!parser.ExpectOptional(";"))
            {
                if (parser.ExpectOptional("INHERITS"))
                {
                    ParseInherits(parser, table);
                }
                else if (parser.ExpectOptional("WITHOUT"))
                {
                    table.With = "OIDS=false";
                }
                else if (parser.ExpectOptional("WITH"))
                {
                    if (parser.ExpectOptional("OIDS") || parser.ExpectOptional("OIDS=true"))
                    {
                        table.With = "OIDS=true";
                    }
                    else if (parser.ExpectOptional("OIDS=false"))
                    {
                        table.With = "OIDS=false";
                    }
                    else
                    {
                        table.With = parser.Expression();
                    }
                }
                else if (parser.ExpectOptional("TABLESPACE"))
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    table.Tablespace = parser.ParseStringCompat();
#pragma warning restore CS0618 // Type or member is obsolete
                }
                else
                {
                    throw new TeamworkParserException("CannotParseStringUnsupportedCommand");
                }
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
        /// Parses CONSTRAINT definition.
        /// </summary>
        private static void ParseConstraint(Parser parser, PgTable table)
        {
            var constraintName = ParserUtils.GetObjectName(parser.ParseIdentifier());
            var constraint = new PgConstraint(constraintName, table.Name, parser.Expression());
            table.AddConstraint(constraint);
        }

        /// <summary>
        /// Parses column definition.
        /// </summary>
        private static void ParseColumn(Parser parser, PgTable table)
        {
            var colName = ParserUtils.GetObjectName(parser.ParseIdentifier());
            var column = new PgColumn(colName, parser.Expression());
            table.AddColumn(column);
        }
    }
}