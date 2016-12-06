﻿// <copyright file="CreateSequenceParser.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres.Parsers
{
    /// <summary>
    /// Parses CREATE SEQUENCE statements.
    /// </summary>
    public class CreateSequenceParser
    {
        /// <summary>
        /// Creates a new instance of CreateSequenceParser.
        /// </summary>
        private CreateSequenceParser()
        {
        }

        /// <summary>
        /// Parses CREATE SEQUENCE statement.
        /// </summary>
        public static void Parse(PgDatabase database, string statement)
        {
            Parser parser = new Parser(statement);
            parser.Expect("CREATE", "SEQUENCE");

            string sequenceName = parser.ParseIdentifier();

            PgSequence sequence = new PgSequence(ParserUtils.GetObjectName(sequenceName));

            string schemaName = ParserUtils.GetSchemaName(sequenceName, database);

            PgSchema schema = database.GetSchema(schemaName);

            if (schema == null)
                throw new Exception(string.Format("CannotFindSchema", schemaName, statement));

            schema.AddSequence(sequence);

            while (!parser.ExpectOptional(";"))
            {
                if (parser.ExpectOptional("INCREMENT"))
                {
                    parser.ExpectOptional("BY");
                    sequence.Increment = parser.ParseString();
                }
                else if (parser.ExpectOptional("MINVALUE"))
                    sequence.MinValue = parser.ParseString();
                else if (parser.ExpectOptional("MAXVALUE"))
                    sequence.MaxValue = parser.ParseString();
                else if (parser.ExpectOptional("START"))
                {
                    parser.ExpectOptional("WITH");
                    sequence.StartWith = parser.ParseString();
                }
                else if (parser.ExpectOptional("CACHE"))
                    sequence.Cache = parser.ParseString();
                else if (parser.ExpectOptional("CYCLE"))
                    sequence.Cycle = true;
                else if (parser.ExpectOptional("OWNED", "BY"))
                {
                    if (parser.ExpectOptional("NONE"))
                        sequence.Owner = null;
                    else
                    {
                        sequence.Owner = ParserUtils.GetObjectName(parser.ParseIdentifier());
                    }
                }
                else if (parser.ExpectOptional("NO"))
                {
                    if (parser.ExpectOptional("MINVALUE"))
                        sequence.MinValue = null;
                    else if (parser.ExpectOptional("MAXVALUE"))
                        sequence.MaxValue = null;
                    else if (parser.ExpectOptional("CYCLE"))
                        sequence.Cycle = false;
                    else
                    {
                        parser.ThrowUnsupportedCommand();
                    }
                }
                else
                {
                    parser.ThrowUnsupportedCommand();
                }
            }
        }
    }
}