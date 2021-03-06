﻿// <copyright file="FileType.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
namespace APE.PostgreSQL.Teamwork
{
    /// <summary>
    /// Type of the files from the database table.
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// Difference files which can be executed to apply the difference between two dumps.
        /// </summary>
        Diff,

        /// <summary>
        /// Differences files which can be executed to undo the differences of a <see cref="Diff"/> file.
        /// </summary>
        UndoDiff,

        /// <summary>
        /// Dump files which are generated by postgres and which contain the current state of a database.
        /// </summary>
        Dump,
    }
}
