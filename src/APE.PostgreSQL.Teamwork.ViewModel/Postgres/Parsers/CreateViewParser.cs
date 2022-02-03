// <copyright file="CreateViewParser.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres.Parsers
{
    /// <summary>
    /// Parses CREATE VIEW statements.
    /// </summary>
    public class CreateViewParser
    {
        /// <summary>
        /// Creates a new instance of CreateViewParser.
        /// </summary>
        private CreateViewParser()
        {
        }

        /// <summary>
        /// Parses CREATE VIEW statement.
        /// </summary>
        public static void Parse(PgDatabase database, string statement)
        {
            var parser = new Parser(statement);
            parser.Expect("CREATE");
            parser.ExpectOptional("OR", "REPLACE");
            parser.Expect("VIEW");

            var viewName = parser.ParseIdentifier();

            var columnsExist = parser.ExpectOptional("(");

            var columnNames = new List<string>(10);

            if (columnsExist)
            {
                while (!parser.ExpectOptional(")"))
                {
                    columnNames.Add(ParserUtils.GetObjectName(parser.ParseIdentifier()));
                    parser.ExpectOptional(",");
                }
            }

            parser.ExpectOptional("WITH", "(security_barrier='false')");

            parser.Expect("AS");

            var query = parser.Rest();

            var view = new PgView(ParserUtils.GetObjectName(viewName))
            {
                ColumnNames = columnNames,
                Query = query,
            };
            var schemaName = ParserUtils.GetSchemaName(viewName, database);

            if (database.SchemaIsIgnored(schemaName))
                return;

            var schema = database.GetSchema(schemaName);

            if (schema == null)
            {
                throw new TeamworkParserException($"CannotFindSchema {schemaName} {statement}");
            }

            schema.Add(view);
        }
    }
}