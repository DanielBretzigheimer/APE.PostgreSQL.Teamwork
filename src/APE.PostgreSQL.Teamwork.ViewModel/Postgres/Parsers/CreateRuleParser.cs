using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres.Parsers
{
    public class CreateRuleParser
    {
        public static void Parse(PgDatabase database, string statement)
        {
            var parser = new Parser(statement);
            parser.Expect("CREATE");
            parser.ExpectOptional("OR", "REPLACE");
            parser.Expect("RULE");

            var ruleIdentifier = parser.ParseIdentifier();
            var rule = new PgRule(ParserUtils.GetObjectName(ruleIdentifier));
            var schemaName = ParserUtils.GetSchemaName(ruleIdentifier, database);

            parser.Expect("AS", "ON");
            rule.EventType = parser.ExpectOptionalOneOf("SELECT", "INSERT", "UPDATE", "DELETE");
            parser.Expect("TO");
            var tableIdentifier = parser.ParseIdentifier();

            if (parser.ExpectOptional("WHERE"))
            {
                var endIndex = parser.String.IndexOf("DO");
                rule.Condition = parser.GetSubString(0, endIndex);
            }

            rule.TableName = ParserUtils.GetObjectName(tableIdentifier);

            parser.Expect("DO");
            rule.Do = parser.ExpectOptionalOneOf("ALSO", "INSTEAD");
            rule.Command = parser.Rest() + ";";

            PgSchema schema = database.GetSchema(schemaName);

            if (schema == null)
            {
                throw new Exception(string.Format("CannotFindSchema {0}. Statement {1}", schemaName, statement));
            }

            // check if the rule is a view
            if (rule.EventType == "SELECT" && rule.Do == "INSTEAD")
            {
                var table = schema.Tables.Single(t => t.Name == rule.TableName);
                schema.Tables.Remove(table);
                schema.Views.Add(new PgView(table.Name) { Comment = table.Comment, Query = rule.Command });
            }
            else
            {
                schema.Rules.Add(rule);
            }
        }
    }
}
