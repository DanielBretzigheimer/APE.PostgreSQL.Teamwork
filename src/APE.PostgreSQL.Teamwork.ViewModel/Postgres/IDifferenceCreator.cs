// <copyright file="IDifferenceCreator.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>

using System.IO;

namespace APE.PostgreSQL.Teamwork.ViewModel.Postgres
{
    /// <summary>
    /// Creates a new SQL File which contains all difference between the two dump files.
    /// </summary>
    public interface IDifferenceCreator
    {
        /// <summary>
        /// Creates a difference file of the two dumps at the given path.
        /// </summary>
        /// <returns>Boolean indicating whether differences where found or not.</returns>
        bool Create(string filePath, Database database, string oldDumpFile, string newDumpFile);

        /// <summary>
        /// Creates a difference file of the two dumps at the given path.
        /// </summary>
        /// <returns>Boolean indicating whether differences where found or not.</returns>
        bool Create(Stream stream, Database database, string oldDumpFile, string newDumpFile);
    }
}
