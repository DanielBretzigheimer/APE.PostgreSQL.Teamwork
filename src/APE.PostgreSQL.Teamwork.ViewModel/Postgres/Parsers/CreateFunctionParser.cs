// <copyright file="CreateFunctionParser.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres.Parsers
{
    /// <summary>
    /// Parses CREATE FUNCTION and CREATE OR REPLACE FUNCTION statements.
    /// </summary>
    public class CreateFunctionParser
    {
        /// <summary>
        /// Creates a new instance of CreateFunctionParser.
        /// </summary>
        private CreateFunctionParser()
        {
        }

        /// <summary>
        /// Parses CREATE FUNCTION and CREATE OR REPLACE FUNCTION statement.
        /// </summary>
        public static void Parse(PgDatabase database, string statement)
        {
            Parser parser = new Parser(statement);
            parser.Expect("CREATE");
            parser.ExpectOptional("OR", "REPLACE");
            parser.Expect("FUNCTION");

            string functionName = parser.ParseIdentifier();
            string schemaName = ParserUtils.GetSchemaName(functionName, database);

            PgSchema schema = database.GetSchema(schemaName);

            if (schema == null)
                throw new Exception(string.Format("CannotFindSchema", schemaName, statement));

            PgFunction function = new PgFunction();
            function.Name = ParserUtils.GetObjectName(functionName);
            schema.AddFunction(function);

            parser.Expect("(");

            while (!parser.ExpectOptional(")"))
            {
                string mode;
                if (parser.ExpectOptional("IN"))
                    mode = "IN";
                else if (parser.ExpectOptional("OUT"))
                    mode = "OUT";
                else if (parser.ExpectOptional("INOUT"))
                    mode = "INOUT";
                else if (parser.ExpectOptional("VARIADIC"))
                    mode = "VARIADIC";
                else
                    mode = null;

                int position = parser.Position;
                string argumentName = null;
                string dataType = parser.ParseDataType();

                int position2 = parser.Position;

                if (!parser.ExpectOptional(")") && !parser.ExpectOptional(",") && !parser.ExpectOptional("=") && !parser.ExpectOptional("DEFAULT"))
                {
                    parser.Position = position;
                    argumentName = ParserUtils.GetObjectName(parser.ParseIdentifier());
                    dataType = parser.ParseDataType();
                }
                else
                    parser.Position = position2;

                string defaultExpression;
                if (parser.ExpectOptional("=") || parser.ExpectOptional("DEFAULT"))
                    defaultExpression = parser.Expression;
                else
                    defaultExpression = null;

                PgFunction.Argument argument = new PgFunction.Argument();
                argument.DataType = dataType;
                argument.DefaultExpression = defaultExpression;
                argument.Mode = mode;
                argument.Name = argumentName;
                function.AddArgument(argument);

                if (parser.ExpectOptional(")"))
                    break;
                else
                {
                    parser.Expect(",");
                }
            }

            function.Body = parser.Rest;
        }
    }
}