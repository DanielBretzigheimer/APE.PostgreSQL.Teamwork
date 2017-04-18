// <copyright file="AlterSequenceParser.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres.Parsers
{
    /// <summary>
    /// Parses ALTER SEQUENCE statements.
    /// </summary>
    public class AlterSequenceParser
    {
        /// <summary>
        /// Creates new instance of AlterSequenceParser.
        /// </summary>
        private AlterSequenceParser()
        {
        }

        /// <summary>
        /// Parses ALTER SEQUENCE statement.
        /// </summary>
        public static void Parse(PgDatabase database, string statement)
        {
            var parser = new Parser(statement);

            parser.Expect("ALTER", "SEQUENCE");

            var sequenceName = parser.ParseIdentifier();

            var schemaName = ParserUtils.GetSchemaName(sequenceName, database);

            PgSchema schema = database.GetSchema(schemaName);

            if (schema == null)
            {
                throw new TeamworkParserException($"CannotFindSchema {schemaName} from statement {statement}");
            }

            var objectName = ParserUtils.GetObjectName(sequenceName);

            PgSequence sequence = schema.GetSequence(objectName);

            if (sequence == null)
            {
                throw new TeamworkParserException($"CannotFindSequence {sequenceName} from statement {statement}");
            }

            while (!parser.ExpectOptional(";"))
            {
                if (parser.ExpectOptional("OWNED", "BY"))
                {
                    if (parser.ExpectOptional("NONE"))
                    {
                        sequence.Owner = null;
                    }
                    else
                    {
                        sequence.Owner = parser.Expression();
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