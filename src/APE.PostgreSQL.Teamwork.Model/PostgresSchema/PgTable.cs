// <copyright file="PgTable.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using APE.PostgreSQL.Teamwork.Model.Utils;

namespace APE.PostgreSQL.Teamwork.Model.PostgresSchema
{
    /// <summary>
    /// Stores table information.
    /// </summary>
    public class PgTable
    {
        /// <summary>
        /// List of columns defined on the table.
        /// </summary>
        private readonly IList<PgColumn> columns = new List<PgColumn>();

        /// <summary>
        /// List of constraints defined on the table.
        /// </summary>
        private readonly IList<PgConstraint> constraints = new List<PgConstraint>();

        /// <summary>
        /// List of indexes defined on the table.
        /// </summary>
        private readonly IList<PgIndex> indexes = new List<PgIndex>();

        /// <summary>
        /// List of triggers defined on the table.
        /// </summary>
        private readonly IList<PgTrigger> triggers = new List<PgTrigger>();

        /// <summary>
        /// List of names of inherited tables.
        /// </summary>
        private readonly IList<string> inherits = new List<string>();

        /// <summary>
        /// Creates a new <see cref="PgTable"/> object.
        /// </summary>
        /// <param name="name">The name of the <see cref="PgTable"/>.</param>
        public PgTable(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets the name of the index on which the table is clustered.
        /// </summary>
        [NullGuard.AllowNull]
        public string ClusterIndexName { get; set; }

        /// <summary>
        /// Getter for <seealso cref="columns"/>. The list cannot be modified.
        /// </summary>
        /// <returns> <seealso cref="columns"/> </returns>
        public IList<PgColumn> Columns
        {
            get
            {
                return new ReadOnlyCollection<PgColumn>(this.columns);
            }
        }

        /// <summary>
        /// Gets or sets the comment for the table.
        /// </summary>
        [NullGuard.AllowNull]
        public string Comment { get; set; }

        /// <summary>
        /// Gets a list of all <see cref="PgConstraint"/> for this <see cref="PgTable"/>.
        /// </summary>
        public IList<PgConstraint> Constraints
        {
            get
            {
                return new ReadOnlyCollection<PgConstraint>(this.constraints);
            }
        }

        /// <summary>
        /// Creates and returns SQL for creation of the table.
        /// </summary>
        /// <returns> created SQL statement. </returns>
        public string CreationSQL
        {
            get
            {
                var sql = new StringBuilder(1000);
                sql.Append("CREATE TABLE ");
                sql.Append(this.Name.QuoteName());
                sql.Append(" (\n");

                var first = true;

                if (this.columns.Count == 0)
                {
                    sql.Append(')');
                }
                else
                {
                    foreach (PgColumn column in this.columns)
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            sql.Append(",\n");
                        }

                        sql.Append("\t");
                        sql.Append(column.GetFullDefinition(false));
                    }

                    sql.Append("\n)");
                }

                if (this.inherits != null && this.inherits.Count > 0)
                {
                    sql.Append("\nINHERITS (");

                    first = true;

                    foreach (var tableName in this.inherits)
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            sql.Append(", ");
                        }

                        sql.Append(tableName);
                    }

                    sql.Append(")");
                }

                if (this.With != null && this.With.Length > 0)
                {
                    sql.Append("\n");

                    if ("OIDS=false".Equals(this.With, StringComparison.CurrentCultureIgnoreCase))
                    {
                        sql.Append("WITHOUT OIDS");
                    }
                    else
                    {
                        sql.Append("WITH ");

                        if ("OIDS".Equals(this.With, StringComparison.CurrentCultureIgnoreCase) || "OIDS=true".Equals(this.With, StringComparison.CurrentCultureIgnoreCase))
                        {
                            sql.Append("OIDS");
                        }
                        else
                        {
                            sql.Append(this.With);
                        }
                    }
                }

                if (this.Tablespace != null && this.Tablespace.Length > 0)
                {
                    sql.Append("\nTABLESPACE ");
                    sql.Append(this.Tablespace);
                }

                sql.Append(';');

                foreach (PgColumn column in this.ColumnsWithStatistics)
                {
                    sql.Append("\nALTER TABLE ONLY ");
                    sql.Append(this.Name.QuoteName());
                    sql.Append(" ALTER COLUMN ");
                    sql.Append(column.Name.QuoteName());
                    sql.Append(" SET STATISTICS ");
                    sql.Append(column.Statistics);
                    sql.Append(';');
                }

                if (this.Comment != null && this.Comment.Length > 0)
                {
                    sql.Append("\n\nCOMMENT ON TABLE ");
                    sql.Append(this.Name.QuoteName());
                    sql.Append(" IS ");
                    sql.Append(this.Comment);
                    sql.Append(';');
                }

                foreach (PgColumn column in this.columns)
                {
                    if (column.Comment != null && column.Comment != string.Empty)
                    {
                        sql.Append("\n\nCOMMENT ON COLUMN ");
                        sql.Append(this.Name.QuoteName());
                        sql.Append('.');
                        sql.Append(column.Name.QuoteName());
                        sql.Append(" IS ");
                        sql.Append(column.Comment);
                        sql.Append(';');
                    }
                }

