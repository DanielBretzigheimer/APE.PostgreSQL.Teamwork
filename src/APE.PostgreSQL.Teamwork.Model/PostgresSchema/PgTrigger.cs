// <copyright file="PgTrigger.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Collections.ObjectModel;
using System.Text;
using APE.PostgreSQL.Teamwork.Model.Utils;

namespace APE.PostgreSQL.Teamwork.Model.PostgresSchema
{
    /// <summary>
    /// Stores trigger information.
    /// </summary>
    public class PgTrigger
    {
        /// <summary>
        /// Optional list of columns for UPDATE event.
        /// </summary>
        private readonly IList<string> updateColumns = new List<string>();

        public PgTrigger(string name)
            => this.Name = name;

        /// <summary>
        /// Gets or sets a boolean indicating whether the trigger should be fired BEFORE or AFTER action. Default is
        /// before.
        /// </summary>
        public bool Before { get; set; }

        /// <summary>
        /// Gets or sets the comment of the <see cref="PgTrigger"/>.
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Creates and returns SQL for creation of trigger.
        /// </summary>
        /// <returns> created SQL. </returns>
        public string CreationSQL
        {
            get
            {
                var creationSql = new StringBuilder(100);
                creationSql.Append("CREATE TRIGGER ");
                creationSql.Append(PgDiffStringExtension.QuoteName(this.Name));
                creationSql.Append("\n\t");
                creationSql.Append(this.Before ? "BEFORE" : "AFTER");

                var firstEvent = true;

                if (this.OnInsert)
                {
                    creationSql.Append(" INSERT");
                    firstEvent = false;
                }

                if (this.OnUpdate)
                {
                    if (firstEvent)
                        firstEvent = false;
                    else
                        creationSql.Append(" OR");

                    creationSql.Append(" UPDATE");

                    if (this.updateColumns.Count > 0)
                    {
                        creationSql.Append(" OF");

                        var first = true;

                        foreach (var columnName in this.updateColumns)
                        {
                            if (first)
                                first = false;
                            else
                                creationSql.Append(',');

                            creationSql.Append(' ');
                            creationSql.Append(columnName);
                        }
                    }
                }

                if (this.OnDelete)
                {
                    if (!firstEvent)
                        creationSql.Append(" OR");

                    creationSql.Append(" DELETE");
                }

                if (this.OnTruncate)
                {
                    if (!firstEvent)
                        creationSql.Append(" OR");

                    creationSql.Append(" TRUNCATE");
                }

                creationSql.Append(" ON ");
                creationSql.Append(PgDiffStringExtension.QuoteName(this.TableName));
                creationSql.Append("\n\tFOR EACH ");
                creationSql.Append(this.ForEachRow ? "ROW" : "STATEMENT");

                if (this.When != null && this.When.Length > 0)
                {
                    creationSql.Append("\n\tWHEN (");
                    creationSql.Append(this.When);
                    creationSql.Append(')');
                }

                creationSql.Append("\n\tEXECUTE PROCEDURE ");
                creationSql.Append(this.Function);
                creationSql.Append(';');

                if (this.Comment != null && this.Comment.Length > 0)
                {
                    creationSql.Append("\n\nCOMMENT ON TRIGGER ");
                    creationSql.Append(PgDiffStringExtension.QuoteName(this.Name));
                    creationSql.Append(" ON ");
                    creationSql.Append(PgDiffStringExtension.QuoteName(this.TableName));
                    creationSql.Append(" IS ");
                    creationSql.Append(this.Comment);
                    creationSql.Append(';');
                }

                return creationSql.ToString();
            }
        }

        /// <summary>
        /// Creates and returns SQL for dropping the trigger.
        /// </summary>
        /// <returns> created SQL. </returns>
        public string DropSQL
            => $"DROP TRIGGER {PgDiffStringExtension.QuoteName(this.Name)} ON {PgDiffStringExtension.QuoteName(this.TableName)};";

        /// <summary>
        /// Gets or sets a boolean indicating whether the trigger should be fired FOR EACH ROW or FOR EACH STATEMENT.
        /// Default is FOR EACH STATEMENT.
        /// </summary>
        public bool ForEachRow { get; set; }

        /// <summary>
        /// Gets or sets a the function that should be fired on the <see cref="PgTrigger"/>.
        /// </summary>
        public string? Function { get; set; }

        /// <summary>
        /// Gets or sets the name of the <see cref="PgTrigger"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a boolean indicating whether the trigger should be fired on DELETE.
        /// </summary>
        public bool OnDelete { get; set; }

        /// <summary>
        /// Gets or sets a boolean indicating whether the trigger should be fired on INSERT.
        /// </summary>
        public bool OnInsert { get; set; }

        /// <summary>
        /// Gets or sets a boolean indicating whether the trigger should be fired on UPDATE.
        /// </summary>
        public bool OnUpdate { get; set; }

        /// <summary>
        /// Gets or sets a boolean indicating whether the trigger should be fired on TRUNCATE.
        /// </summary>
        public bool OnTruncate { get; set; }

        /// <summary>
        /// Gets or sets the name of the table to which this <see cref="PgTrigger"/> belongs.
        /// </summary>
        public string TableName { get; set; } = string.Empty;

        /// <summary>
        /// Gets a list of all columns which should be updated.
        /// </summary>
        public IList<string> UpdateColumns => new ReadOnlyCollection<string>(this.updateColumns);

        /// <summary>
        /// Gets or sets a string indicating when the <see cref="PgTrigger"/> should be fired.
        /// </summary>
        public string? When { get; set; }

        /// <summary>
        /// Adds column name to the list of update columns.
        /// </summary>
        public void AddUpdateColumn(string columnName) => this.updateColumns.Add(columnName);

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        public override bool Equals(object? obj)
        {
            var equals = false;

            if (this == obj)
            {
                equals = true;
            }
            else if (obj is PgTrigger trigger)
            {
                equals = (this.Before == trigger.Before)
                    && (this.ForEachRow == trigger.ForEachRow)
                    && (this.Function?.Equals(trigger.Function) ?? trigger.Function is null)
                    && this.Name.Equals(trigger.Name)
                    && (this.OnDelete == trigger.OnDelete)
                    && (this.OnInsert == trigger.OnInsert)
                    && (this.OnUpdate == trigger.OnUpdate)
                    && (this.OnTruncate == trigger.OnTruncate)
                    && this.TableName.Equals(trigger.TableName);

                if (equals)
                {
                    var sorted1 = new List<string>(this.updateColumns);
                    var sorted2 = new List<string>(trigger.UpdateColumns);
                    sorted1.Sort();
                    sorted2.Sort();

                    equals = sorted1.Equals(sorted2);
                }
            }

            return equals;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => (this.GetType().FullName + "|" + this.Before + "|" + this.ForEachRow + "|" + this.Function + "|" + this.Name + "|" + this.OnDelete + "|" + this.OnInsert + "|" + this.OnUpdate + "|" + this.OnTruncate + "|" + this.TableName).GetHashCode();
    }
}