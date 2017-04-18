// <copyright file="PgDiffPrivileges.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema.Enums;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres
{
    public static class PgDiffPrivileges
    {
        private const string BetaMessage = DifferenceCreator.WarningMessagePrefix + "Privileges are in beta and may not work as expected!";

        /// <summary>
        /// Adds <see cref="PgPrivilegeCommand.Grant"/> and <see cref="PgPrivilegeCommand.Revoke"/> privileges.
        /// </summary>
        public static void Create(StreamWriter writer, PgSchema oldSchema, PgSchema newSchema, SearchPathHelper searchPathHelper)
        {
            // Add new sequences
            foreach (var privilege in newSchema.Privileges)
            {
                if (oldSchema == null || !oldSchema.ContainsPrivilege(privilege))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.WriteLine(BetaMessage);
                    writer.WriteLine(privilege.Create());
                }
            }

            if (oldSchema == null)
            {
                return;
            }

            // revert privileges
            foreach (var privilege in oldSchema.Privileges)
            {
                if (!newSchema.ContainsPrivilege(privilege))
                {
                    searchPathHelper.OutputSearchPath(writer);
                    writer.WriteLine();
                    writer.WriteLine(BetaMessage);
                    writer.WriteLine(privilege.CreateRevert());
                }
            }
        }
    }
}
