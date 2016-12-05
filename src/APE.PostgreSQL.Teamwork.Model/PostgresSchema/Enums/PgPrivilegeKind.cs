// <copyright file="PgPrivilegeKind.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APE.PostgreSQL.Teamwork.Model.PostgresSchema.Enums
{
    /// <summary>
    /// All postgres privilege kinds.
    /// </summary>
    public enum PgPrivilegeKind
    {
        Select,
        Insert,
        Update,
        Delete,
        Truncate,
        References,
        Trigger,
        Create,
        Connect,
        Temporary,
        Execute,
        Usage,
        All,
    }
}
