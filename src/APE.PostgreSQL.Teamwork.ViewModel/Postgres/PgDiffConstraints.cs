// <copyright file="PgDiffConstraints.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Collections.Generic;
using System.IO;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
using APE.PostgreSQL.Teamwork.Model.Utils;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres
{
    /// <summary>
    /// Diffs constraints.
    /// </summary>
    public class PgDiffConstraints
    {
        private PgDiffConstraints()
        {
        }

        /// <summary>
        /// Outputs statements for creation of new constraints.
        /// </summary>
        /// <param name="writer">Writer the output should be written to.</param>
        /// <param name="oldSchema">Original schema.</param>
        /// <param name="newSchema">New schema.</param>
        /// <param name="primaryKey">Determines whether primary keys should be processed or any other constraints should be processed.</param>
        /// <param name="searchPathHelper">Search path helper.</param>
        /// <param name="foreignKey">Determines wheter forein keys should be processed.</param>
        public static void Create(StreamWriter writer, [NullGuard.AllowNull] PgSchema oldSchema, PgSchema newSchema, bool primaryKey, bool foreignKey, SearchPathHelper searchPathHelper)
        {
            foreach (PgTable newTable in newSchema.Tables)
            {
                PgTable oldTable;

                if (oldSchema == null)
                {
                    oldTable = null;
                }
                else
                {
                    oldTable = oldSchema.GetTable(newTable.Name);
                }

                // Add new constraints
                foreach (PgConstraint constraint in GetNewConstraints(oldTable, newTable, primaryKey, foreignKey))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.WriteLine(constraint.CreationSQL);
                }
            }
        }

        /// <summary>
        /// Outputs statements for dropping non-existent or modified constraints.
        /// </summary>
        /// <param name="writer">Writer the output should be written to.</param>
        /// <param name="primaryKey">Determines whether primary keys should be processed or any other constraints should be processed.</param>
        /// <param name="searchPathHelper">Search path helper.</param>
        /// <param name="newSchema">The schema of the new database.</param>
        /// <param name="oldSchema">The schema of the old database.</param>
        public static void Drop(StreamWriter writer, [NullGuard.AllowNull] PgSchema oldSchema, PgSchema newSchema, bool primaryKey, SearchPathHelper searchPathHelper)
        {
            foreach (PgTable newTable in newSchema.Tables)
            {
                PgTable oldTable;

                if (oldSchema == null)
                {
                    oldTable = null;
                }
                else
                {
                    oldTable = oldSchema.GetTable(newTable.Name);
                }

                // Drop constraints that no more exist or are modified
                foreach (PgConstraint constraint in GetDropConstraints(oldTable, newTable, primaryKey))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.WriteLine(constraint.DropSQL);
                }
            }
        }

        /// <summary>
        /// Outputs statements for constraint comments that have changed.
        /// </summary>
        public static void AlterComments(StreamWriter writer, [NullGuard.AllowNull] PgSchema oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            if (oldSchema == null)
            {
                return;
            }

            foreach (PgTable oldTable in oldSchema.Tables)
            {
                PgTable newTable = newSchema.GetTable(oldTable.Name);

                if (newTable == null)
                {
                    continue;
                }

                foreach (PgConstraint oldConstraint in oldTable.Constraints)
                {
                    PgConstraint newConstraint = newTable.GetConstraint(oldConstraint.Name);

                    if (newConstraint == null)
                    {
                        continue;
                    }

                    if ((oldConstraint.Comment == null && newConstraint.Comment != null)
                        || (oldConstraint.Comment != null && newConstraint.Comment != null && !oldConstraint.Comment.Equals(newConstraint.Comment)))
                    {
                        searchPathHelper.OutputSearchPath(writer);
                        writer.WriteLine();
                        writer.Write("COMMENT ON ");

                        if (newConstraint.PrimaryKeyConstraint)
                        {
                            writer.Write("INDEX ");
                            writer.Write(PgDiffStringExtension.QuoteName(newConstraint.Name));
                        }
                        else
                        {
                            writer.Write("CONSTRAINT ");
                            writer.Write(PgDiffStringExtension.QuoteName(newConstraint.Name));
                            writer.Write(" ON ");
                            writer.Write(PgDiffStringExtension.QuoteName(newConstraint.TableName));
                        }

                        writer.Write(" IS ");
                        writer.Write(newConstraint.Comment);
                        writer.WriteLine(';');
                    }
                    else if (oldConstraint.Comment != null && newConstraint.Comment == null)
                    {
                        searchPathHelper.OutputSearchPath(writer);
                        writer.WriteLine();
                        writer.Write("COMMENT ON ");

                        if (newConstraint.PrimaryKeyConstraint)
                        {
                            writer.Write("INDEX ");
                            writer.Write(PgDiffStringExtension.QuoteName(newConstraint.Name));
                        }
                        else
                        {
                            writer.Write("CONSTRAINT ");
                            writer.Write(PgDiffStringExtension.QuoteName(newConstraint.Name));
                            writer.Write(" ON ");
                            writer.Write(PgDiffStringExtension.QuoteName(newConstraint.TableName));
                        }

                        writer.WriteLine(" IS NULL;");
                    }
                }
            }
        }

        /// <summary>
        /// Returns list of constraints that should be dropped.
        /// </summary>
        /// <param name="oldTable">Original table or null.</param>
        /// <param name="newTable">New table or null.</param>
        /// <param name="primaryKey">Determines whether primary keys should be processed or any other constraints should be processed.</param>
        /// <returns>List of constraints that should be dropped.</returns>
        private static IList<PgConstraint> GetDropConstraints([NullGuard.AllowNull] PgTable oldTable, [NullGuard.AllowNull] PgTable newTable, bool primaryKey)
        {
            // todo db Constraints that are depending on a removed field should not be added to drop because they are already removed.
            IList<PgConstraint> constraints = new List<PgConstraint>();

            if (newTable != null && oldTable != null)
            {
                foreach (PgConstraint constraint in oldTable.Constraints)
                {
                    if (constraint.PrimaryKeyConstraint == primaryKey && (!newTable.ContainsConstraint(constraint.Name) || !newTable.GetConstraint(constraint.Name).Equals(constraint)))
                    {
                        constraints.Add(constraint);
                    }
                }
            }

            return constraints;
        }

        /// <summary>
        /// Returns list of constraints that should be added.
        /// </summary>
        private static IList<PgConstraint> GetNewConstraints([NullGuard.AllowNull] PgTable oldTable, [NullGuard.AllowNull] PgTable newTable, bool primaryKey, bool foreignKey)
        {
            IList<PgConstraint> constraints = new List<PgConstraint>();

            if (newTable == null)
            {
                return constraints;
            }

            if (oldTable == null)
            {
                foreach (PgConstraint constraint in newTable.Constraints)
                {
                    if (constraint.PrimaryKeyConstraint == primaryKey && constraint.ForeignKeyConstraint == foreignKey)
                    {
                        constraints.Add(constraint);
                    }
                }
            }
            else
            {
                foreach (PgConstraint constraint in newTable.Constraints)
                {
                    if ((constraint.PrimaryKeyConstraint == primaryKey && constraint.ForeignKeyConstraint == foreignKey) && (!oldTable.ContainsConstraint(constraint.Name) || !oldTable.GetConstraint(constraint.Name).Equals(constraint)))
                    {
                        constraints.Add(constraint);
                    }
                }
            }

            return constraints;
        }
    }
}