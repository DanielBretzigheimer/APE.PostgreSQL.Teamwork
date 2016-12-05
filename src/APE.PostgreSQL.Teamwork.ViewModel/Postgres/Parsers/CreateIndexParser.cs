// <copyright file="CreateIndexParser.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;

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
            Parser parser = new Parser(statement);
            parser.Expect("CREATE");

            bool unique = parser.ExpectOptional("UNIQUE");

            parser.Expect("INDEX");
            parser.ExpectOptional("CONCURRENTLY");

            string indexName = ParserUtils.GetObjectName(parser.ParseIdentifier());

            parser.Expect("ON");

            string tableName = parser.ParseIdentifier();

            string definition = parser.Rest;

            string schemaName = ParserUtils.GetSchemaName(tableName, database);

            PgSchema schema = database.GetSchema(schemaName);

            if (schema == null)
                throw new Exception(string.Format("CannotFindSchema", schemaName, statement));

            string objectName = ParserUtils.GetObjectName(tableName);

            PgTable table = schema.GetTable(objectName);

            if (table == null)
                throw new Exception(string.Format("CannotFindTable", tableName, statement));

            PgIndex index = new PgIndex(indexName);
            table.AddIndex(index);
            schema.AddIndex(index);
            index.Definition = definition.Trim();
            index.TableName = table.Name;
            index.Unique = unique;
        }
    }
}