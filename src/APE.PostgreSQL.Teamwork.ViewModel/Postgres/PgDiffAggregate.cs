// <copyright file="PgDiffAggregate.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.IO;
using APE.PostgreSQL.Teamwork;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
using APE.PostgreSQL.Teamwork.Model.Utils;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres
{
    /// <summary>
    /// Diffs aggregates.
    /// </summary>
    public class PgDiffAggregate
    {
        private PgDiffAggregate()
        {
        }

        /// <summary>
        /// Outputs statements for new or modified aggregates.
        /// </summary>
        /// <param name="writer">Writer the output should be written to.</param>
        /// <param name="oldSchema">Original schema.</param>
        /// <param name="newSchema">New schema.</param>
        /// <param name="searchPathHelper">Search path helper.</param>
        public static void Create(StreamWriter writer, PgSchema oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            // Add new aggregates and replace modified aggregates
            foreach (PgAggregate newAggregate in newSchema.Aggregates)
            {
                PgAggregate oldAggregate;

                if (oldSchema == null)
                {
                    oldAggregate = null;
                }
                else
                {
                    oldAggregate = oldSchema.GetAggregate(newAggregate.Signature);
                }

                if ((oldAggregate == null) || !newAggregate.Equals(oldAggregate))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.WriteLine(newAggregate.CreationSQL);
                }
            }
        }

        /// <summary>
        /// Outputs statements for dropping of aggregates that exist no more.
        /// </summary>
        /// <param name="writer">Writer the output should be written to.</param>
        /// <param name="oldSchema">Original schema.</param>
        /// <param name="newSchema">New schema.</param>
        /// <param name="searchPathHelper">Search path helper.</param>
        public static void Drop(StreamWriter writer, PgSchema oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            if (oldSchema == null)
            {
                return;
            }

            // Drop aggregates that exist no more
            foreach (PgAggregate oldAggregate in oldSchema.Aggregates)
            {
                if (!newSchema.ContainsAggregate(oldAggregate.Signature))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.WriteLine(oldAggregate.DropSQL);
                }
            }
        }

        /// <summary>
        /// Outputs statements for aggregate comments that have changed.
        /// </summary>
        public static void AlterComments(StreamWriter writer, PgSchema oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            if (oldSchema == null)
            {
                return;
            }

            foreach (PgAggregate oldAggregate in oldSchema.Aggregates)
            {
                PgAggregate newAggregate = newSchema.GetAggregate(oldAggregate.Signature);

                if (newAggregate == null)
                {
                    continue;
                }

                if ((oldAggregate.Comment == null && newAggregate.Comment != null)
                    || (oldAggregate.Comment != null && newAggregate.Comment != null && !oldAggregate.Comment.Equals(newAggregate.Comment)))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();

                    writer.Write("COMMENT ON AGGREGATE ");
                    writer.Write(PgDiffStringExtension.QuoteName(newAggregate.Name));
                    writer.Write('(');

                    var addComma = false;

                    foreach (PgAggregate.Argument argument in newAggregate.Arguments)
                    {
                        if (addComma)
                        {
                            writer.Write(", ");
                        }
                        else
                        {
                            addComma = true;
                        }

                        writer.Write(argument.DataType);
                    }

                    writer.Write(") IS ");
                    writer.Write(newAggregate.Comment);
                    writer.WriteLine(';');
                }
                else if (oldAggregate.Comment != null && newAggregate.Comment == null)
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();

                    writer.Write("COMMENT ON AGGREGATE ");
                    writer.Write(PgDiffStringExtension.QuoteName(newAggregate.Name));
                    writer.Write('(');

                    var addComma = false;
                    foreach (PgAggregate.Argument argument in newAggregate.Arguments)
                    {
                        if (addComma)
                        {
                            writer.Write(", ");
                        }
                        else
                        {
                            addComma = true;
                        }

                        writer.Write(argument.DataType);
                    }

                    writer.WriteLine(") IS NULL;");
                }
            }
        }
    }
}