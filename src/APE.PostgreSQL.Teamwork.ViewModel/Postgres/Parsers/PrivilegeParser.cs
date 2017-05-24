// <copyright file="PrivilegeParser.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema.Enums;
using log4net;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres.Parsers
{
    /// <summary>
    /// Parses privileges (GRANT or REVOKE).
    /// </summary>
    public class PrivilegeParser
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static List<PgPrivilegeKind> enumEntries = new List<PgPrivilegeKind>();

        static PrivilegeParser()
        {
            foreach (PgPrivilegeKind entry in Enum.GetValues(typeof(PgPrivilegeKind)))
            {
                enumEntries.Add(entry);
            }
        }

        public static void Parse(PgDatabase database, string statement, PgPrivilegeCommand command)
        {
            var parser = new Parser(statement);
            parser.Expect(command.ToString().ToUpper());
            var privilegeKind = parser.ExpectOptionalOneOf(enumEntries.Select(k => k.ToString().ToUpper()).ToArray());
            var kind = enumEntries.Single(k => k.ToString().ToUpper() == privilegeKind);
            parser.Expect("ON");

            var type = parser.ParseIdentifier().ToUpper();
            var name = parser.ParseIdentifier();
            var nameWithSchema = name;

            if (parser.ExpectOptional("("))
            {
                name += "(";
                while (!parser.ExpectOptional(")"))
                {
                    var argumentName = ParserUtils.GetObjectName(parser.ParseIdentifier());
                    var dataType = parser.ParseDataType();
                    name += $"{argumentName} {dataType}";

                    if (parser.ExpectOptional(")"))
                    {
                        break;
                    }
                    else
                    {
                        parser.Expect(",");
                        name += ", ";
                    }
                }

                name += ")";
            }

            switch (command)
            {
                case PgPrivilegeCommand.Grant:
                    parser.Expect("TO");
                    break;
                case PgPrivilegeCommand.Revoke:
                    parser.Expect("FROM");
                    break;
                default:
                    throw new ArgumentException($"The postgres privilege command {command} is unknown.");
            }

            var role = parser.ParseIdentifier();
            parser.Expect(";");

            var privilege = new PgPrivilege()
            {
                Command = command,
                Privilege = kind,
                OnType = type,
                OnName = name,
                Role = role,
            };

            var schemaName = ParserUtils.GetSchemaName(nameWithSchema, database);
            PgSchema schema = database.GetSchema(schemaName);

            if (schema == null)
            {
                throw new Exception($"Cannot find schema {schemaName} for statement {statement}.");
            }

            schema.AddPrivilege(privilege);
        }
    }
}
