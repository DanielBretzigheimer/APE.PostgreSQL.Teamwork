// <copyright file="SQLFileTester.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
using APE.PostgreSQL.Teamwork.Model.Utils;
using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;
using APE.PostgreSQL.Teamwork.ViewModel.Postgres.Loader;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    /// <summary>
    /// Encapsulates methods to test a given <see cref="SQLFile"/>.
    /// </summary>
    public class SQLFileTester : ISQLFileTester // todo db unittest
    {
        /// <summary>
        /// Tests the given <see cref="SQLFile"/> on the given <see cref="Database"/>.
        /// </summary>
        public void CreateData(Database database, SQLFile target)
        {
            if (database.CurrentVersion < target.Version)
            {
                throw new InvalidOperationException("The target file was not executed on the database");
            }

            // read file
            var postgresDatabase = PgDumpLoader.LoadDatabaseSchema(target.Path, database, false, false);

            // create data for tables
            foreach (var schema in postgresDatabase.Schemas)
            {
                var tables = new Dictionary<PgTable, bool>();
                foreach (var table in schema.Tables)
                {
                    tables.Add(table, false);
                }

                foreach (var table in schema.Tables)
                {
                    this.CreateData(database, target, schema, table, tables);
                }
            }
        }

        public void TestEmptyMethods(Database database, SQLFile target)
        {
            if (database.CurrentVersion < target.Version)
            {
                throw new InvalidOperationException("The target file was not executed on the database");
            }

            // read file
            var oldDatabase = PgDumpLoader.LoadDatabaseSchema(target.Path, database, false, false);

            // call empty functions
        }

        private void CreateData(Database database, ISQLFile target, PgSchema schema, PgTable table, Dictionary<PgTable, bool> executedTables)
        {
            foreach (var constraint in table.Constraints)
            {
                var references = "REFERENCES ";
                if (constraint.CreationSQL.Contains(references))
                {
                    var referenceStart = constraint.CreationSQL.IndexOf(references) + references.Length;
                    var referenceEnd = constraint.CreationSQL[referenceStart..].IndexOf("(");

                    var reference = constraint.CreationSQL.Substring(referenceStart, referenceEnd);

                    if (schema.Tables.Any(t => t.Name.QuoteName() == reference))
                    {
                        var constraintTable = schema.Tables.Single(t => t.Name.QuoteName() == reference);

                        // check if file was not already executed
                        if (executedTables[constraintTable] == false)
                        {
                            this.CreateData(database, target, schema, constraintTable, executedTables);
                        }
                    }
                }
            }

            var columns = string.Empty;
            var first = true;
            foreach (var column in table.Columns)
            {
                if (!first)
                {
                    columns += ", ";
                }

                if (column.NullValue)
                {
                    columns += "DEFAULT";
                }
                else if (column.DefaultValue != null)
                {
                    columns += column.DefaultValue;
                }
                else if (schema.Types.Any(t => t.Name.QuoteName() == column.Type))
                {
                    var type = schema.Types.Single(t => t.Name.QuoteName() == column.Type);
                    columns += type.EnumEntries.First();
                }
                else
                {
                    columns += this.GetRandomValue(column.Type, target);
                }

                first = false;
            }

            var insertSQL = $"INSERT INTO {table.Name.QuoteName()} VALUES ({columns});";
            database.ExecuteCommandNonQuery(insertSQL);
            executedTables[table] = true;
        }

        /// <summary>
        /// Gets a random value of the given postgresql type.
        /// </summary>
        /// <exception cref="TeamworkConnectionException">Is raised if the given type is not supported.</exception>
        /// <returns>The random value.</returns>
        private string GetRandomValue(string type, ISQLFile target)
        {
            switch (type)
            {
                case "bigint":
                case "integer":
                case "smallint":
                case "decimal":
                case "numeric":
                case "int":
                case "bit":
                case "varbit":
                case "bit varying":
                    return "1";
                case "bool":
                case "boolean":
                    return "true";
                case "double precision":
                case "real":
                case "money":
                    return "1.2";
                case "date":
                case "timestamp":
                case "timestamp without time zone":
                case "time":
                case "time without time zone":
                case "time with time zone":
                case "timestamp with time zone":
                    return "now()";
                case "char":
                case "character":
                    return "C";
                case "text":
                case "varchar":
                case "character varying":
                    return "testtext";
                default:
                    throw new TeamworkConnectionException(target, $"Could not create data for file {target.FileName} because the type {type} is not supported!");
            }
        }
    }
}
