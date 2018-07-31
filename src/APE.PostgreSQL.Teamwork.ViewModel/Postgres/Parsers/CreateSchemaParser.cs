// <copyright file="CreateSchemaParser.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres.Parsers
{
    /// <summary>
    /// Parses CREATE SCHEMA statements.
    /// </summary>
    public class CreateSchemaParser
    {
        /// <summary>
        /// Creates a new CreateSchemaParser object.
        /// </summary>
        private CreateSchemaParser()
        {
        }

        /// <summary>
        /// Parses CREATE SCHEMA statement.
        /// </summary>
        public static void Parse(PgDatabase database, string statement)
        {
            var parser = new Parser(statement);
            parser.Expect("CREATE", "SCHEMA");

            if (parser.ExpectOptional("AUTHORIZATION"))
            {
                var schema = new PgSchema(ParserUtils.GetObjectName(parser.ParseIdentifier()));
                database.AddSchema(schema);
                schema.Authorization = schema.Name;

                var definition = parser.Rest();
                if (definition != null && definition.Length > 0)
                {
                    schema.Definition = definition;
                }
            }
            else
            {
                var schema = new PgSchema(ParserUtils.GetObjectName(parser.ParseIdentifier()));
                database.AddSchema(schema);

                if (parser.ExpectOptional("AUTHORIZATION"))
                {
                    schema.Authorization = ParserUtils.GetObjectName(parser.ParseIdentifier());
                }

                var definition = parser.Rest();
                if (definition != null && definition.Length > 0)
                {
                    schema.Definition = definition;
                }
            }
        }
    }
}