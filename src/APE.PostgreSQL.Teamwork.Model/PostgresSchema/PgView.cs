// <copyright file="PgView.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using APE.PostgreSQL.Teamwork.Model.Utils;

namespace APE.PostgreSQL.Teamwork.Model.PostgresSchema
{
    /// <summary>
    /// Stores view information.
    /// </summary>
    public class PgView
    {
        /// <summary>
        /// List of optional column default values.
        /// </summary>
        private readonly IList<DefaultValue> defaultValues = new List<DefaultValue>(0);

        /// <summary>
        /// List of optional column comments.
        /// </summary>
        private readonly IList<ColumnComment> columnComments = new List<ColumnComment>(0);

        /// <summary>
        /// Creates a new instance of the <see cref="PgView"/>.
        /// </summary>
        /// <param name="name">The name of this <see cref="PgView"/>.</param>
        public PgView(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets a <see cref="List{String}"/> of all column names.
        /// </summary>
        public List<string> ColumnNames { get; set; }

        /// <summary>
        /// Gets or sets the comment of the <see cref="PgView"/>.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Creates and returns SQL for creation of the view.
        /// </summary>
        /// <returns> created SQL statement </returns>
        public string CreationSQL
        {
            get
            {
                var creationSql = new StringBuilder(this.Query.Length * 2);
                creationSql.Append("CREATE VIEW ");
                creationSql.Append(PgDiffStringExtension.QuoteName(this.Name));

                if (this.ColumnNames != null && this.ColumnNames.Count > 0)
                {
                    creationSql.Append(" (");

                    for (var i = 0; i < this.ColumnNames.Count; i++)
                    {
                        if (i > 0)
                        {
                            creationSql.Append(", ");
                        }

                        creationSql.Append(PgDiffStringExtension.QuoteName(this.ColumnNames[i]));
                    }

                    creationSql.Append(')');
                }

                creationSql.Append(" AS\n\t");
                creationSql.Append(this.Query);
                creationSql.Append(';');

                foreach (DefaultValue defaultValue in this.defaultValues)
                {
                    creationSql.Append("\n\nALTER VIEW ");
                    creationSql.Append(PgDiffStringExtension.QuoteName(this.Name));
                    creationSql.Append(" ALTER COLUMN ");
                    creationSql.Append(PgDiffStringExtension.QuoteName(defaultValue.ColumnName));
                    creationSql.Append(" SET DEFAULT ");
                    creationSql.Append(defaultValue.Value);
                    creationSql.Append(';');
                }

                if (this.Comment != null && this.Comment.Length > 0)
                {
                    creationSql.Append("\n\nCOMMENT ON VIEW ");
                    creationSql.Append(PgDiffStringExtension.QuoteName(this.Name));
                    creationSql.Append(" IS ");
                    creationSql.Append(this.Comment);
                    creationSql.Append(';');
                }

                foreach (ColumnComment columnComment in this.columnComments)
                {
                    if (columnComment.Comment != null && columnComment.Comment != string.Empty)
                    {
                        creationSql.Append("\n\nCOMMENT ON COLUMN ");
                        creationSql.Append(PgDiffStringExtension.QuoteName(columnComment.ColumnName));
                        creationSql.Append(" IS ");
                        creationSql.Append(columnComment.Comment);
                        creationSql.Append(';');
                    }
                }

                return creationSql.ToString();
            }
        }

        /// <summary>
        /// Creates and returns SQL statement for dropping the view.
        /// </summary>
        /// <returns> created SQL statement </returns>
        public string DropSQL
        {
            get
            {
                return "DROP VIEW " + PgDiffStringExtension.QuoteName(this.Name) + ";";
            }
        }

        /// <summary>
        /// Gets the name of this <see cref="PgView"/>.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the query for this <see cref="PgView"/>.
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Gets a <see cref="List{DefaultValue}"/> for this <see cref="PgView"/>.
        /// </summary>
        public IList<DefaultValue> DefaultValues
        {
            get
            {
                return new ReadOnlyCollection<DefaultValue>(this.defaultValues);
            }
        }

        /// <summary>
        /// Gets a <see cref="IList{ColumnComment}"/> for all columns of this <see cref="PgView"/>.
        /// </summary>
        public IList<ColumnComment> ColumnComments
        {
            get
            {
                return new ReadOnlyCollection<ColumnComment>(this.columnComments);
            }
        }

        /// <summary>
        /// Adds/replaces column default value specification.
        /// </summary>
        public void AddColumnDefaultValue(string columnName, string defaultValue)
        {
            this.RemoveColumnDefaultValue(columnName);
            this.defaultValues.Add(new DefaultValue(columnName, defaultValue));
        }

        /// <summary>
        /// Removes column default value if present.
        /// </summary>
        public void RemoveColumnDefaultValue(string columnName)
        {
            foreach (DefaultValue item in this.defaultValues)
            {
                if (item.ColumnName.Equals(columnName))
                {
                    this.defaultValues.Remove(item);
                    return;
                }
            }
        }

        /// <summary>
        /// Adds/replaces column comment.
        /// </summary>
        public void AddColumnComment(string columnName, string comment)
        {
            this.RemoveColumnDefaultValue(columnName);
            this.columnComments.Add(new ColumnComment(columnName, comment));
        }

        /// <summary>
        /// Removes column comment if present.
        /// </summary>
        public void RemoveColumnComment(string columnName)
        {
            foreach (ColumnComment item in this.columnComments)
            {
                if (item.ColumnName.Equals(columnName))
                {
                    this.columnComments.Remove(item);
                    return;
                }
            }
        }

        /// <summary>
        /// Contains information about default value of column.
        /// </summary>
        public class DefaultValue
        {
            /// <summary>
            /// Creates new instance of DefaultValue.
            /// </summary>
            internal DefaultValue(string columnName, string defaultValue)
            {
                this.ColumnName = columnName;
                this.Value = defaultValue;
            }

            /// <summary>
            /// Gets the column name.
            /// </summary>
            public string ColumnName { get; private set; }

            /// <summary>
            /// Gets the current value.
            /// </summary>
            public string Value { get; private set; }
        }

        /// <summary>
        /// Contains information about column comment.
        /// </summary>
        public class ColumnComment
        {
            /// <summary>
            /// Creates new instance of ColumnComment.
            /// </summary>
            internal ColumnComment(string columnName, string comment)
            {
                this.ColumnName = columnName;
                this.Comment = comment;
            }

            /// <summary>
            /// Gets the column name.
            /// </summary>
            public string ColumnName { get; private set; }

            /// <summary>
            /// Gets the comment.
            /// </summary>
            public string Comment { get; private set; }
        }
    }
}