// <copyright file="PgDiffRules.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>

using System.IO;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres
{
    public class PgDiffRules
    {
        static internal void Drop(StreamWriter writer, PgSchema? oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            if (oldSchema == null)
                return;

            foreach (var rule in oldSchema.Rules)
            {
                if (!newSchema.Contains(rule))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.WriteLine(rule.DropSql);
                }
            }
        }

        /// <summary>
        /// Outputs statements for creation of new <see cref="PgRule"/>s.
        /// </summary>
        static internal void Create(StreamWriter writer, PgSchema? oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            foreach (var rule in newSchema.Rules)
            {
                if (oldSchema == null || !oldSchema.Contains(rule))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.WriteLine(rule.CreationSql);
                }
            }
        }
    }
}
