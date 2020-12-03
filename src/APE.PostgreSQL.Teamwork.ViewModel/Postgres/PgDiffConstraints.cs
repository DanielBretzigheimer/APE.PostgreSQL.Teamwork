// <copyright file="PgDiffConstraints.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            foreach (var newTable in newSchema.Tables)
            {
                PgTable oldTable = null;
                if (oldSchema != null)
                    oldTable = oldSchema.GetTable(newTable.Name);

                var oldConstraints = new List<PgConstraint>();
                if (oldTable != null)
                    oldConstraints.AddRange(oldTable.Constraints);

                // Add new constraints
                foreach (var newConstraint in GetNewConstraints(oldTable, newTable, primaryKey, foreignKey))
                {
                    if (oldTable != null)
                    {
                        var oldConstraint = oldConstraints.FirstOrDefault(c => c.IsRenamed(newConstraint));
                        if (oldConstraint != null)
                        {
                            // constraint was renamed
                            searchPathHelper.OutputSearchPath(writer);
                            writer.WriteLine();
                            writer.WriteLine(oldConstraint.RenameToSql(newConstraint.Name));
                            oldConstraints.Remove(oldConstraint);
                            continue;
                        }
                    }

                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.WriteLine(newConstraint.CreationSQL);
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
            foreach (var newTable in newSchema.Tables)
            {
                PgTable oldTable = null;
                if (oldSchema != null)
                    oldTable = oldSchema.GetTable(newTable.Name);

                // Drop constraints that no more exist or are modified
                foreach (var constraint in GetDropConstraints(oldTable, newTable, primaryKey))
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
        /// Returns a list of all constraints which were renamed.
        /// </summary>
        /// <param name="oldTable">Original table or null.</param>
        /// <param name="newTable">New table or null.</param>
        /// <returns>A <see cref="Dictionary{PgConstraint, String}"/> which contains the original <see cref="PgConstraint"/> and the new <see cref="PgConstraint.Name"/>.</returns>
        private static Dictionary<PgConstraint, string> GetRenameConstraints([NullGuard.AllowNull] PgTable oldTable, [NullGuard.AllowNull] PgTable newTable)
        {
            var retval = new Dictionary<PgConstraint, string>();

            if (newTable != null && oldTable != null)
            {
                var oldConstraints = new List<PgConstraint>(oldTable.Constraints);
                foreach (var newConstraint in newTable.Constraints)
                {
                    // if there are multiple constraints rename only the first
                    var oldConstraint = oldConstraints.FirstOrDefault(c => c.IsRenamed(newConstraint));
                    if (oldConstraint != null)
                    {
                        retval.Add(oldConstraint, oldConstraint.Name);
                        oldConstraints.Remove(oldConstraint);
                    }
                }
            }

            return retval;
        }

        /// <summary>
        /// Returns list of constraints that should be dropped.
        /// </summary>
        /// <param name="oldTable">Original table or null.</param>
        /// <param name="newTable">New table or null.</param>
        /// <param name="primaryKey">Determines whether primary keys should be processed or any other constraints should be processed.</param>
        /// <returns>List of constraints that should be dropped.</returns>
        private static List<PgConstraint> GetDropConstraints([NullGuard.AllowNull] PgTable oldTable, [NullGuard.AllowNull] PgTable newTable, bool primaryKey)
        {
            // todo db Constraints that are depending on a removed field should not be added to drop because they are already removed.
            var constraints = new List<PgConstraint>();
            var renamedConstraints = GetRenameConstraints(oldTable, newTable);

            if (newTable != null && oldTable != null)
            {
                foreach (var oldConstraint in oldTable.Constraints)
                {
                    // we don't have to drop this constraint because its renamed
                    if (renamedConstraints.ContainsKey(oldConstraint))
                        continue;

                    if (oldConstraint.PrimaryKeyConstraint == primaryKey
                        && (!newTable.ContainsConstraint(oldConstraint.Name) || !newTable.GetConstraint(oldConstraint.Name).Equals(oldConstraint)))
                    {
                        constraints.Add(oldConstraint);
                    }
                }
            }

            return constraints;
        }

        /// <summary>
        /// Returns list of constraints that should be added.
        /// </summary>
        private static List<PgConstraint> GetNewConstraints([NullGuard.AllowNull] PgTable oldTable, [NullGuard.AllowNull] PgTable newTable, bool primaryKey, bool foreignKey)
        {
            var constraints = new List<PgConstraint>();
            var renamedConstraints = GetRenameConstraints(oldTable, newTable);

            // no new constraints
            if (newTable == null)
                return constraints;

            if (oldTable == null)
            {
                // all constraints are new
                foreach (var newConstraint in newTable.Constraints)
                {
                    if (newConstraint.PrimaryKeyConstraint == primaryKey && newConstraint.ForeignKeyConstraint == foreignKey)
                        constraints.Add(newConstraint);
                }
            }
            else
            {
                foreach (var newConstraint in newTable.Constraints)
                {
                    if ((newConstraint.PrimaryKeyConstraint == primaryKey && newConstraint.ForeignKeyConstraint == foreignKey)
                        && (!oldTable.ContainsConstraint(newConstraint.Name) || !oldTable.GetConstraint(newConstraint.Name).Equals(newConstraint)))
                    {
                        constraints.Add(newConstraint);
                    }
                }
            }

            return constraints;
        }
    }
}