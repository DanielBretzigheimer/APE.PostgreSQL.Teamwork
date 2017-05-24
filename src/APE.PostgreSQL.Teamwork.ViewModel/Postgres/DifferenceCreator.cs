// <copyright file="DifferenceCreator.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.IO;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
using APE.PostgreSQL.Teamwork.Model.Templates;
using APE.PostgreSQL.Teamwork.Model.Utils;
using APE.PostgreSQL.Teamwork.ViewModel.Postgres.Loader;
using System.Collections.Generic;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres
{
    /// <summary>
    /// Creates a new SQL File which contains all difference between the two
    /// dump files.
    /// </summary>
    public class DifferenceCreator : IDifferenceCreator
    {
        public const string WarningMessagePrefix = "-- ATTENTION: ";

        /// <summary>
        /// Creates a diff of the database between the two dump files and writes it to the stream writer.
        /// </summary>
        /// <returns>A bool indicating differences where found.</returns>
        public bool Create(string filePath, string databaseName, string oldDumpFile, string newDumpFile)
        {
            var result = false;
            using (var writer = new StreamWriter(filePath))
            {
                result = this.Create(writer, databaseName, oldDumpFile, newDumpFile);
            }

            return result;
        }

        /// <summary>
        /// Creates a diff of the database between the two dump files and writes it to the stream writer.
        /// </summary>
        /// <returns>A bool indicating differences where found.</returns>
        public bool Create(Stream stream, string databaseName, string oldDumpFile, string newDumpFile)
        {
            var result = false;
            using (var writer = new StreamWriter(stream))
            {
                result = this.Create(writer, databaseName, oldDumpFile, newDumpFile);
            }

            return result;
        }

        public bool Create(StreamWriter writer, string databaseName, string oldDumpFile, string newDumpFile)
        {
            PgDatabase oldDatabase = PgDumpLoader.LoadDatabaseSchema(oldDumpFile, databaseName, false, false);
            PgDatabase newDatabase = PgDumpLoader.LoadDatabaseSchema(newDumpFile, databaseName, false, false);

            // mark the file as deleteable if no changes where made
            var created = false;
            created = this.DiffDatabaseSchemas(writer, oldDatabase, newDatabase, false);
            writer.Close();
            return created;
        }

        /// <summary>
        /// Creates a diff of the given databases and writes it to the stream writer.
        /// </summary>
        private bool DiffDatabaseSchemas(StreamWriter writer, PgDatabase oldDatabase, PgDatabase newDatabase, bool outputIgnoredStatements)
        {
            writer.Flush();
            var startLength = writer.BaseStream.Length;

            if ((oldDatabase.Comment == null && newDatabase.Comment != null) ||
                (oldDatabase.Comment != null && newDatabase.Comment != null && !oldDatabase.Comment.Equals(newDatabase.Comment)))
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

                    foreach (var statement in oldDatabase.IgnoredStatements)
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

                    foreach (var statement in newDatabase.IgnoredStatements)
                    {
                        writer.WriteLine();
                        writer.WriteLine(statement);
                    }

                    writer.WriteLine("*/");
                }
            }

            writer.Flush();

            // check if new content was written
            return writer.BaseStream.Length > startLength;
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
            {
                if (newDatabase.GetSchema(oldSchema.Name) == null)
                {
                    // ignore the teamwork schema, because it is needed to check the version
                    if (oldSchema.Name == SQLTemplates.PostgreSQLTeamworkSchemaName)
                    {
                        continue;
                    }

                    writer.WriteLine();
                    writer.WriteLine("DROP SCHEMA IF EXISTS " + oldSchema.Name.QuoteName() + " CASCADE;");
                }
            }
        }

        private void UpdateSchemas(StreamWriter writer, PgDatabase oldDatabase, PgDatabase newDatabase)
        {
            Dictionary<PgSchema, PgSchema> newAndOldSchemas = new Dictionary<PgSchema, PgSchema>();

            foreach (var newSchema in newDatabase.Schemas)
            {
                // ignore the teamwork schema, because it is automatically created
                if (newSchema.Name == SQLTemplates.PostgreSQLTeamworkSchemaName)
                {
                    continue;
                }

                var searchPathHelper = new SearchPathHelper(newSchema);
                var oldSchema = oldDatabase.GetSchema(newSchema.Name);
                {
                }
                {
                }
                if (oldSchema != null)
                {
                    if ((oldSchema.Comment == null && newSchema.Comment != null)
                        || (oldSchema.Comment != null && newSchema.Comment != null && !oldSchema.Comment.Equals(newSchema.Comment)))
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

                newAndOldSchemas.Add(newSchema, oldSchema);
            }

            var schemaActions = new List<Action<StreamWriter, PgSchema, PgSchema>>()
            {
                (w, newSchema, oldSchema) => { PgDiffTriggers.Drop(w, oldSchema, newSchema, new SearchPathHelper(newSchema)); },
                (w, newSchema, oldSchema) => { PgDiffAggregate.Drop(w, oldSchema, newSchema, new SearchPathHelper(newSchema)); },
                (w, newSchema, oldSchema) => { PgDiffFunctions.Drop(w, oldSchema, newSchema, new SearchPathHelper(newSchema)); },
                (w, newSchema, oldSchema) => { PgDiffViews.Drop(w, oldSchema, newSchema, new SearchPathHelper(newSchema)); },
                (w, newSchema, oldSchema) => { PgDiffConstraints.Drop(w, oldSchema, newSchema, true, new SearchPathHelper(newSchema)); },
                (w, newSchema, oldSchema) => { PgDiffConstraints.Drop(w, oldSchema, newSchema, false, new SearchPathHelper(newSchema)); },
                (w, newSchema, oldSchema) => { PgDiffIndexes.Drop(w, oldSchema, newSchema, new SearchPathHelper(newSchema)); },
                (w, newSchema, oldSchema) => { PgDiffTables.DropClusters(w, oldSchema, newSchema, new SearchPathHelper(newSchema)); },
                (w, newSchema, oldSchema) => { PgDiffTables.Drop(w, oldSchema, newSchema, new SearchPathHelper(newSchema)); },
                (w, newSchema, oldSchema) => { PgDiffTypes.Create(w, oldSchema, newSchema, new SearchPathHelper(newSchema)); },
                (w, newSchema, oldSchema) => { PgDiffSequences.Create(w, oldSchema, newSchema, new SearchPathHelper(newSchema)); },
                (w, newSchema, oldSchema) => { PgDiffSequences.Alter(w, oldSchema, newSchema, new SearchPathHelper(newSchema), false); },
                (w, newSchema, oldSchema) => { PgDiffTables.Create(w, oldSchema, newSchema, new SearchPathHelper(newSchema)); },
                (w, newSchema, oldSchema) => { PgDiffTables.Alter(w, oldSchema, newSchema, new SearchPathHelper(newSchema)); },
                (w, newSchema, oldSchema) => { PgDiffSequences.AlterCreatedSequences(w, oldSchema, newSchema, new SearchPathHelper(newSchema)); },
                (w, newSchema, oldSchema) => { PgDiffFunctions.Create(w, oldSchema, newSchema, new SearchPathHelper(newSchema), false); },
                (w, newSchema, oldSchema) => { PgDiffAggregate.Create(w, oldSchema, newSchema, new SearchPathHelper(newSchema)); },
                (w, newSchema, oldSchema) => { PgDiffConstraints.Create(w, oldSchema, newSchema, true, false, new SearchPathHelper(newSchema)); },
                (w, newSchema, oldSchema) => { PgDiffConstraints.Create(w, oldSchema, newSchema, false, false, new SearchPathHelper(newSchema)); },
                (w, newSchema, oldSchema) => { PgDiffConstraints.Create(w, oldSchema, newSchema, false, true, new SearchPathHelper(newSchema)); },
                (w, newSchema, oldSchema) => { PgDiffIndexes.Create(w, oldSchema, newSchema, new SearchPathHelper(newSchema)); },
                (w, newSchema, oldSchema) => { PgDiffTables.CreateClusters(w, oldSchema, newSchema, new SearchPathHelper(newSchema)); },
                (w, newSchema, oldSchema) => { PgDiffTriggers.Create(w, oldSchema, newSchema, new SearchPathHelper(newSchema)); },
                (w, newSchema, oldSchema) => { PgDiffViews.Create(w, oldSchema, newSchema, new SearchPathHelper(newSchema)); },
                (w, newSchema, oldSchema) => { PgDiffViews.Alter(w, oldSchema, newSchema, new SearchPathHelper(newSchema)); },
                (w, newSchema, oldSchema) => { PgDiffSequences.Drop(w, oldSchema, newSchema, new SearchPathHelper(newSchema)); },
                (w, newSchema, oldSchema) => { PgDiffPrivileges.Create(w, oldSchema, newSchema, new SearchPathHelper(newSchema)); },

                // drop type after table is altered
                (w, newSchema, oldSchema) => { PgDiffTypes.Drop(w, oldSchema, newSchema, new SearchPathHelper(newSchema)); },

                (w, newSchema, oldSchema) => { PgDiffFunctions.AlterComments(w, oldSchema, newSchema, new SearchPathHelper(newSchema)); },
                (w, newSchema, oldSchema) => { PgDiffConstraints.AlterComments(w, oldSchema, newSchema, new SearchPathHelper(newSchema)); },
                (w, newSchema, oldSchema) => { PgDiffIndexes.AlterComments(w, oldSchema, newSchema, new SearchPathHelper(newSchema)); },
                (w, newSchema, oldSchema) => { PgDiffTriggers.AlterComments(w, oldSchema, newSchema, new SearchPathHelper(newSchema)); },
            };

            foreach (var action in schemaActions)
            {
                foreach (var newAndOldSchema in newAndOldSchemas)
                {
                    action(writer, newAndOldSchema.Key, newAndOldSchema.Value);
                }
            }
        }
    }
}