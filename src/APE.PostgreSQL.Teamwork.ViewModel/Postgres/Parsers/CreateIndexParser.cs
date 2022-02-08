// <copyright file="CreateIndexParser.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres.Parsers
{
    /// <summary>
    /// Parses CREATE INDEX statements.
    /// </summary>
    public class CreateIndexParser
    {
        /// <summary>
        /// Creates a new instance of CreateIndexParser.
        /// </summary>
        private CreateIndexParser()
        {
        }

        /// <summary>
        /// Parses CREATE INDEX statement.
        /// </summary>
        public static void Parse(PgDatabase database, string statement)
        {
            var parser = new Parser(statement);
            parser.Expect("CREATE");

            var unique = parser.ExpectOptional("UNIQUE");

            parser.Expect("INDEX");
            parser.ExpectOptional("CONCURRENTLY");

            var indexName = ParserUtils.GetObjectName(parser.ParseIdentifier());

            parser.Expect("ON");

            var tableName = parser.ParseIdentifier();

            var definition = parser.Rest()
                ?? throw new NullReferenceException($"Index definition for {indexName} was null.");

            var schemaName = ParserUtils.GetSchemaName(tableName, database);

            if (database.SchemaIsIgnored(schemaName))
                return;

            var schema = database.GetSchema(schemaName);

            if (schema == null)
            {
                throw new TeamworkParserException($"CannotFindSchema {schemaName} from  {statement}");
            }

            var objectName = ParserUtils.GetObjectName(tableName);

            var table = schema.GetTable(objectName);

            if (table == null)
            {
                throw new TeamworkParserException($"CannotFindTable {tableName} from {statement}");
            }

            var index = new PgIndex(indexName);
            table.AddIndex(index);
            schema.Add(index);
            index.Definition = definition.Trim();
            index.TableName = table.Name;
            index.Unique = unique;
        }
    }
}