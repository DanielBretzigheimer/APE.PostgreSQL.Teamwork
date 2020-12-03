// <copyright file="CreateSequenceParser.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;

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
            var parser = new Parser(statement);
            parser.Expect("CREATE", "SEQUENCE");

            var sequenceName = parser.ParseIdentifier();

            var sequence = new PgSequence(ParserUtils.GetObjectName(sequenceName));

            var schemaName = ParserUtils.GetSchemaName(sequenceName, database);

            if (database.SchemaIsIgnored(schemaName))
                return;

            PgSchema schema = database.GetSchema(schemaName);

            if (schema == null)
            {
                throw new TeamworkParserException($"CannotFindSchema {schemaName} from {statement}");
            }

            schema.Add(sequence);

            while (!parser.ExpectOptional(";"))
            {
                if (parser.ExpectOptional("INCREMENT"))
                {
                    parser.ExpectOptional("BY");
#pragma warning disable CS0618 // Type or member is obsolete
                    sequence.Increment = parser.ParseStringCompat();
#pragma warning restore CS0618 // Type or member is obsolete
                }
                else if (parser.ExpectOptional("MINVALUE"))
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    sequence.MinValue = parser.ParseStringCompat();
#pragma warning restore CS0618 // Type or member is obsolete
                }
                else if (parser.ExpectOptional("MAXVALUE"))
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    sequence.MaxValue = parser.ParseStringCompat();
#pragma warning restore CS0618 // Type or member is obsolete
                }
                else if (parser.ExpectOptional("START"))
                {
                    parser.ExpectOptional("WITH");
#pragma warning disable CS0618 // Type or member is obsolete
                    sequence.StartWith = parser.ParseStringCompat();
#pragma warning restore CS0618 // Type or member is obsolete
                }
                else if (parser.ExpectOptional("CACHE"))
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    sequence.Cache = parser.ParseStringCompat();
#pragma warning restore CS0618 // Type or member is obsolete
                }
                else if (parser.ExpectOptional("CYCLE"))
                {
                    sequence.Cycle = true;
                }
                else if (parser.ExpectOptional("OWNED", "BY"))
                {
                    if (parser.ExpectOptional("NONE"))
                    {
                        sequence.Owner = null;
                    }
                    else
                    {
                        sequence.Owner = ParserUtils.GetObjectName(parser.ParseIdentifier());
                    }
                }
                else if (parser.ExpectOptional("NO"))
                {
                    if (parser.ExpectOptional("MINVALUE"))
                    {
                        sequence.MinValue = null;
                    }
                    else if (parser.ExpectOptional("MAXVALUE"))
                    {
                        sequence.MaxValue = null;
                    }
                    else if (parser.ExpectOptional("CYCLE"))
                    {
                        sequence.Cycle = false;
                    }
                    else
                    {
                        throw new TeamworkParserException("CannotParseStringUnsupportedCommand");
                    }
                }
                else
                {
                    throw new TeamworkParserException("CannotParseStringUnsupportedCommand");
                }
            }
        }
    }
}