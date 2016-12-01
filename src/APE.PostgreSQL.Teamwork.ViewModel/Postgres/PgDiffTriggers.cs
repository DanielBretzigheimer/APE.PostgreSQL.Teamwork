// <copyright file="PgDiffTriggers.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Collections.Generic;
using System.IO;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
using APE.PostgreSQL.Teamwork.Model.Utils;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres
{
    /// <summary>
    /// Diffs triggers.
    /// </summary>
    public class PgDiffTriggers
    {
        /// <summary>
        /// Creates a new instance of <see cref="PgDiffTriggers"/>.
        /// </summary>
        private PgDiffTriggers()
        {
        }

        /// <summary>
        /// Outputs statements for creation of new triggers.
        /// </summary>
        public static void CreateTriggers(StreamWriter writer, PgSchema oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            foreach (PgTable newTable in newSchema.Tables)
            {
                PgTable oldTable;

                if (oldSchema == null)
                    oldTable = null;
                else
                {
                    oldTable = oldSchema.GetTable(newTable.Name);
                }

                // Add new triggers
                foreach (PgTrigger trigger in GetNewTriggers(oldTable, newTable))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.WriteLine(trigger.CreationSQL);
                }
            }
        }

        /// <summary>
        /// Outputs statements for dropping triggers.
        /// </summary>
        public static void DropTriggers(StreamWriter writer, PgSchema oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            foreach (PgTable newTable in newSchema.Tables)
            {
                PgTable oldTable;

                if (oldSchema == null)
                    oldTable = null;
                else
                    oldTable = oldSchema.GetTable(newTable.Name);

                // Drop triggers that no more exist or are modified
                foreach (PgTrigger trigger in GetDropTriggers(oldTable, newTable))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.WriteLine(trigger.DropSQL);
                }
            }
        }

        /// <summary>
        /// Outputs statements for trigger comments that have changed.
        /// </summary>
        public static void AlterComments(StreamWriter writer, PgSchema oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            if (oldSchema == null)
                return;

            foreach (PgTable oldTable in oldSchema.Tables)
            {
                PgTable newTable = newSchema.GetTable(oldTable.Name);

                if (newTable == null)
                    continue;

                foreach (PgTrigger oldTrigger in oldTable.Triggers)
                {
                    PgTrigger newTrigger = newTable.GetTrigger(oldTrigger.Name);

                    if (newTrigger == null)
                        continue;

                    if (oldTrigger.Comment == null && newTrigger.Comment != null
                        || oldTrigger.Comment != null
                        && newTrigger.Comment != null
                        && !oldTrigger.Comment.Equals(newTrigger.Comment))
                    {
                        searchPathHelper.OutputSearchPath(writer);
                        writer.WriteLine();
                        writer.Write("COMMENT ON TRIGGER ");
                        writer.Write(PgDiffStringExtension.QuoteName(newTrigger.Name));
                        writer.Write(" ON ");
                        writer.Write(PgDiffStringExtension.QuoteName(newTrigger.TableName));
                        writer.Write(" IS ");
                        writer.Write(newTrigger.Comment);
                        writer.WriteLine(';');
                    }
                    else if (oldTrigger.Comment != null && newTrigger.Comment == null)
                    {
                        searchPathHelper.OutputSearchPath(writer);
                        writer.WriteLine();
                        writer.Write("COMMENT ON TRIGGER ");
                        writer.Write(PgDiffStringExtension.QuoteName(newTrigger.Name));
                        writer.Write(" ON ");
                        writer.Write(PgDiffStringExtension.QuoteName(newTrigger.TableName));
                        writer.WriteLine(" IS NULL;");
                    }
                }
            }
        }

        /// <summary>
        /// Returns list of triggers that should be dropped.
        /// </summary>
        private static IList<PgTrigger> GetDropTriggers(PgTable oldTable, PgTable newTable)
        {
            IList<PgTrigger> list = new List<PgTrigger>();

            if (newTable != null && oldTable != null)
            {
                IList<PgTrigger> newTriggers = newTable.Triggers;
                foreach (PgTrigger oldTrigger in oldTable.Triggers)
                    if (!newTriggers.Contains(oldTrigger))
                        list.Add(oldTrigger);
            }

            return list;
        }

        /// <summary>
        /// Returns list of triggers that should be added.
        /// </summary>
        private static IList<PgTrigger> GetNewTriggers(PgTable oldTable, PgTable newTable)
        {
            List<PgTrigger> list = new List<PgTrigger>();

            if (newTable != null)
            {
                if (oldTable == null)
                    list.AddRange(newTable.Triggers);
                else
                {
                    foreach (PgTrigger newTrigger in newTable.Triggers)
                        if (!oldTable.Triggers.Contains(newTrigger))
                            list.Add(newTrigger);
                }
            }

            return list;
        }
    }
}