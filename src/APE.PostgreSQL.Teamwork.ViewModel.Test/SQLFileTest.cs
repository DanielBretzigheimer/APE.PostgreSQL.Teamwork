// <copyright file="SQLFileTest.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.IO;
using System.Linq;
using APE.PostgreSQL.Teamwork.Model;
using APE.PostgreSQL.Teamwork.Model.Templates;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace APE.PostgreSQL.Teamwork.ViewModel.Test
{
    [TestClass]
    public class SQLFileTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var databaseMock = new Mock<IDatabase>();
            var fileMock = new Mock<IFileSystemAccess>();

            new Action(() => { new SQLFile(string.Empty, databaseMock.Object, fileMock.Object); }).Should().Throw<FileNotFoundException>();

            fileMock.Setup(f => f.FileExists(It.IsAny<string>())).Returns(true);
            new Action(() => { new SQLFile(@"D:\SVN_Arbeitskopien\Loepfe\trunk\BDE\src\Loepfe.BDE.Database\0162.a.undoDiff.dat", databaseMock.Object, fileMock.Object); }).Should().Throw<ArgumentException>();

            new Action(() => { new SQLFile(@"D:\SVN_Arbeitskopien\Loepfe\trunk\BDE\src\Loepfe.BDE.Database\0162.a.undoDiff.dat", databaseMock.Object, null); }).Should().Throw<ArgumentNullException>();
            new Action(() => { new SQLFile(@"D:\SVN_Arbeitskopien\Loepfe\trunk\BDE\src\Loepfe.BDE.Database\0162.a.undoDiff.dat", null, fileMock.Object); }).Should().Throw<ArgumentNullException>();

            var file = new SQLFile(@"D:\SVN_Arbeitskopien\Loepfe\trunk\BDE\src\Loepfe.BDE.Database\0162.a.undoDiff.sql", databaseMock.Object, fileMock.Object);
            file.FileType.Should().Be(FileType.UndoDiff);

            file = new SQLFile(@"D:\SVN_Arbeitskopien\Loepfe\trunk\BDE\src\Loepfe.BDE.Database\0162.a.diff.sql", databaseMock.Object, fileMock.Object);
            file.FileType.Should().Be(FileType.Diff);

            file = new SQLFile(@"D:\SVN_Arbeitskopien\Loepfe\trunk\BDE\src\Loepfe.BDE.Database\0077.dump.sql", databaseMock.Object, fileMock.Object);
            file.FileType.Should().Be(FileType.Dump);
        }

        [TestMethod]
        [DeploymentItem("Templates", "Templates")]
        public void ExecuteTest()
        {
            var databaseMock = new Mock<IDatabase>();
            var fileMock = new Mock<IFileSystemAccess>();
            fileMock.Setup(f => f.FileExists(It.IsAny<string>())).Returns(true);

            var sqlStatements = new string[] { "SELECT * FROM \"Test\";" };

            fileMock.Setup(f => f.ReadAllLines(It.IsAny<string>())).Returns(sqlStatements);

            var file = new SQLFile(@"D:\SVN_Arbeitskopien\Loepfe\trunk\BDE\src\Loepfe.BDE.Database\0162.a.undoDiff.sql", databaseMock.Object, fileMock.Object);

            file.ExecuteInTransaction();
        }

        [TestMethod]
        public void GetSQLStatementsTest()
        {
            var databaseMock = new Mock<IDatabase>();
            var fileMock = new Mock<IFileSystemAccess>();
            fileMock.Setup(f => f.FileExists(It.IsAny<string>())).Returns(true);
            var sqlStatements = new string[]
            {
                "SET search_path = public;",
                "SELECT * FROM \"Test\";",
                "CREATE FUNCTION test()",
                "$$;",
                "CREATE FUNCTION test()",
                "$_$;",
                "CREATE FUNCTION test()",
                "OWNER TO public;",
                "BEGIN;",
                "COMMIT;",
                "Blub",
            };

            fileMock.Setup(f => f.ReadAllLines(It.IsAny<string>())).Returns(sqlStatements);

            var file = new SQLFile(@"D:\SVN_Arbeitskopien\Loepfe\trunk\BDE\src\Loepfe.BDE.Database\0162.a.undoDiff.sql", databaseMock.Object, fileMock.Object);

            file.SQLStatements.Count().Should().Be(7);
        }

        [TestMethod]
        [DeploymentItem("Templates", "Templates")]
        public void MarkAsExecutedTest()
        {
            var databaseMock = new Mock<IDatabase>();
            var fileMock = new Mock<IFileSystemAccess>();
            fileMock.Setup(f => f.FileExists(It.IsAny<string>())).Returns(true);

            var file = new SQLFile("\\0001.diff.sql", databaseMock.Object, fileMock.Object);

            databaseMock.Setup(c => c.ExecuteCommandNonQuery(SQLTemplates.AddExecutedFileSql(new DatabaseVersion("\\Test\\0001" + SQLTemplates.DiffFile), FileType.Diff, "Exported from this database"))).Verifiable();

            file.MarkAsExecuted();

            databaseMock.VerifyAll();
        }
    }
}
