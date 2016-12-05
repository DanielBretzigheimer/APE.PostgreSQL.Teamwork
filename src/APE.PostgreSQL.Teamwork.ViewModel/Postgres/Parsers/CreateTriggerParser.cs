// <copyright file="CreateTriggerParser.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres.Parsers
{
    /// <summary>
    /// Parses CREATE TRIGGER statements.
    /// </summary>
    public class CreateTriggerParser
    {
        /// <summary>
        /// Creates a new CreateTableParser object.
        /// </summary>
        private CreateTriggerParser()
        {
        }

        /// <summary>
        /// Parses CREATE TRIGGER statement.
        /// </summary>
        public static void Parse(PgDatabase database, string statement, bool ignoreSlonyTriggers)
        {
            Parser parser = new Parser(statement);
            parser.Expect("CREATE", "TRIGGER");

            string triggerName = parser.ParseIdentifier();

            string objectName = ParserUtils.GetObjectName(triggerName);

            PgTrigger trigger = new PgTrigger();
            trigger.Name = objectName;

            if (parser.ExpectOptional("BEFORE"))
                trigger.Before = true;
            else if (parser.ExpectOptional("AFTER"))
                trigger.Before = false;

            bool first = true;

            while (true)
            {
                if (!first && !parser.ExpectOptional("OR"))
                    break;
                else if (parser.ExpectOptional("INSERT"))
                    trigger.OnInsert = true;
                else if (parser.ExpectOptional("UPDATE"))
                {
                    trigger.OnUpdate = true;

                    if (parser.ExpectOptional("OF"))
                    {
                        do
                        {
                            trigger.AddUpdateColumn(parser.ParseIdentifier());
                        }
                        while (parser.ExpectOptional(","));
                    }
                }
                else if (parser.ExpectOptional("DELETE"))
                    trigger.OnDelete = true;
                else if (parser.ExpectOptional("TRUNCATE"))
                    trigger.OnTruncate = true;
                else if (first)
                    break;
                else
                {
                    parser.ThrowUnsupportedCommand();
                }

                first = false;
            }

            parser.Expect("ON");

            string tableName = parser.ParseIdentifier();

            trigger.TableName = ParserUtils.GetObjectName(tableName);

            if (parser.ExpectOptional("FOR"))
            {
                parser.ExpectOptional("EACH");

                if (parser.ExpectOptional("ROW"))
                    trigger.ForEachRow = true;
                else if (parser.ExpectOptional("STATEMENT"))
                    trigger.ForEachRow = false;
                else
                {
                    parser.ThrowUnsupportedCommand();
                }
            }

            if (parser.ExpectOptional("WHEN"))
            {
                parser.Expect("(");
                trigger.When = parser.Expression;
                parser.Expect(")");
            }

            parser.Expect("EXECUTE", "PROCEDURE");
            trigger.Function = parser.Rest;

            bool ignoreSlonyTrigger = ignoreSlonyTriggers && ("_slony_logtrigger".Equals(trigger.Name) || "_slony_denyaccess".Equals(trigger.Name));

            if (!ignoreSlonyTrigger)
            {
                PgSchema tableSchema = database.GetSchema(ParserUtils.GetSchemaName(tableName, database));
                tableSchema.GetTable(trigger.TableName).AddTrigger(trigger);
            }
        }
    }
}