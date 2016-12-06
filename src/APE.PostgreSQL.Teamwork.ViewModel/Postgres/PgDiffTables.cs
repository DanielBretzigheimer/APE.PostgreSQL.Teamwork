// <copyright file="pgdifftables.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
using APE.PostgreSQL.Teamwork.Model.Utils;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres
{
    /// <summary>
    /// Diffs tables.
    /// </summary>
    public class PgDiffTables
    {
        /// <summary>
        /// Creates a new instance of <see cref="PgDiffTables"/>.
        /// </summary>
        private PgDiffTables()
        {
        }

        /// <summary>
        /// Outputs statements for creation of clusters.
        /// </summary>
        public static void DropClusters(StreamWriter writer, PgSchema oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            foreach (PgTable newTable in newSchema.Tables)
            {
                PgTable oldTable;

                if (oldSchema == null)
                    oldTable = null;
                else
                    oldTable = oldSchema.GetTable(newTable.Name);

                string oldCluster;

                if (oldTable == null)
                    oldCluster = null;
                else
                    oldCluster = oldTable.ClusterIndexName;

                string newCluster = newTable.ClusterIndexName;

                if (oldCluster != null && newCluster == null && newTable.ContainsIndex(oldCluster))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.Write("ALTER TABLE ");
                    writer.Write(PgDiffStringExtension.QuoteName(newTable.Name));
                    writer.WriteLine(" SET WITHOUT CLUSTER;");
                }
            }
        }

        /// <summary>
        /// Outputs statements for dropping of clusters.
        /// </summary>
        public static void CreateClusters(StreamWriter writer, PgSchema oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
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

                string oldCluster;

                if (oldTable == null)
                    oldCluster = null;
                else
                {
                    oldCluster = oldTable.ClusterIndexName;
                }

                string newCluster = newTable.ClusterIndexName;

                if ((oldCluster == null && newCluster != null) || (oldCluster != null && newCluster != null && newCluster.CompareTo(oldCluster) != 0))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.Write("ALTER TABLE ");
                    writer.Write(PgDiffStringExtension.QuoteName(newTable.Name));
                    writer.Write(" CLUSTER ON ");
                    writer.Write(PgDiffStringExtension.QuoteName(newCluster));
                    writer.WriteLine(';');
                }
            }
        }

        /// <summary>
        /// Outputs statements for altering tables.
        /// </summary>
        public static void Alter(StreamWriter writer, PgSchema oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            foreach (PgTable newTable in newSchema.Tables)
            {
                if (oldSchema == null || !oldSchema.ContainsTable(newTable.Name))
                    continue;

                PgTable oldTable = oldSchema.GetTable(newTable.Name);
                UpdateTableColumns(writer, oldTable, newTable, searchPathHelper);
                CheckWithOIDS(writer, oldTable, newTable, searchPathHelper);
                CheckInherits(writer, oldTable, newTable, searchPathHelper);
                CheckTablespace(writer, oldTable, newTable, searchPathHelper);
                AddAlterStatistics(writer, oldTable, newTable, searchPathHelper);
                AddAlterStorage(writer, oldTable, newTable, searchPathHelper);
                AlterComments(writer, oldTable, newTable, searchPathHelper);
            }
        }

        /// <summary>
        /// Outputs statements for creation of new tables.
        /// </summary>
        public static void Create(StreamWriter writer, PgSchema oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            foreach (PgTable table in newSchema.Tables)
            {
                if (oldSchema == null || !oldSchema.ContainsTable(table.Name))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.WriteLine(table.CreationSQL);
                }
            }
        }

        /// <summary>
        /// Outputs statements for dropping tables.
        /// </summary>
        public static void Drop(StreamWriter writer, PgSchema oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            if (oldSchema == null)
                return;

            Dictionary<string, List<PgTable>> referencedTables = new Dictionary<string, List<PgTable>>();

            List<PgTable> dropedTables = new List<PgTable>();
            foreach (PgTable table in oldSchema.Tables)
            {
                if (!newSchema.ContainsTable(table.Name))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    dropedTables.Add(table);

                    foreach (var constraint in table.Constraints)
                    {
                        Regex regex = new Regex(@"FOREIGN KEY.+REFERENCES (?<ReferencedTable>.+)\(.+\)");
                        MatchCollection matchCollection = regex.Matches(constraint.Definition);
                        if (matchCollection.Count > 0)
                        {
                            string referencedTable = matchCollection[0].Groups["ReferencedTable"].Value;

                            if (referencedTables.ContainsKey(referencedTable))
                            {
                                List<PgTable> tables;
                                if (referencedTables.TryGetValue(referencedTable, out tables))
                                    tables.Add(table);
                            }
                            else
                                referencedTables.Add(referencedTable, new List<PgTable>() { table });
                        }
                    }
                }
            }

            List<PgTable> dropableTables = null;
            while (dropableTables == null)
            {
                dropableTables = SortTablesByReferences(referencedTables, dropedTables);
            }

            foreach (PgTable table in dropableTables)
            {
                writer.WriteLine();
                writer.WriteLine(table.DropSQL);
            }
        }

        /// <summary>
        /// Generate the needed alter table xxx set statistics when needed.
        /// </summary>
        private static void AddAlterStatistics(StreamWriter writer, PgTable oldTable, PgTable newTable, SearchPathHelper searchPathHelper)
        {
            Dictionary<string, int?> stats = new Dictionary<string, int?>();

            foreach (PgColumn newColumn in newTable.Columns)
            {
                PgColumn oldColumn = oldTable.GetColumn(newColumn.Name);

                if (oldColumn != null)
                {
                    int? oldStat = oldColumn.Statistics;

                    int? newStat = newColumn.Statistics;
                    int? newStatValue = null;

                    if (newStat != null && (oldStat == null || !newStat.Equals(oldStat)))
                        newStatValue = newStat;
                    else if (oldStat != null && newStat == null)
                        newStatValue = Convert.ToInt32(-1);

                    if (newStatValue != null)
                        stats[newColumn.Name] = newStatValue;
                }
            }

            foreach (KeyValuePair<string, int?> entry in stats)
            {
                searchPathHelper.OutputSearchPath(writer);
                writer.WriteLine();
                writer.Write("ALTER TABLE ONLY ");
                writer.Write(PgDiffStringExtension.QuoteName(newTable.Name));
                writer.Write(" ALTER COLUMN ");
                writer.Write(PgDiffStringExtension.QuoteName(entry.Key));
                writer.Write(" SET STATISTICS ");
                writer.Write(entry.Value);
                writer.WriteLine(';');
            }
        }

        /// <summary>
        /// Generate the needed alter table xxx set storage when needed.
        /// </summary>
        private static void AddAlterStorage(StreamWriter writer, PgTable oldTable, PgTable newTable, SearchPathHelper searchPathHelper)
        {
            foreach (PgColumn newColumn in newTable.Columns)
            {
                PgColumn oldColumn = oldTable.GetColumn(newColumn.Name);

                string oldStorage = (oldColumn == null || oldColumn.Storage == null || oldColumn.Storage == string.Empty) ? null : oldColumn.Storage;

                string newStorage = (newColumn.Storage == null || newColumn.Storage == string.Empty) ? null : newColumn.Storage;

                if (newStorage == null && oldStorage != null)
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.WriteLine(string.Format("WarningUnableToDetermineStorageType", newTable.Name + '.' + newColumn.Name));

                    continue;
                }

                if (newStorage == null || newStorage.Equals(oldStorage, StringComparison.CurrentCultureIgnoreCase))
                    continue;

                searchPathHelper.OutputSearchPath(writer);
                writer.WriteLine();
                writer.Write("ALTER TABLE ONLY ");
                writer.Write(PgDiffStringExtension.QuoteName(newTable.Name));
                writer.Write(" ALTER COLUMN ");
                writer.Write(PgDiffStringExtension.QuoteName(newColumn.Name));
                writer.Write(" SET STORAGE ");
                writer.Write(newStorage);
                writer.Write(';');
            }
        }

        /// <summary>
        /// Adds statements for creation of new columns to the list of statements.
        /// </summary>
        private static void AddCreateTableColumns(IList<string> statements, PgTable oldTable, PgTable newTable, IList<PgColumn> dropDefaultsColumns, bool addDefaults)
        {
            foreach (PgColumn column in newTable.Columns)
            {
                if (!oldTable.ContainsColumn(column.Name))
                {
                    statements.Add("\tADD COLUMN " + column.GetFullDefinition(addDefaults));

                    if (addDefaults && !column.NullValue && (column.DefaultValue == null || column.DefaultValue == string.Empty))
                        dropDefaultsColumns.Add(column);
                }
            }
        }

        /// <summary>
        /// Adds statements for removal of columns to the list of statements.
        /// </summary>
        private static void AddDropTableColumns(IList<string> statements, PgTable oldTable, PgTable newTable)
        {
            foreach (PgColumn column in oldTable.Columns)
            {
                if (!newTable.ContainsColumn(column.Name))
                    statements.Add("\tDROP COLUMN " + PgDiffStringExtension.QuoteName(column.Name));
            }
        }

        /// <summary>
        /// Adds statements for modification of columns to the list of statements.
        /// </summary>
        private static void AddModifyTableColumns(IList<string> statements, PgTable oldTable, PgTable newTable, IList<PgColumn> dropDefaultsColumns, bool addDefaults)
        {
            foreach (PgColumn newColumn in newTable.Columns)
            {
                if (!oldTable.ContainsColumn(newColumn.Name))
                    continue;

                PgColumn oldColumn = oldTable.GetColumn(newColumn.Name);

                string newColumnName = PgDiffStringExtension.QuoteName(newColumn.Name);

                if (!oldColumn.Type.Equals(newColumn.Type))
                    statements.Add("\tALTER COLUMN " + newColumnName + " TYPE " + newColumn.Type + " /* " + string.Format("TypeParameterChange", newTable.Name, oldColumn.Type, newColumn.Type) + " */");

                string oldDefault = (oldColumn.DefaultValue == null) ? string.Empty : oldColumn.DefaultValue;
                string newDefault = (newColumn.DefaultValue == null) ? string.Empty : newColumn.DefaultValue;

                if (!oldDefault.Equals(newDefault))
                {
                    if (newDefault.Length == 0)
                        statements.Add("\tALTER COLUMN " + newColumnName + " DROP DEFAULT");
                    else
                    {
                        statements.Add("\tALTER COLUMN " + newColumnName + " SET DEFAULT " + newDefault);
                    }
                }

                if (oldColumn.NullValue != newColumn.NullValue)
                {
                    if (newColumn.NullValue)
                        statements.Add("\tALTER COLUMN " + newColumnName + " DROP NOT NULL");
                    else
                    {
                        if (addDefaults)
                        {
                            string defaultValue = PgColumnUtils.GetDefaultValue(newColumn.Type);

                            if (defaultValue != null)
                            {
                                statements.Add("\tALTER COLUMN " + newColumnName + " SET DEFAULT " + defaultValue);
                                dropDefaultsColumns.Add(newColumn);
                            }
                        }

                        statements.Add("\tALTER COLUMN " + newColumnName + " SET NOT NULL");
                    }
                }
            }
        }

        /// <summary>
        /// Checks whether there is a discrepancy in INHERITS for original and new
        /// table.
        /// </summary>
        private static void CheckInherits(StreamWriter writer, PgTable oldTable, PgTable newTable, SearchPathHelper searchPathHelper)
        {
            foreach (string tableName in oldTable.Inherits)
            {
                if (!newTable.Inherits.Contains(tableName))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.WriteLine("ALTER TABLE " + PgDiffStringExtension.QuoteName(newTable.Name));
                    writer.WriteLine("\tNO INHERIT " + PgDiffStringExtension.QuoteName(tableName) + ';');
                }
            }

            foreach (string tableName in newTable.Inherits)
            {
                if (!oldTable.Inherits.Contains(tableName))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.WriteLine("ALTER TABLE " + PgDiffStringExtension.QuoteName(newTable.Name));
                    writer.WriteLine("\tINHERIT " + PgDiffStringExtension.QuoteName(tableName) + ';');
                }
            }
        }

        /// <summary>
        /// Checks whether OIDS are dropped from the new table. There is no way to
        /// add OIDS to existing table so we do not create SQL statement for addition
        /// of OIDS but we issue warning.
        /// </summary>
        private static void CheckWithOIDS(StreamWriter writer, PgTable oldTable, PgTable newTable, SearchPathHelper searchPathHelper)
        {
            if (oldTable.With == null && newTable.With == null || oldTable.With != null && oldTable.With.Equals(newTable.With))
                return;

            searchPathHelper.OutputSearchPath(writer);
            writer.WriteLine();
            writer.WriteLine("ALTER TABLE " + PgDiffStringExtension.QuoteName(newTable.Name));

            if (newTable.With == null || "OIDS=false".Equals(newTable.With, StringComparison.CurrentCultureIgnoreCase))
                writer.WriteLine("\tSET WITHOUT OIDS;");
            else if ("OIDS".Equals(newTable.With, StringComparison.CurrentCultureIgnoreCase) || "OIDS=true".Equals(newTable.With, StringComparison.CurrentCultureIgnoreCase))
                writer.WriteLine("\tSET WITH OIDS;");
            else
            {
                writer.WriteLine("\tSET " + newTable.With + ";");
            }
        }

        /// <summary>
        /// Checks tablespace modification.
        /// </summary>
        private static void CheckTablespace(StreamWriter writer, PgTable oldTable, PgTable newTable, SearchPathHelper searchPathHelper)
        {
            if (oldTable.Tablespace == null && newTable.Tablespace == null
                || oldTable.Tablespace != null
                && oldTable.Tablespace.Equals(newTable.Tablespace))
                return;

            searchPathHelper.OutputSearchPath(writer);
            writer.WriteLine();
            writer.WriteLine("ALTER TABLE " + PgDiffStringExtension.QuoteName(newTable.Name));
            writer.WriteLine("\tTABLESPACE " + newTable.Tablespace + ';');
        }

        private static List<PgTable> SortTablesByReferences(Dictionary<string, List<PgTable>> referencedTables, List<PgTable> dropedTables)
        {
            for (int i = 0; i < dropedTables.Count; i++)
            {
                List<PgTable> tables = new List<PgTable>();
                if (referencedTables.TryGetValue("\"" + dropedTables[i].Name + "\"", out tables))
                {
                    foreach (PgTable table in tables)
                    {
                        if (dropedTables.IndexOf(table) > i)
                        {
                            dropedTables.Remove(table);
                            dropedTables.Insert(i, table);
                            return null;
                        }
                    }
                }
            }

            return dropedTables;
        }

        /// <summary>
        /// Outputs statements for addition, removal and modifications of table
        /// columns.
        /// </summary>
        private static void UpdateTableColumns(StreamWriter writer, PgTable oldTable, PgTable newTable, SearchPathHelper searchPathHelper)
        {
            IList<string> statements = new List<string>();
            IList<PgColumn> dropDefaultsColumns = new List<PgColumn>();
            AddDropTableColumns(statements, oldTable, newTable);
            AddCreateTableColumns(statements, oldTable, newTable, dropDefaultsColumns, false);
            AddModifyTableColumns(statements, oldTable, newTable, dropDefaultsColumns, false);

            if (statements.Count > 0)
            {
                string quotedTableName = PgDiffStringExtension.QuoteName(newTable.Name);
                searchPathHelper.OutputSearchPath(writer);
                writer.WriteLine();
                writer.WriteLine("ALTER TABLE " + quotedTableName);

                for (int i = 0; i < statements.Count; i++)
                {
                    writer.Write(statements[i]);
                    writer.WriteLine((i + 1) < statements.Count ? "," : ";");
                }

                if (dropDefaultsColumns.Count > 0)
                {
                    writer.WriteLine();
                    writer.WriteLine("ALTER TABLE " + quotedTableName);

                    for (int i = 0; i < dropDefaultsColumns.Count; i++)
                    {
                        writer.Write("\tALTER COLUMN ");
                        writer.Write(PgDiffStringExtension.QuoteName(dropDefaultsColumns[i].Name));
                        writer.Write(" DROP DEFAULT");
                        writer.WriteLine((i + 1) < dropDefaultsColumns.Count ? "," : ";");
                    }
                }
            }
        }

        /// <summary>
        /// Outputs statements for tables and columns for which comments have
        /// changed.
        /// </summary>
        private static void AlterComments(StreamWriter writer, PgTable oldTable, PgTable newTable, SearchPathHelper searchPathHelper)
        {
            if (oldTable.Comment == null && newTable.Comment != null
                || oldTable.Comment != null
                && newTable.Comment != null
                && !oldTable.Comment.Equals(newTable.Comment))
            {
                searchPathHelper.OutputSearchPath(writer);
                writer.WriteLine();
                writer.Write("COMMENT ON TABLE ");
                writer.Write(PgDiffStringExtension.QuoteName(newTable.Name));
                writer.Write(" IS ");
                writer.Write(newTable.Comment);
                writer.WriteLine(';');
            }
            else if (oldTable.Comment != null && newTable.Comment == null)
            {
                searchPathHelper.OutputSearchPath(writer);
                writer.WriteLine();
                writer.Write("COMMENT ON TABLE ");
                writer.Write(PgDiffStringExtension.QuoteName(newTable.Name));
                writer.WriteLine(" IS NULL;");
            }

            foreach (PgColumn newColumn in newTable.Columns)
            {
                PgColumn oldColumn = oldTable.GetColumn(newColumn.Name);

                string oldComment = oldColumn == null ? null : oldColumn.Comment;

                string newComment = newColumn.Comment;

                if (newComment != null && (oldComment == null ? newComment != null : !oldComment.Equals(newComment)))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.Write("COMMENT ON COLUMN ");
                    writer.Write(PgDiffStringExtension.QuoteName(newTable.Name));
                    writer.Write('.');
                    writer.Write(PgDiffStringExtension.QuoteName(newColumn.Name));
                    writer.Write(" IS ");
                    writer.Write(newColumn.Comment);
                    writer.WriteLine(';');
                }
                else if (oldComment != null && newComment == null)
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.Write("COMMENT ON COLUMN ");
                    writer.Write(PgDiffStringExtension.QuoteName(newTable.Name));
                    writer.Write('.');
                    writer.Write(PgDiffStringExtension.QuoteName(newColumn.Name));
                    writer.WriteLine(" IS NULL;");
                }
            }
        }
    }
}