// <copyright file="differencecreator.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.IO;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
using APE.PostgreSQL.Teamwork.Model.Templates;
using APE.PostgreSQL.Teamwork.Model.Utils;
using APE.PostgreSQL.Teamwork.ViewModel.Postgres.Loader;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres
{
    /// <summary>
    /// Creates a new SQL File which contains all difference between the two
    /// dump files.
    /// </summary>
    public class DifferenceCreator : IDifferenceCreator
    {
        /// <summary>
        /// Creates a diff of the database between the two dump files and writes it to the stream writer.
        /// </summary>
        /// <returns>A bool indicating differences where found.</returns>
        public bool Create(string filePath, string databaseName, string oldDumpFile, string newDumpFile)
        {
            PgDatabase oldDatabase = PgDumpLoader.LoadDatabaseSchema(oldDumpFile, databaseName, false, false);
            PgDatabase newDatabase = PgDumpLoader.LoadDatabaseSchema(newDumpFile, databaseName, false, false);

            bool created = false;

            // write diff file
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // mark the file as deleteable if no changes where made
                created = this.DiffDatabaseSchemas(writer, oldDatabase, newDatabase, false);
                writer.Close();
            }

            return created;
        }

        /// <summary>
        /// Creates a diff of the given databases and writes it to the stream writer.
        /// </summary>
        private bool DiffDatabaseSchemas(StreamWriter writer, PgDatabase oldDatabase, PgDatabase newDatabase, bool outputIgnoredStatements)
        {
            writer.Flush();
            long startLength = writer.BaseStream.Length;

            if (oldDatabase.Comment == null && newDatabase.Comment != null || oldDatabase.Comment != null && newDatabase.Comment != null && !oldDatabase.Comment.Equals(newDatabase.Comment))
            {
                writer.WriteLine();
                writer.Write("COMMENT ON DATABASE " + newDatabase.Name + " IS ");
                writer.Write(newDatabase.Comment);
                writer.WriteLine(';');
            }
            else if (oldDatabase.Comment != null && newDatabase.Comment == null)
            {
                writer.WriteLine();
                writer.WriteLine("COMMENT ON DATABASE " + newDatabase.Name + " IS NULL;");
            }

            this.DropOldSchemas(writer, oldDatabase, newDatabase);
            this.CreateNewSchemas(writer, oldDatabase, newDatabase);
            this.UpdateSchemas(writer, oldDatabase, newDatabase);

            if (outputIgnoredStatements)
            {
                if (oldDatabase.IgnoredStatements.Count != 0)
                {
                    writer.WriteLine();
                    writer.Write("/* ");
                    writer.WriteLine("OriginalDatabaseIgnoredStatements");

                    foreach (string statement in oldDatabase.IgnoredStatements)
                    {
                        writer.WriteLine();
                        writer.WriteLine(statement);
                    }

                    writer.WriteLine("*/");
                }

                if (newDatabase.IgnoredStatements.Count != 0)
                {
                    writer.WriteLine();
                    writer.Write("/* ");
                    writer.WriteLine("NewDatabaseIgnoredStatements");

                    foreach (string statement in newDatabase.IgnoredStatements)
                    {
                        writer.WriteLine();
                        writer.WriteLine(statement);
                    }

                    writer.WriteLine("*/");
                }
            }

            writer.Flush();

            // check if new content was written
            if (writer.BaseStream.Length > startLength)
                return true;
            else
                return false;
        }

        private void CreateNewSchemas(StreamWriter writer, PgDatabase oldDatabase, PgDatabase newDatabase)
        {
            foreach (PgSchema newSchema in newDatabase.Schemas)
            {
                // ignore the teamwork schema, because it is automatically created
                if (newSchema.Name != SQLTemplates.PostgreSQLTeamworkSchemaName && oldDatabase.GetSchema(newSchema.Name) == null)
                {
                    writer.WriteLine();
                    writer.WriteLine(newSchema.CreationSQL);
                }
            }
        }

        private void DropOldSchemas(StreamWriter writer, PgDatabase oldDatabase, PgDatabase newDatabase)
        {
            foreach (PgSchema oldSchema in oldDatabase.Schemas)
                if (newDatabase.GetSchema(oldSchema.Name) == null)
                {
                    // ignore the teamwork schema, because it is needed to check the version
                    if (oldSchema.Name == SQLTemplates.PostgreSQLTeamworkSchemaName)
                        continue;

                    writer.WriteLine();
                    writer.WriteLine("DROP SCHEMA IF EXISTS " + oldSchema.Name.QuoteName() + " CASCADE;");
                }
        }

        private void UpdateSchemas(StreamWriter writer, PgDatabase oldDatabase, PgDatabase newDatabase)
        {
            bool setSearchPath = newDatabase.Schemas.Count > 1 || !newDatabase.Schemas[0].Name.Equals("public");

            foreach (PgSchema newSchema in newDatabase.Schemas)
            {
                // ignore the teamwork schema, because it is automatically created 
                if (newSchema.Name == SQLTemplates.PostgreSQLTeamworkSchemaName)
                    continue;

                SearchPathHelper searchPathHelper;

                if (setSearchPath)
                    searchPathHelper = new SearchPathHelper("SET search_path = " + newSchema.Name.GetQuotedName(true) + ", pg_catalog;");
                else
                    searchPathHelper = new SearchPathHelper(null);

                PgSchema oldSchema = oldDatabase.GetSchema(newSchema.Name);

                if (oldSchema != null)
                {
                    if (oldSchema.Comment == null && newSchema.Comment != null || oldSchema.Comment != null && newSchema.Comment != null && !oldSchema.Comment.Equals(newSchema.Comment))
                    {
                        writer.WriteLine();
                        writer.Write("COMMENT ON SCHEMA ");
                        writer.Write(newSchema.Name.QuoteName());
                        writer.Write(" IS ");
                        writer.Write(newSchema.Comment);
                        writer.WriteLine(';');
                    }
                    else if (oldSchema.Comment != null && newSchema.Comment == null)
                    {
                        writer.WriteLine();
                        writer.Write("COMMENT ON SCHEMA ");
                        writer.Write(newSchema.Name.QuoteName());
                        writer.WriteLine(" IS NULL;");
                    }
                }

                // drop
                PgDiffTriggers.DropTriggers(writer, oldSchema, newSchema, searchPathHelper);
                PgDiffAggregate.DropAggregates(writer, oldSchema, newSchema, searchPathHelper);
                PgDiffFunctions.DropFunctions(writer, oldSchema, newSchema, searchPathHelper);
                PgDiffViews.DropViews(writer, oldSchema, newSchema, searchPathHelper);
                PgDiffConstraints.DropConstraints(writer, oldSchema, newSchema, true, searchPathHelper);
                PgDiffConstraints.DropConstraints(writer, oldSchema, newSchema, false, searchPathHelper);
                PgDiffIndexes.DropIndexes(writer, oldSchema, newSchema, searchPathHelper);
                PgDiffTables.DropClusters(writer, oldSchema, newSchema, searchPathHelper);
                PgDiffTables.DropTables(writer, oldSchema, newSchema, searchPathHelper);

                // create and alter
                PgDiffTypes.CreateTypes(writer, oldSchema, newSchema, searchPathHelper);
                PgDiffSequences.CreateSequences(writer, oldSchema, newSchema, searchPathHelper);
                PgDiffSequences.AlterSequences(writer, oldSchema, newSchema, searchPathHelper, false);
                PgDiffTables.CreateTables(writer, oldSchema, newSchema, searchPathHelper);
                PgDiffTables.AlterTables(writer, oldSchema, newSchema, searchPathHelper);
                PgDiffSequences.AlterCreatedSequences(writer, oldSchema, newSchema, searchPathHelper);
                PgDiffFunctions.CreateFunctions(writer, oldSchema, newSchema, searchPathHelper, false);
                PgDiffAggregate.CreateAggregates(writer, oldSchema, newSchema, searchPathHelper);
                PgDiffConstraints.CreateConstraints(writer, oldSchema, newSchema, true, false, searchPathHelper);
                PgDiffConstraints.CreateConstraints(writer, oldSchema, newSchema, false, false, searchPathHelper);
                PgDiffConstraints.CreateConstraints(writer, oldSchema, newSchema, false, true, searchPathHelper);
                PgDiffIndexes.CreateIndexes(writer, oldSchema, newSchema, searchPathHelper);
                PgDiffTables.CreateClusters(writer, oldSchema, newSchema, searchPathHelper);
                PgDiffTriggers.CreateTriggers(writer, oldSchema, newSchema, searchPathHelper);
                PgDiffViews.CreateViews(writer, oldSchema, newSchema, searchPathHelper);
                PgDiffViews.AlterViews(writer, oldSchema, newSchema, searchPathHelper);
                PgDiffSequences.DropSequences(writer, oldSchema, newSchema, searchPathHelper);

                // drop type after table is altered
                PgDiffTypes.DropTypes(writer, oldSchema, newSchema, searchPathHelper);

                // alter comments
                PgDiffFunctions.AlterComments(writer, oldSchema, newSchema, searchPathHelper);
                PgDiffConstraints.AlterComments(writer, oldSchema, newSchema, searchPathHelper);
                PgDiffIndexes.AlterComments(writer, oldSchema, newSchema, searchPathHelper);
                PgDiffTriggers.AlterComments(writer, oldSchema, newSchema, searchPathHelper);
            }
        }
    }
}