// <copyright file="CreateViewParser.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;

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
            Parser parser = new Parser(statement);
            parser.Expect("CREATE");
            parser.ExpectOptional("OR", "REPLACE");
            parser.Expect("VIEW");

            string viewName = parser.ParseIdentifier();

            bool columnsExist = parser.ExpectOptional("(");

            List<string> columnNames = new List<string>(10);

            if (columnsExist)
            {
                while (!parser.ExpectOptional(")"))
                {
                    columnNames.Add(ParserUtils.GetObjectName(parser.ParseIdentifier()));
                    parser.ExpectOptional(",");
                }
            }

            parser.Expect("AS");

            string query = parser.Rest;

            PgView view = new PgView(ParserUtils.GetObjectName(viewName));
            view.ColumnNames = columnNames;
            view.Query = query;

            string schemaName = ParserUtils.GetSchemaName(viewName, database);

            PgSchema schema = database.GetSchema(schemaName);

            if (schema == null)
                throw new Exception(string.Format("CannotFindSchema", schemaName, statement));

            schema.AddView(view);
        }
    }
}