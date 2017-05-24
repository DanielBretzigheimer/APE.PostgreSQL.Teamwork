// <copyright file="PgDiffFunctions.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.IO;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
using APE.PostgreSQL.Teamwork.Model.Utils;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres
{
    /// <summary>
    /// Diffs functions.
    /// </summary>
    public class PgDiffFunctions
    {
        /// <summary>
        /// Creates a new instance of <see cref="PgDiffFunctions"/>.
        /// </summary>
        private PgDiffFunctions()
        {
        }

        /// <summary>
        /// Outputs statements for new or modified functions.
        /// </summary>
        public static void Create(StreamWriter writer, [NullGuard.AllowNull] PgSchema oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper, bool ignoreFunctionWhitespace)
        {
            // Add new functions and replace modified functions
            foreach (PgFunction newFunction in newSchema.Functions)
            {
                PgFunction oldFunction;

                if (oldSchema == null)
                {
                    oldFunction = null;
                }
                else
                {
                    oldFunction = oldSchema.GetFunction(newFunction.Signature);
                }

                if ((oldFunction == null) || !newFunction.Equals(oldFunction, ignoreFunctionWhitespace))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.WriteLine(newFunction.CreationSQL);
                }
            }
        }

        /// <summary>
        /// Outputs statements for dropping of functions that exist no more.
        /// </summary>
        public static void Drop(StreamWriter writer, [NullGuard.AllowNull] PgSchema oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            if (oldSchema == null)
            {
                return;
            }

            // Drop functions that exist no more
            foreach (PgFunction oldFunction in oldSchema.Functions)
            {
                if (!newSchema.ContainsFunction(oldFunction.Signature))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.WriteLine(oldFunction.DropSQL);
                }
            }
        }

        /// <summary>
        /// Outputs statements for function comments that have changed.
        /// </summary>
        public static void AlterComments(StreamWriter writer, [NullGuard.AllowNull] PgSchema oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            if (oldSchema == null)
            {
                return;
            }

            foreach (PgFunction oldfunction in oldSchema.Functions)
            {
                PgFunction newFunction = newSchema.GetFunction(oldfunction.Signature);

                if (newFunction == null)
                {
                    continue;
                }

                if ((oldfunction.Comment == null && newFunction.Comment != null)
                    || (oldfunction.Comment != null && newFunction.Comment != null && !oldfunction.Comment.Equals(newFunction.Comment)))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();

                    writer.Write("COMMENT ON FUNCTION ");
                    writer.Write(PgDiffStringExtension.QuoteName(newFunction.Name));
                    writer.Write('(');

                    var addComma = false;
                    foreach (PgFunction.Argument argument in newFunction.Arguments)
                    {
                        if (addComma)
                        {
                            writer.Write(", ");
                        }
                        else
                        {
                            addComma = true;
                        }

                        writer.Write(argument.GetDeclaration(false));
                    }

                    writer.Write(") IS ");
                    writer.Write(newFunction.Comment);
                    writer.WriteLine(';');
                }
                else if (oldfunction.Comment != null && newFunction.Comment == null)
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();

                    writer.Write("COMMENT ON FUNCTION ");
                    writer.Write(PgDiffStringExtension.QuoteName(newFunction.Name));
                    writer.Write('(');

                    var addComma = false;
                    foreach (PgFunction.Argument argument in newFunction.Arguments)
                    {
                        if (addComma)
                        {
                            writer.Write(", ");
                        }
                        else
                        {
                            addComma = true;
                        }

                        writer.Write(argument.GetDeclaration(false));
                    }

                    writer.WriteLine(") IS NULL;");
                }
            }
        }
    }
}