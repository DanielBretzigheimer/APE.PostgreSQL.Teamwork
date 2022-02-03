// <copyright file="ErrorStatus.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>

namespace APE.PostgreSQL.Teamwork.Model
{
    /// <summary>
    /// All status a statement can get.
    /// </summary>
    public enum ErrorStatus
    {
        /// <summary>
        /// The SQL was executed but returned an error.
        /// </summary>
        Error,

        /// <summary>
        /// The SQL was successfully executed.
        /// </summary>
        Successful,

        /// <summary>
        /// The SQL was not executed.
        /// </summary>
        Unknown,
    }
}
