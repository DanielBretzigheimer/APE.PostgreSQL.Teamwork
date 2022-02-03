// <copyright file="CreateFunctionParser.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;

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
            var parser = new Parser(statement);
            parser.Expect("CREATE");
            parser.ExpectOptional("OR", "REPLACE");
            parser.Expect("FUNCTION");

            var functionName = parser.ParseIdentifier();
            var schemaName = ParserUtils.GetSchemaName(functionName, database);

            var schema = database.GetSchema(schemaName);

            if (database.SchemaIsIgnored(schemaName))
                return;

            if (schema == null)
            {
                throw new TeamworkParserException($"CannotFindSchema {schemaName} {statement}");
            }

            var function = new PgFunction()
            {
                Name = ParserUtils.GetObjectName(functionName),
            };
            schema.Add(function);

            parser.Expect("(");

            while (!parser.ExpectOptional(")"))
            {
                string? mode;
                if (parser.ExpectOptional("IN"))
                {
                    mode = "IN";
                }
                else if (parser.ExpectOptional("OUT"))
                {
                    mode = "OUT";
                }
                else if (parser.ExpectOptional("INOUT"))
                {
                    mode = "INOUT";
                }
                else if (parser.ExpectOptional("VARIADIC"))
                {
                    mode = "VARIADIC";
                }
                else
                {
                    mode = null;
                }

                var position = parser.Position;
                string? argumentName = null;
                var dataType = parser.ParseDataType();

                var position2 = parser.Position;

                if (!parser.ExpectOptional(")") && !parser.ExpectOptional(",") && !parser.ExpectOptional("=") && !parser.ExpectOptional("DEFAULT"))
                {
                    parser.Position = position;
                    argumentName = ParserUtils.GetObjectName(parser.ParseIdentifier());
                    dataType = parser.ParseDataType();
                }
                else
                {
                    parser.Position = position2;
                }

                string? defaultExpression;
                if (parser.ExpectOptional("=") || parser.ExpectOptional("DEFAULT"))
                {
                    defaultExpression = parser.Expression();
                }
                else
                {
                    defaultExpression = null;
                }

                var argument = new PgFunction.Argument()
                {
                    DataType = dataType,
                    DefaultExpression = defaultExpression,
                    Mode = mode,
                    Name = argumentName,
                };
                function.AddArgument(argument);

                if (parser.ExpectOptional(")"))
                {
                    break;
                }
                else
                {
                    parser.Expect(",");
                }
            }

            function.Body = parser.Rest();
        }
    }
}