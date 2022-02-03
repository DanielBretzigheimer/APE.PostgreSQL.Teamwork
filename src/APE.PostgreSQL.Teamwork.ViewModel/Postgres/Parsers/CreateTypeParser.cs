// <copyright file="CreateTypeParser.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Text;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres.Parsers
{
    /// <summary>
    /// Parser for a <see cref="PgType"/>.
    /// </summary>
    public class CreateTypeParser
    {
        /// <summary>
        /// Parses the given statement and adds the <see cref="PgType"/> to the <see cref="PgDatabase"/>.
        /// </summary>
        /// <param name="database">The <see cref="PgDatabase"/> to which the <see cref="PgType"/> is added.</param>
        /// <param name="statement">The SQL statement of the <see cref="PgType"/>.</param>
        public static void Parse(PgDatabase database, string statement)
        {
            var parser = new Parser(statement);
            parser.Expect("CREATE");
            parser.Expect("TYPE");

            var typeName = parser.ParseIdentifier();

            parser.Expect("AS");

            // check if type is enum
            if (parser.ExpectOptional("ENUM"))
            {
                var enumEntries = new List<string>();

                var columnsExist = parser.ExpectOptional("(");

                if (columnsExist)
                {
                    while (!parser.ExpectOptional(")"))
                    {
#pragma warning disable CS0618 // Type or member is obsolete
                        var entry = parser.ParseStringCompat();
#pragma warning restore CS0618 // Type or member is obsolete
                        enumEntries.Add(ParserUtils.GetObjectName(entry));
                        parser.ExpectOptional(",");
                    }
                }

                var type = new PgType(ParserUtils.GetObjectName(typeName), true)
                {
                    EnumEntries = enumEntries,
                };
                var schemaName = ParserUtils.GetSchemaName(typeName, database);

                if (database.SchemaIsIgnored(schemaName))
                    return;

                var schema = database.GetSchema(schemaName);
                if (schema == null)
                {
                    throw new Exception(string.Format("Cannot find schema {0}. Statement {1}", schemaName, statement));
                }

                schema.Add(type);
            }
            else
            {
                var type = new PgType(ParserUtils.GetObjectName(typeName), false);

                var arguments = new List<PgArgument>();

                // type is no enum
                parser.Expect("(");

                var attributeType = new StringBuilder();

                while (true)
                {
                    var name = parser.ParseIdentifier();
                    var datatype = parser.ParseDataType();

                    arguments.Add(new PgArgument(name, datatype));

                    parser.SkipWhitespace();
                    if (parser.ExpectOptional(")"))
                        break;
                    else
                        parser.Expect(",");
                }

                type.AttributeArguments = arguments;

                parser.Expect(";");

                var schemaName = ParserUtils.GetSchemaName(typeName, database);
                var schema = database.GetSchema(schemaName);
                if (schema == null)
                {
                    throw new Exception(string.Format("Cannot find schema {0}. Statement {1}", schemaName, statement));
                }

                schema.Add(type);
            }
        }
    }
}