                return sql.ToString();
            }
        }

        /// <summary>
        /// Creates and returns SQL statement for dropping the table.
        /// </summary>
        /// <returns> created SQL statement. </returns>
        public string DropSQL
        {
            get
            {
                return "DROP TABLE IF EXISTS " + this.Name.QuoteName() + ";";
            }
        }

        /// <summary>
        /// Getter for <seealso cref="indexes"/>. The list cannot be modified.
        /// </summary>
        /// <returns> <seealso cref="indexes"/> </returns>
        public IList<PgIndex> Indexes
        {
            get
            {
                return new ReadOnlyCollection<PgIndex>(this.indexes);
            }
        }

        /// <summary>
        /// Gets a list with all inherits from the <see cref="PgTable"/>.
        /// </summary>
        public IList<string> Inherits
        {
            get
            {
                return new ReadOnlyCollection<string>(this.inherits);
            }
        }

        /// <summary>
        /// Gets or sets the name of the <see cref="PgTable"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets a list of all <see cref="PgTrigger"/>s.
        /// </summary>
        public IList<PgTrigger> Triggers
        {
            get
            {
                return new ReadOnlyCollection<PgTrigger>(this.triggers);
            }
        }

        /// <summary>
        /// Gets or sets the WITH clause. If value is null then it is not set, otherwise can be set to
        /// OIDS=true, OIDS=false, or storage parameters can be set.
        /// </summary>
        [NullGuard.AllowNull]
        public string With { get; set; }

        /// <summary>
        /// Gets or sets the table space for this <see cref="PgTable"/>.
        /// </summary>
        [NullGuard.AllowNull]
        public string Tablespace { get; set; }

        /// <summary>
        /// Returns list of columns that have statistics defined.
        /// </summary>
        /// <returns> list of columns that have statistics defined. </returns>
        private IList<PgColumn> ColumnsWithStatistics
        {
            get
            {
                IList<PgColumn> list = new List<PgColumn>();
                foreach (PgColumn column in this.columns)
                {
                    if (column.Statistics != null)
                    {
                        list.Add(column);
                    }
                }

                return list;
            }
        }

        /// <summary>
        /// Finds column according to specified column name.
        /// </summary>
        /// <param name="name">Name of the column to be searched.</param>
        /// <returns>Found column or null if no such column has been found.</returns>
        [return: NullGuard.AllowNull]
        public PgColumn GetColumn(string name)
        {
            foreach (PgColumn column in this.columns)
            {
                if (column.Name.Equals(name))
                {
                    return column;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds index according to specified index name.
        /// </summary>
        /// <param name="name">Name of the index to be searched.</param>
        /// <returns>Found index or null if no such index has been found.</returns>
        public PgIndex GetIndex(string name)
        {
            foreach (PgIndex index in this.indexes)
            {
                if (index.Name.Equals(name))
                {
                    return index;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds trigger according to specified trigger name.
        /// </summary>
        /// <param name="name">Name of the trigger to be searched.</param>
        /// <returns>Found trigger or null if no such trigger has been found.</returns>
        [return: NullGuard.AllowNull]
        public PgTrigger GetTrigger(string name)
        {
            foreach (PgTrigger trigger in this.triggers)
            {
                if (trigger.Name.Equals(name))
                {
                    return trigger;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds constraint according to specified constraint name.
        /// </summary>
        /// <param name="name">Name of the constraint to be searched.</param>
        /// <returns>Found constraint or null if no such constraint has been found.</returns>
        [return: NullGuard.AllowNull]
        public PgConstraint GetConstraint(string name)
        {
            foreach (PgConstraint constraint in this.constraints)
            {
                if (constraint.Name.Equals(name))
                {
                    return constraint;
                }
            }

            return null;
        }

        /// <summary>
        /// Adds the given table name to the list of inherits.
        /// </summary>
        public void AddInherits(string tableName)
        {
            this.inherits.Add(tableName);
        }

        /// <summary>
        /// Adds a <see cref="PgColumn"/> to the list of columns.
        /// </summary>
        public void AddColumn(PgColumn column)
        {
            this.columns.Add(column);
        }

        /// <summary>
        /// Adds a <see cref="PgConstraint"/> to the list of constraints.
        /// </summary>
        public void AddConstraint(PgConstraint constraint)
        {
            this.constraints.Add(constraint);
        }

        /// <summary>
        /// Adds a <see cref="PgIndex"/> to the list of indexes.
        /// </summary>
        public void AddIndex(PgIndex index)
        {
            this.indexes.Add(index);
        }

        /// <summary>
        /// Adds a <see cref="PgTrigger"/> to the list of triggers.
        /// </summary>
        public void AddTrigger(PgTrigger trigger)
        {
            this.triggers.Add(trigger);
        }

        /// <summary>
        /// Returns true if table contains given column, otherwise false.
        /// </summary>
        /// <param name="name">Name of the column.</param>
        /// <returns>True if table contains given column, otherwise false.</returns>
        public bool ContainsColumn(string name)
        {
            foreach (PgColumn column in this.columns)
            {
                if (column.Name.Equals(name))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if table contains given constraint name, otherwise false.
        /// </summary>
        /// <param name="name">Name of the constraint.</param>
        /// <returns>True if table contains given constraint, otherwise false.</returns>
        public bool ContainsConstraint(string name)
        {
            foreach (PgConstraint constraint in this.constraints)
            {
                if (constraint.Name.Equals(name))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if table contains given index, otherwise false.
        /// </summary>
        /// <param name="name">Name of the index.</param>
        /// <returns>True if table contains given index, otherwise false.</returns>
        public bool ContainsIndex(string name)
        {
            foreach (PgIndex index in this.indexes)
            {
                if (index.Name.Equals(name))
                {
                    return true;
                }
            }

            return false;
        }
    }
}