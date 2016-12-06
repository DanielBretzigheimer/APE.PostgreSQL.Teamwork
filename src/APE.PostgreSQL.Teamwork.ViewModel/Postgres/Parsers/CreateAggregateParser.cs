// <copyright file="CreateAggregateParser.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;
using log4net;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres.Parsers
{
    /// <summary>
    /// Parses CREATE AGGREGATE and CREATE OR REPLACE AGGREGATE statements.
    /// </summary>
    public class CreateAggregateParser
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            Parser parser = new Parser(statement);
            parser.Expect("CREATE");
            parser.Expect("AGGREGATE");

            string aggregateName = parser.ParseIdentifier();

            string schemaName = ParserUtils.GetSchemaName(aggregateName, database);

            PgSchema schema = database.GetSchema(schemaName);

            if (schema == null)
                throw new Exception(string.Format("CannotFindSchema {0} for {1}", schemaName, statement));

            PgAggregate aggregate = new PgAggregate();
            aggregate.Name = ParserUtils.GetObjectName(aggregateName);
            schema.AddAggregate(aggregate);

            parser.Expect("(");

            List<PgAggregate.Argument> arguments = new List<PgAggregate.Argument>();

            bool first = true;
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
                        Log.Warn(string.Format("Aggregate {0} had a parameter which was not fully a data type (could be because it contained the name). {1} was ignored!", statement, tmpArgument.DataType), ex);
                        arguments.Remove(tmpArgument);
                    }
                }

                string dataType = parser.ParseDataType();

                PgAggregate.Argument argument = new PgAggregate.Argument();
                argument.DataType = dataType;
                arguments.Add(argument);
                first = false;
            }

            foreach (var argument in arguments)
                aggregate.Arguments.Add(argument);

            aggregate.Body = parser.Rest;
        }
    }
}