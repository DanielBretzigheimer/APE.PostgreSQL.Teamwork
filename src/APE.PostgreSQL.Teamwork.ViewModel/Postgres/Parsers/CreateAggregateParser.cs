// <copyright file="CreateAggregateParser.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;
using Serilog;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres.Parsers
{
    /// <summary>
    /// Parses CREATE AGGREGATE and CREATE OR REPLACE AGGREGATE statements.
    /// </summary>
    public class CreateAggregateParser
    {
        /// <summary>
        /// Creates a new instance of CreateAggregateParser.
        /// </summary>
        private CreateAggregateParser()
        {
        }

        /// <summary>
        /// Parses CREATE AGGREGATE and CREATE OR REPLACE AGGREGATE statement.
        /// </summary>
        public static void Parse(PgDatabase database, string statement)
        {
            var parser = new Parser(statement);
            parser.Expect("CREATE");
            parser.Expect("AGGREGATE");

            var aggregateName = parser.ParseIdentifier();

            var schemaName = ParserUtils.GetSchemaName(aggregateName, database);

            if (database.SchemaIsIgnored(schemaName))
                return;

            var schema = database.GetSchema(schemaName);

            if (schema == null)
            {
                throw new Exception(string.Format("CannotFindSchema {0} for {1}", schemaName, statement));
            }

            var aggregate = new PgAggregate()
            {
                Name = ParserUtils.GetObjectName(aggregateName),
            };
            schema.Add(aggregate);

            parser.Expect("(");

            var arguments = new List<PgAggregate.Argument>();

            var first = true;
            while (!parser.ExpectOptional(")"))
            {
                if (!first)
                {
                    try
                    {
                        parser.Expect(",");
                    }
                    catch (TeamworkParserException ex)
                    {
                        var tmpArgument = arguments.Last();

                        // last data type was a name
                        Log.Warning(string.Format("Aggregate {0} had a parameter which was not fully a data type (could be because it contained the name). {1} was ignored!", statement, tmpArgument.DataType), ex);
                        arguments.Remove(tmpArgument);
                    }
                }

                var dataType = parser.ParseDataType();

                var argument = new PgAggregate.Argument()
                {
                    DataType = dataType,
                };
                arguments.Add(argument);
                first = false;
            }

            foreach (var argument in arguments)
                aggregate.Arguments.Add(argument);

            aggregate.Body = parser.Rest();
        }
    }
}