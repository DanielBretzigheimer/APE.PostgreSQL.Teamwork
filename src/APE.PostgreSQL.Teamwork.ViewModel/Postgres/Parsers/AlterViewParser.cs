// <copyright file="AlterViewParser.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres.Parsers
{
    /// <summary>
    /// Parses ALTER VIEW statements.
    /// </summary>
    public class AlterViewParser
    {
        /// <summary>
        /// Creates new instance of AlterViewParser.
        /// </summary>
        private AlterViewParser()
        {
        }

        /// <summary>
        /// Parses ALTER VIEW statement.
        /// </summary>
        public static void Parse(PgDatabase database, string statement, bool outputIgnoredStatements)
        {
            var parser = new Parser(statement);
            parser.Expect("ALTER", "VIEW");

            var viewName = parser.ParseIdentifier();

            var schemaName = ParserUtils.GetSchemaName(viewName, database);

            if (database.SchemaIsIgnored(schemaName))
                return;

            var schema = database.GetSchema(schemaName);

            if (schema == null)
            {
                throw new TeamworkParserException($"CannotFindSchema {schemaName} from {statement}");
            }

            var objectName = ParserUtils.GetObjectName(viewName);

            var view = schema.GetView(objectName);

            if (view == null)
            {
                throw new TeamworkParserException($"CannotFindView {viewName} from {statement}");
            }

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
                        throw new TeamworkParserException("CannotParseStringUnsupportedCommand");
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
                    throw new TeamworkParserException("CannotParseStringUnsupportedCommand");
                }
            }
        }
    }
}