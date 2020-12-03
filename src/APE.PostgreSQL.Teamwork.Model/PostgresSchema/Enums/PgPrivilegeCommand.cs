// <copyright file="PgPrivilegeCommand.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APE.PostgreSQL.Teamwork.Model.PostgresSchema.Enums
{
    /// <summary>
    /// The postgres privilege commands.
    /// </summary>
    public enum PgPrivilegeCommand
    {
        Grant,
        Revoke,
    }
}
