// <copyright file="PgDiffIndexes.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.IO;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
using APE.PostgreSQL.Teamwork.Model.Utils;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres
{
    /// <summary>
    /// Diffs indexes.
    /// </summary>
    public class PgDiffIndexes
    {
        /// <summary>
        /// Creates a new instance of <see cref="PgDiffIndexes"/>.
        /// </summary>
        private PgDiffIndexes()
        {
        }

        /// <summary>
        /// Outputs statements for creation of new indexes.
        /// </summary>
        public static void Create(StreamWriter writer, PgSchema? oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            foreach (var newTable in newSchema.Tables)
            {
                // Add new indexes
                if (oldSchema == null)
                {
                    foreach (var index in newTable.Indexes)
                    {
                        searchPathHelper.OutputSearchPath(writer);
                        writer.WriteLine();
                        writer.WriteLine(index.CreationSQL);
                    }
                }
                else
                {
                    foreach (var index in GetNewIndexes(oldSchema.GetTable(newTable.Name), newTable))
                    {
                        searchPathHelper.OutputSearchPath(writer);
                        writer.WriteLine();
                        writer.WriteLine(index.CreationSQL);
                    }
                }
            }
        }

        /// <summary>
        /// Outputs statements for dropping indexes that exist no more.
        /// </summary>
        public static void Drop(StreamWriter writer, PgSchema? oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            foreach (var newTable in newSchema.Tables)
            {
                PgTable? oldTable = null;
                if (oldSchema != null)
                {
                    oldTable = oldSchema.GetTable(newTable.Name);
                }

                // Drop indexes that do not exist in new schema or are modified
                foreach (var index in GetDropIndexes(oldTable, newTable))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.WriteLine(index.DropSQL);
                }
            }
        }

        /// <summary>
        /// Outputs statements for index comments that have changed.
        /// </summary>
        public static void AlterComments(StreamWriter writer, PgSchema? oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            if (oldSchema == null)
            {
                return;
            }

            foreach (var oldIndex in oldSchema.Indexes)
            {
                var newIndex = newSchema.GetIndex(oldIndex.Name);

                if (newIndex == null)
                {
                    continue;
                }

                if ((oldIndex.Comment == null && newIndex.Comment != null)
                    || (oldIndex.Comment != null
                    && newIndex.Comment != null
                    && !oldIndex.Comment.Equals(newIndex.Comment)))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.Write("COMMENT ON INDEX ");
                    writer.Write(PgDiffStringExtension.QuoteName(newIndex.Name));
                    writer.Write(" IS ");
                    writer.Write(newIndex.Comment);
                    writer.WriteLine(';');
                }
                else if (oldIndex.Comment != null && newIndex.Comment == null)
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.Write("COMMENT ON INDEX ");
                    writer.Write(PgDiffStringExtension.QuoteName(newIndex.Name));
                    writer.WriteLine(" IS NULL;");
                }
            }
        }

        /// <summary>
        /// Returns list of indexes that should be dropped.
        /// </summary>
        private static IList<PgIndex> GetDropIndexes(PgTable? oldTable, PgTable? newTable)
        {
            // todo db Teamwork Indexes that are depending on a removed field should not be dropped
            // because they are already removed.
            IList<PgIndex> list = new List<PgIndex>();

            if (newTable != null && oldTable != null)
            {
                foreach (var index in oldTable.Indexes)
                {
                    var newTableIndex = newTable.GetIndex(index.Name);
                    if (newTableIndex is null || !newTableIndex.Equals(index))
                        list.Add(index);
                }
            }

            return list;
        }

        /// <summary>
        /// Returns list of indexes that should be added.
        /// </summary>
        private static IList<PgIndex> GetNewIndexes(PgTable? oldTable, PgTable? newTable)
        {
            IList<PgIndex> indexes = new List<PgIndex>();

            if (newTable == null)
            {
                return indexes;
            }

            if (oldTable == null)
            {
                foreach (var index in newTable.Indexes)
                {
                    indexes.Add(index);
                }
            }
            else
            {
                foreach (var index in newTable.Indexes)
                {
                    var tableIndex = oldTable.GetIndex(index.Name);
                    if (tableIndex is null || !tableIndex.Equals(index))
                        indexes.Add(index);
                }
            }

            return indexes;
        }
    }
}