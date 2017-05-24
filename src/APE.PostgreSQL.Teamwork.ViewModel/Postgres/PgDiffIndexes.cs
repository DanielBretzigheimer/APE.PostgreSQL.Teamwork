// <copyright file="PgDiffIndexes.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Collections.Generic;
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
        public static void Create(StreamWriter writer, [NullGuard.AllowNull] PgSchema oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            foreach (PgTable newTable in newSchema.Tables)
            {
                // Add new indexes
                if (oldSchema == null)
                {
                    foreach (PgIndex index in newTable.Indexes)
                    {
                        searchPathHelper.OutputSearchPath(writer);
                        writer.WriteLine();
                        writer.WriteLine(index.CreationSQL);
                    }
                }
                else
                {
                    foreach (PgIndex index in GetNewIndexes(oldSchema.GetTable(newTable.Name), newTable))
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
        public static void Drop(StreamWriter writer, [NullGuard.AllowNull] PgSchema oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            foreach (PgTable newTable in newSchema.Tables)
            {
                PgTable oldTable = null;
                if (oldSchema != null)
                {
                    oldTable = oldSchema.GetTable(newTable.Name);
                }

                // Drop indexes that do not exist in new schema or are modified
                foreach (PgIndex index in GetDropIndexes(oldTable, newTable))
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
        public static void AlterComments(StreamWriter writer, [NullGuard.AllowNull] PgSchema oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            if (oldSchema == null)
            {
                return;
            }

            foreach (PgIndex oldIndex in oldSchema.Indexes)
            {
                PgIndex newIndex = newSchema.GetIndex(oldIndex.Name);

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
        private static IList<PgIndex> GetDropIndexes([NullGuard.AllowNull] PgTable oldTable, [NullGuard.AllowNull] PgTable newTable)
        {
            // todo db Teamwork Indexes that are depending on a removed field should not be added
            // to drop because they are already removed.
            IList<PgIndex> list = new List<PgIndex>();

            if (newTable != null && oldTable != null)
            {
                foreach (PgIndex index in oldTable.Indexes)
                {
                    if (!newTable.ContainsIndex(index.Name) || !newTable.GetIndex(index.Name).Equals(index))
                    {
                        list.Add(index);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Returns list of indexes that should be added.
        /// </summary>
        private static IList<PgIndex> GetNewIndexes([NullGuard.AllowNull] PgTable oldTable, [NullGuard.AllowNull] PgTable newTable)
        {
            IList<PgIndex> indexes = new List<PgIndex>();

            if (newTable == null)
            {
                return indexes;
            }

            if (oldTable == null)
            {
                foreach (PgIndex index in newTable.Indexes)
                {
                    indexes.Add(index);
                }
            }
            else
            {
                foreach (PgIndex index in newTable.Indexes)
                {
                    if (!oldTable.ContainsIndex(index.Name) || !oldTable.GetIndex(index.Name).Equals(index))
                    {
                        indexes.Add(index);
                    }
                }
            }

            return indexes;
        }
    }
}