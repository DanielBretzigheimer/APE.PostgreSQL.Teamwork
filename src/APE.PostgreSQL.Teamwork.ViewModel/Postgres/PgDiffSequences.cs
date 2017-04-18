// <copyright file="PgDiffSequences.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.IO;
using System.Text;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
using APE.PostgreSQL.Teamwork.Model.Utils;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres
{
    /// <summary>
    /// Diffs sequences.
    /// </summary>
    public class PgDiffSequences
    {
        /// <summary>
        /// Creates a new instance of <see cref="PgDiffSequences"/>.
        /// </summary>
        private PgDiffSequences()
        {
        }

        /// <summary>
        /// Outputs statements for creation of new sequences.
        /// </summary>
        /// <param name="writer">Writer the output should be written to.</param>
        /// <param name="oldSchema">Original schema.</param>
        /// <param name="newSchema">New schema.</param>
        /// <param name="searchPathHelper">Search path helper.</param>
        public static void Create(StreamWriter writer, PgSchema oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            // Add new sequences
            foreach (PgSequence sequence in newSchema.Sequences)
            {
                if (oldSchema == null || !oldSchema.ContainsSequence(sequence.Name))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.WriteLine(sequence.CreationSQL);
                }
            }
        }

        /// <summary>
        /// Outputs statements for altering of new sequences.
        /// </summary>
        /// <param name="writer">Writer the output should be written to.</param>
        /// <param name="oldSchema">The old schema.</param>
        /// <param name="newSchema">The new schema.</param>
        /// <param name="searchPathHelper">The search path helper.</param>
        public static void AlterCreatedSequences(StreamWriter writer, PgSchema oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            // Alter created sequences
            foreach (PgSequence sequence in newSchema.Sequences)
            {
                if ((oldSchema == null || !oldSchema.ContainsSequence(sequence.Name)) && sequence.Owner != null && sequence.Owner != string.Empty)
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.WriteLine(sequence.OwnedBySQL);
                }
            }
        }

        /// <summary>
        /// Outputs statements for dropping of sequences that do not exist anymore.
        /// </summary>
        public static void Drop(StreamWriter writer, PgSchema oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            if (oldSchema == null)
            {
                return;
            }

            // Drop sequences that do not exist in new schema
            foreach (PgSequence sequence in oldSchema.Sequences)
            {
                if (!newSchema.ContainsSequence(sequence.Name))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.WriteLine(sequence.DropSQL);
                }
            }
        }

        /// <summary>
        /// Outputs statement for modified sequences.
        /// </summary>
        public static void Alter(StreamWriter writer, PgSchema oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper, bool ignoreStartWith)
        {
            if (oldSchema == null)
            {
                return;
            }

            var alterSequenceSql = new StringBuilder(100);
            foreach (PgSequence newSequence in newSchema.Sequences)
            {
                PgSequence oldSequence = oldSchema.GetSequence(newSequence.Name);

                if (oldSequence == null)
                {
                    continue;
                }

                alterSequenceSql.Length = 0;

                var oldIncrement = oldSequence.Increment;
                var newIncrement = newSequence.Increment;

                if (newIncrement != null && !newIncrement.Equals(oldIncrement))
                {
                    alterSequenceSql.Append("\n\tINCREMENT BY ");
                    alterSequenceSql.Append(newIncrement);
                }

                var oldMinValue = oldSequence.MinValue;
                var newMinValue = newSequence.MinValue;

                if (newMinValue == null && oldMinValue != null)
                {
                    alterSequenceSql.Append("\n\tNO MINVALUE");
                }
                else if (newMinValue != null && !newMinValue.Equals(oldMinValue))
                {
                    alterSequenceSql.Append("\n\tMINVALUE ");
                    alterSequenceSql.Append(newMinValue);
                }

                var oldMaxValue = oldSequence.MaxValue;
                var newMaxValue = newSequence.MaxValue;

                if (newMaxValue == null && oldMaxValue != null)
                {
                    alterSequenceSql.Append("\n\tNO MAXVALUE");
                }
                else if (newMaxValue != null && !newMaxValue.Equals(oldMaxValue))
                {
                    alterSequenceSql.Append("\n\tMAXVALUE ");
                    alterSequenceSql.Append(newMaxValue);
                }

                if (!ignoreStartWith)
                {
                    var oldStart = oldSequence.StartWith;
                    var newStart = newSequence.StartWith;

                    if (newStart != null && !newStart.Equals(oldStart))
                    {
                        alterSequenceSql.Append("\n\tRESTART WITH ");
                        alterSequenceSql.Append(newStart);
                    }
                }

                var oldCache = oldSequence.Cache;
                var newCache = newSequence.Cache;

                if (newCache != null && !newCache.Equals(oldCache))
                {
                    alterSequenceSql.Append("\n\tCACHE ");
                    alterSequenceSql.Append(newCache);
                }

                var oldCycle = oldSequence.Cycle;
                var newCycle = newSequence.Cycle;

                if (oldCycle && !newCycle)
                {
                    alterSequenceSql.Append("\n\tNO CYCLE");
                }
                else if (!oldCycle && newCycle)
                {
                    alterSequenceSql.Append("\n\tCYCLE");
                }

                var oldOwnedBy = oldSequence.Owner;
                var newOwnedBy = newSequence.Owner;

                if (newOwnedBy != null && !newOwnedBy.Equals(oldOwnedBy))
                {
                    alterSequenceSql.Append("\n\tOWNED BY ");
                    alterSequenceSql.Append(newOwnedBy);
                }

                if (alterSequenceSql.Length > 0)
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.Write("ALTER SEQUENCE " + PgDiffStringExtension.QuoteName(newSequence.Name));
                    writer.Write(alterSequenceSql.ToString());
                    writer.WriteLine(';');
                }

                if ((oldSequence.Comment == null && newSequence.Comment != null)
                    || (oldSequence.Comment != null
                    && newSequence.Comment != null
                    && !oldSequence.Comment.Equals(newSequence.Comment)))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.Write("COMMENT ON SEQUENCE ");
                    writer.Write(PgDiffStringExtension.QuoteName(newSequence.Name));
                    writer.Write(" IS ");
                    writer.Write(newSequence.Comment);
                    writer.WriteLine(';');
                }
                else if (oldSequence.Comment != null && newSequence.Comment == null)
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.Write("COMMENT ON SEQUENCE ");
                    writer.Write(newSequence.Name);
                    writer.WriteLine(" IS NULL;");
                }
            }
        }
    }
}