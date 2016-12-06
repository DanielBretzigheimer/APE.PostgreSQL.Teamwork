// <copyright file="AlterViewParser.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;

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
            Parser parser = new Parser(statement);
            parser.Expect("ALTER", "VIEW");

            string viewName = parser.ParseIdentifier();

            string schemaName = ParserUtils.GetSchemaName(viewName, database);

            PgSchema schema = database.GetSchema(schemaName);

            if (schema == null)
                throw new Exception(string.Format("CannotFindSchema", schemaName, statement));

            string objectName = ParserUtils.GetObjectName(viewName);

            PgView view = schema.GetView(objectName);

            if (view == null)
                throw new Exception(string.Format("CannotFindView", viewName, statement));

            while (!parser.ExpectOptional(";"))
            {
                if (parser.ExpectOptional("ALTER"))
                {
                    parser.ExpectOptional("COLUMN");

                    string columnName = ParserUtils.GetObjectName(parser.ParseIdentifier());

                    if (parser.ExpectOptional("SET", "DEFAULT"))
                    {
                        string expression = parser.Expression;
                        view.AddColumnDefaultValue(columnName, expression);
                    }
                    else if (parser.ExpectOptional("DROP", "DEFAULT"))
                        view.RemoveColumnDefaultValue(columnName);
                    else
                    {
                        parser.ThrowUnsupportedCommand();
                    }
                }
                else if (parser.ExpectOptional("OWNER", "TO"))
                {
                    // we do not parse this one so we just consume the identifier
                    if (outputIgnoredStatements)
                        database.AddIgnoredStatement("ALTER TABLE " + viewName + " OWNER TO " + parser.ParseIdentifier() + ';');
                    else
                        parser.ParseIdentifier();
                }
                else
                    parser.ThrowUnsupportedCommand();
            }
        }
    }
}