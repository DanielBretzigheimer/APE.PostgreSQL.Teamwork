// <copyright file="PgDiffViews.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.IO;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
using APE.PostgreSQL.Teamwork.Model.Utils;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres
{
    /// <summary>
    /// Diffs views.
    /// </summary>
    public class PgDiffViews
    {
        /// <summary>
        /// Creates a new instance of <see cref="PgDiffViews"/>.
        /// </summary>
        private PgDiffViews()
        {
        }

        /// <summary>
        /// Outputs statements for creation of views.
        /// </summary>
        public static void Create(StreamWriter writer, PgSchema? oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            foreach (var newView in newSchema.Views)
            {
                if (oldSchema == null || !oldSchema.TryGetView(newView.Name, out var oldView) || IsViewModified(oldView, newView))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.WriteLine(newView.CreationSQL);
                }
            }
        }

        /// <summary>
        /// Outputs statements for dropping views.
        /// </summary>
        public static void Drop(StreamWriter writer, PgSchema? oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            if (oldSchema == null)
            {
                return;
            }

            foreach (var oldView in oldSchema.Views)
            {
                var newView = newSchema.GetView(oldView.Name);

                if (newView == null || IsViewModified(oldView, newView))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.WriteLine(oldView.DropSQL);
                }
            }
        }

        /// <summary>
        /// Outputs statements for altering view default values.
        /// </summary>
        public static void Alter(StreamWriter writer, PgSchema? oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            if (oldSchema == null)
            {
                return;
            }

            foreach (var oldView in oldSchema.Views)
            {
                var newView = newSchema.GetView(oldView.Name);

                if (newView == null)
                {
                    continue;
                }

                DiffDefaultValues(writer, oldView, newView, searchPathHelper);

                if ((oldView.Comment == null && newView.Comment != null)
                    || (oldView.Comment != null
                    && newView.Comment != null
                    && !oldView.Comment.Equals(newView.Comment)))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.Write("COMMENT ON VIEW ");
                    writer.Write(PgDiffStringExtension.QuoteName(newView.Name));
                    writer.Write(" IS ");
                    writer.Write(newView.Comment);
                    writer.WriteLine(';');
                }
                else if (oldView.Comment != null && newView.Comment == null)
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.Write("COMMENT ON VIEW ");
                    writer.Write(PgDiffStringExtension.QuoteName(newView.Name));
                    writer.WriteLine(" IS NULL;");
                }

                IList<string> columnNames = new List<string>(newView.ColumnComments.Count);

                foreach (var columnComment in newView.ColumnComments)
                {
                    columnNames.Add(columnComment.ColumnName);
                }

                foreach (var columnComment in oldView.ColumnComments)
                {
                    if (!columnNames.Contains(columnComment.ColumnName))
                    {
                        columnNames.Add(columnComment.ColumnName);
                    }
                }

                foreach (var columnName in columnNames)
                {
                    PgView.ColumnComment? oldColumnComment = null;
                    PgView.ColumnComment? newColumnComment = null;

                    foreach (var columnComment in oldView.ColumnComments)
                    {
                        if (columnName.Equals(columnComment.ColumnName))
                        {
                            oldColumnComment = columnComment;
                            break;
                        }
                    }

                    foreach (var columnComment in newView.ColumnComments)
                    {
                        if (columnName.Equals(columnComment.ColumnName))
                        {
                            newColumnComment = columnComment;
                            break;
                        }
                    }

                    if ((oldColumnComment == null && newColumnComment != null)
                        || (oldColumnComment != null
                            && newColumnComment != null
                            && !oldColumnComment.Comment.Equals(newColumnComment.Comment)))
                    {
                        searchPathHelper.OutputSearchPath(writer);
                        writer.WriteLine();
                        writer.Write("COMMENT ON COLUMN ");
                        writer.Write(PgDiffStringExtension.QuoteName(newView.Name));
                        writer.Write('.');
                        writer.Write(PgDiffStringExtension.QuoteName(newColumnComment.ColumnName));
                        writer.Write(" IS ");
                        writer.Write(newColumnComment.Comment);
                        writer.WriteLine(';');
                    }
                    else if (oldColumnComment != null && newColumnComment == null)
                    {
                        searchPathHelper.OutputSearchPath(writer);
                        writer.WriteLine();
                        writer.Write("COMMENT ON COLUMN ");
                        writer.Write(PgDiffStringExtension.QuoteName(newView.Name));
                        writer.Write('.');
                        writer.Write(PgDiffStringExtension.QuoteName(oldColumnComment.ColumnName));
                        writer.WriteLine(" IS NULL;");
                    }
                }
            }
        }

        /// <summary>
        /// Returns true if either column names or query of the view has been
        /// modified.
        /// </summary>
        private static bool IsViewModified(PgView oldView, PgView newView)
        {
            string[]? oldViewColumnNames;

            if (oldView.ColumnNames == null || oldView.ColumnNames.Count == 0)
            {
                oldViewColumnNames = null;
            }
            else
            {
                oldViewColumnNames = oldView.ColumnNames.ToArray();
            }

            string[]? newViewColumnNames;

            if (newView.ColumnNames == null || newView.ColumnNames.Count == 0)
            {
                newViewColumnNames = null;
            }
            else
            {
                newViewColumnNames = newView.ColumnNames.ToArray();
            }

            if (oldViewColumnNames == null && newViewColumnNames == null)
            {
                return !oldView.Query.Replace(" ", string.Empty).Trim().Equals(newView.Query.Replace(" ", string.Empty).Trim());
            }
            else if (oldViewColumnNames == null)
            {
                return true;
            }
            else
            {
                return !oldViewColumnNames.Equals(newViewColumnNames);
            }
        }

        /// <summary>
        /// Diffs default values in views.
        /// </summary>
        private static void DiffDefaultValues(StreamWriter writer, PgView oldView, PgView newView, SearchPathHelper searchPathHelper)
        {
            var oldValues = oldView.DefaultValues;

            var newValues = newView.DefaultValues;

            // modify defaults that are in old view
            foreach (var oldValue in oldValues)
            {
                var found = false;

                foreach (var newValue in newValues)
                {
                    if (oldValue.ColumnName.Equals(newValue.ColumnName))
                    {
                        found = true;

                        if (!oldValue.Value.Equals(newValue.Value))
                        {
                            searchPathHelper.OutputSearchPath(writer);
                            writer.WriteLine();
                            writer.Write("ALTER TABLE ");
                            writer.Write(PgDiffStringExtension.QuoteName(newView.Name));
                            writer.Write(" ALTER COLUMN ");
                            writer.Write(PgDiffStringExtension.QuoteName(newValue.ColumnName));
                            writer.Write(" SET DEFAULT ");
                            writer.Write(newValue.Value);
                            writer.WriteLine(';');
                        }

                        break;
                    }
                }

                if (!found)
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.Write("ALTER TABLE ");
                    writer.Write(PgDiffStringExtension.QuoteName(newView.Name));
                    writer.Write(" ALTER COLUMN ");
                    writer.Write(PgDiffStringExtension.QuoteName(oldValue.ColumnName));
                    writer.WriteLine(" DROP DEFAULT;");
                }
            }

            // add new defaults
            foreach (var newValue in newValues)
            {
                var found = false;

                foreach (var oldValue in oldValues)
                {
                    if (newValue.ColumnName.Equals(oldValue.ColumnName))
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    continue;
                }

                searchPathHelper.OutputSearchPath(writer);
                writer.WriteLine();
                writer.Write("ALTER TABLE ");
                writer.Write(PgDiffStringExtension.QuoteName(newView.Name));
                writer.Write(" ALTER COLUMN ");
                writer.Write(PgDiffStringExtension.QuoteName(newValue.ColumnName));
                writer.Write(" SET DEFAULT ");
                writer.Write(newValue.Value);
                writer.WriteLine(';');
            }
        }
    }
}