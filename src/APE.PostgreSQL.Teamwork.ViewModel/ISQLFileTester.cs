// <copyright file="ISQLFileTester.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    /// <summary>
    /// Encapsulates methods to test a given <see cref="SQLFile"/>.
    /// </summary>
    public interface ISQLFileTester
    {
        /// <summary>
        /// Tests the given <see cref="SQLFile"/> on the given <see cref="Database"/>.
        /// </summary>
        /// <remarks>This method expects that the target file was already executed on the given database.</remarks>
        /// <param name="target">The target <see cref="SQLFile"/> for which tables data is created.</param>
        /// <param name="database">The database on which the data is created.</param>
        void CreateData(Database database, SQLFile target);

        /// <summary>
        /// Tests all empty methods of the target <see cref="SQLFile"/> on the given <see cref="Database"/>.
        /// </summary>
        /// <remarks>This method expects that the target file was already executed on the given database.</remarks>
        /// <param name="database">The database which is used to test the files.</param>
        /// <param name="target">The target <see cref="SQLFile"/> which functions are tested.</param>
        void TestEmptyMethods(Database database, SQLFile target);
    }
}
