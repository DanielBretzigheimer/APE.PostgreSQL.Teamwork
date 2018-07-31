// <copyright file="IPgObject.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>

namespace APE.PostgreSQL.Teamwork.Model.PostgresSchema
{
    public interface IPgObject
    {
        /// <summary>
        /// Gets the SQL for the creation of this <see cref="IPgObject"/>.
        /// </summary>
        string CreationSql { get; }

        /// <summary>
        /// Gets the SQL for the drop of this <see cref="IPgObject"/>.
        /// </summary>
        string DropSql { get; }
    }
}
