// <copyright file="PgDiffTypes.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres
{
    public class PgDiffTypes
    {
        /// <summary>
        /// Creates the types.
        /// </summary>
        internal static void CreateTypes(StreamWriter writer, PgSchema oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            foreach (PgType newType in newSchema.Types)
            {
                if (oldSchema == null || !oldSchema.ContainsType(newType.Signature))
                {
                    PgType oldType = null;
                    if (oldSchema != null)
                        oldType = oldSchema.GetEnum(newType.Name);
                    if (oldType != null)
                    {
                        // if definitions of type changed it will be ignored to
                        continue;
                    }

                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.WriteLine(newType.CreationSQL);
                }
            }
        }

        /// <summary>
        /// Drops the types.
        /// </summary>
        internal static void DropTypes(StreamWriter writer, PgSchema oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            if (oldSchema == null)
                return;

            foreach (PgType oldType in oldSchema.Types)
            {
                if (!newSchema.ContainsType(oldType.Signature))
                {
                    List<string> newValues = newSchema.TypeEntriesChanged(oldType);
                    if (newValues != null)
                        AddDefinition(writer, oldType, newValues, searchPathHelper);
                    else
                    {
                        var newType = newSchema.GetEnum(oldType.Name);
                        if (newType != null)
                        {
                            // definitions of type changed
                            // to avoid conflicts, dont delete schema and ignore the additional definition
                            writer.WriteLine("-- Type was not dropped because of conflicts");
                        }
                        else
                        {
                            // type was deleted
                            searchPathHelper.OutputSearchPath(writer);
                            writer.WriteLine();
                            writer.WriteLine(oldType.DropSQL);
                        }
                    }
                }
            }
        }

        private static void AddDefinition(StreamWriter writer, PgType type, List<string> newValues, SearchPathHelper searchPathHelper)
        {
            foreach (string newValue in newValues)
            {
                searchPathHelper.OutputSearchPath(writer);
                writer.WriteLine();
                writer.WriteLine(type.AlterSQL(newValue));
            }
        }
    }
}
