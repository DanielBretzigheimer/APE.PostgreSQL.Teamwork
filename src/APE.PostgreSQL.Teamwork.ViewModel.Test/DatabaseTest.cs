// <copyright file="DatabaseTest.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using APE.PostgreSQL.Teamwork.Model;
using APE.PostgreSQL.Teamwork.Model.Templates;
using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;
using APE.PostgreSQL.Teamwork.ViewModel.Postgres;
using APE.PostgreSQL.Teamwork.ViewModel.TestHelper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace APE.PostgreSQL.Teamwork.ViewModel.Test
{
    [TestClass]
    public class DatabaseTest
    {
        [TestMethod]
        [DeploymentItem("Templates", "Templates")]
        public void ConstructorTest()
        {
            var name = "TestDB";
            var path = "testpfad";

            var fileMock = new Mock<IFileSystemAccess>();
            var connectionManagerMock = new Mock<IConnectionManager>();
            var processMock = new Mock<IProcessManager>();
            var sqlFileTester = new Mock<ISQLFileTester>();
            var diffCreatorMock = new Mock<IDifferenceCreator>();
            connectionManagerMock.Setup(c => c.ExecuteCommand<int>(It.IsAny<IDatabase>(), It.IsAny<string>())).Returns(new List<int>() { 0 });

            new Action(() => new Database(name, path, null, null, null, null, null, true)).ShouldThrow<ArgumentNullException>();
            new Action(() => new Database(name, path, null, fileMock.Object, processMock.Object, diffCreatorMock.Object, sqlFileTester.Object, true)).ShouldThrow<ArgumentNullException>();
            new Action(() => new Database(name, path, connectionManagerMock.Object, null, processMock.Object, diffCreatorMock.Object, sqlFileTester.Object, true)).ShouldThrow<ArgumentNullException>();
            new Action(() => new Database(name, path, connectionManagerMock.Object, fileMock.Object, null, diffCreatorMock.Object, sqlFileTester.Object, true)).ShouldThrow<ArgumentNullException>();
            new Action(() => new Database(name, path, connectionManagerMock.Object, fileMock.Object, processMock.Object, null, sqlFileTester.Object, true)).ShouldThrow<ArgumentNullException>();
            new Action(() => new Database(name, path, connectionManagerMock.Object, fileMock.Object, processMock.Object, diffCreatorMock.Object, null, true)).ShouldThrow<ArgumentNullException>();

            var d = new Database(name, path, connectionManagerMock.Object, fileMock.Object, processMock.Object, diffCreatorMock.Object, sqlFileTester.Object, initializeData: false);
            d.Should().NotBeNull();
            d.Path.Should().Be(path);
            d.Name.Should().Be(name);
        }

        [TestMethod]
        [DeploymentItem("Templates", "Templates")]
        public void UpdateDataTest()
        {
            var name = "TestDB";
            var path = "testpfad";

            var fileMock = new Mock<IFileSystemAccess>();
            var connectionManagerMock = new Mock<IConnectionManager>();
            var processMock = new Mock<IProcessManager>();
            var diffCreatorMock = new Mock<IDifferenceCreator>();
            var sqlFileTester = new Mock<ISQLFileTester>();
            connectionManagerMock.Setup(c => c.ExecuteCommand<int>(It.IsAny<IDatabase>(), It.IsAny<string>())).Returns(new List<int>() { 0 });
            connectionManagerMock.Setup(c => c.ExecuteCommand<ExecutedFile>(It.IsAny<IDatabase>(), It.IsAny<string>())).Returns(new List<ExecutedFile>() { new ExecutedFile() { Version = "0080" } });

            // UpdateData is called internally when bool is set to true
            var d = new Database(name, path, connectionManagerMock.Object, fileMock.Object, processMock.Object, diffCreatorMock.Object, sqlFileTester.Object, initializeData: true);
            d.CurrentVersion.Main.Should().Be(80);

            connectionManagerMock.Setup(c => c.ExecuteCommand<ExecutedFile>(It.IsAny<IDatabase>(), It.IsAny<string>())).Returns(new List<ExecutedFile>() { new ExecutedFile() { Version = "0080.a" } });
            d.UpdateData();
            d.CurrentVersion.Main.Should().Be(80);

            connectionManagerMock.Setup(c => c.ExecuteCommand<ExecutedFile>(It.IsAny<IDatabase>(), It.IsAny<string>())).Returns(new List<ExecutedFile>() { new ExecutedFile() { Version = "0081" } });
            d.UpdateData();
            d.CurrentVersion.Main.Should().Be(81);
        }

        [TestMethod]
        [DeploymentItem("Templates", "Templates")]
        public void CreateDumpTest()
        {
            var name = "TestDB";
            var path = "testpfad";

            var fileMock = new Mock<IFileSystemAccess>();
            var connectionManagerMock = new Mock<IConnectionManager>();
            var processMock = new Mock<IProcessManager>();
            var diffCreatorMock = new Mock<IDifferenceCreator>();
            var sqlFileTester = new Mock<ISQLFileTester>();
            connectionManagerMock.Setup(c => c.ExecuteCommand<int>(It.IsAny<IDatabase>(), It.IsAny<string>())).Returns(new List<int>() { 0 });
            connectionManagerMock.Setup(c => c.ExecuteCommand<ExecutedFile>(It.IsAny<IDatabase>(), It.IsAny<string>())).Returns(new List<ExecutedFile>() { new ExecutedFile() { Version = "0080" } });
            processMock.Setup(p => p.Execute(It.IsAny<ProcessStartInfo>())).Verifiable();

            // set this because in create dump a SQL File is created which needs this
            fileMock.Setup(f => f.FileExists(It.IsAny<string>())).Returns(true);

            var d = new Database(name, path, connectionManagerMock.Object, fileMock.Object, processMock.Object, diffCreatorMock.Object, sqlFileTester.Object, initializeData: false)
            {
                CurrentVersion = DatabaseVersion.StartVersion,
            };
            d.CreateDump(string.Empty, string.Empty, string.Empty, string.Empty);

            processMock.VerifyAll();
        }

        [TestMethod]
        [DeploymentItem("Templates", "Templates")]
        public void UpdateToVersionTest()
        {
            var name = "TestDB";
            var path = "testpfad";

            var fileMock = new Mock<IFileSystemAccess>();
            var connectionManagerMock = new Mock<IConnectionManager>();
            var processMock = new Mock<IProcessManager>();
            var diffCreatorMock = new Mock<IDifferenceCreator>();
            var sqlFileTester = new Mock<ISQLFileTester>();
            connectionManagerMock.Setup(c => c.ExecuteCommand<int>(It.IsAny<IDatabase>(), It.IsAny<string>())).Returns(new List<int>() { 0 });
            connectionManagerMock.Setup(c => c.ExecuteCommand<ExecutedFile>(It.IsAny<IDatabase>(), It.IsAny<string>())).Returns(new List<ExecutedFile>() { new ExecutedFile() { Version = "0080" } });
            fileMock.Setup(d => d.DirectoryExists(It.IsAny<string>())).Returns(true);
            fileMock.Setup(d => d.GetFiles(It.IsAny<string>())).Returns(new string[]
            {
                "\\Test\\0001" + SQLTemplates.DumpFile,
                "\\Test\\0002" + SQLTemplates.DiffFile,
                "\\Test\\0003" + SQLTemplates.UndoDiffFile,
                "\\TestWithOutVersion" + SQLTemplates.DiffFile,
            });

            // set this because a SQL File is created which needs this
            fileMock.Setup(f => f.FileExists(It.IsAny<string>())).Returns(true);

            // UpdateData is called internally when bool is set to true
            var database = new Database(name, path, connectionManagerMock.Object, fileMock.Object, processMock.Object, diffCreatorMock.Object, sqlFileTester.Object, initializeData: true);

            var version82 = new DatabaseVersion("\\0082.diff.sql");
            connectionManagerMock.Setup(c => c.ExecuteCommand<ExecutedFile>(It.IsAny<IDatabase>(), It.IsAny<string>())).Returns(new List<ExecutedFile>() { new ExecutedFile() { Version = "0082" } });
            database.UpdateToVersion(version82);
            database.CurrentVersion.Main.Should().Be(82);

            database.UpdateToVersion(DatabaseVersion.StartVersion);
            connectionManagerMock.Setup(c => c.ExecuteCommand<ExecutedFile>(It.IsAny<IDatabase>(), It.IsAny<string>())).Returns(new List<ExecutedFile>());
            database.CallUpdateVersion();
            database.CurrentVersion.Main.Should().Be(0);
        }

        [TestMethod]
        [DeploymentItem("Templates", "Templates")]
        public void SearchFilesTest()
        {
            var name = "TestDB";
            var path = "testpfad";

            var fileMock = new Mock<IFileSystemAccess>();
            var connectionManagerMock = new Mock<IConnectionManager>();
            var processMock = new Mock<IProcessManager>();
            var diffCreatorMock = new Mock<IDifferenceCreator>();
            var sqlFileTester = new Mock<ISQLFileTester>();
            connectionManagerMock.Setup(c => c.ExecuteCommand<int>(It.IsAny<IDatabase>(), It.IsAny<string>())).Returns(new List<int>() { 0 });
            connectionManagerMock.Setup(c => c.ExecuteCommand<ExecutedFile>(It.IsAny<IDatabase>(), It.IsAny<string>())).Returns(new List<ExecutedFile>());
            fileMock.Setup(d => d.DirectoryExists(It.IsAny<string>())).Returns(true);
            fileMock.Setup(d => d.GetFiles(It.IsAny<string>())).Returns(new string[]
            {
                "\\Test\\0001" + SQLTemplates.DumpFile,
                "\\Test\\0002" + SQLTemplates.DiffFile,
                "\\Test\\0003" + SQLTemplates.UndoDiffFile,
                "\\TestWithOutVersion" + SQLTemplates.DiffFile,
            });

            // set this because a SQL File is created which needs this
            fileMock.Setup(f => f.FileExists(It.IsAny<string>())).Returns(true);

            // UpdateData is called internally when bool is set to true
            var database = new Database(name, path, connectionManagerMock.Object, fileMock.Object, processMock.Object, diffCreatorMock.Object, sqlFileTester.Object, initializeData: true);

            database.CallSearchFiles(true);
            database.UndoDiffFiles.Count.Should().Be(1);
            database.DiffFiles.Count.Should().Be(2);
        }

        [TestMethod]
        [DeploymentItem("Templates", "Templates")]
        public void ReduceVersionTest()
        {
            var name = "TestDB";
            var path = "testpfad";

            var fileMock = new Mock<IFileSystemAccess>();
            var connectionManagerMock = new Mock<IConnectionManager>();
            var processMock = new Mock<IProcessManager>();
            var diffCreatorMock = new Mock<IDifferenceCreator>();
            var sqlFileTester = new Mock<ISQLFileTester>();

            connectionManagerMock.Setup(c => c.ExecuteCommand<int>(It.IsAny<IDatabase>(), It.IsAny<string>())).Returns(new List<int>() { 0 });
            connectionManagerMock.Setup(c => c.ExecuteCommand<ExecutedFile>(It.IsAny<IDatabase>(), It.IsAny<string>())).Returns(new List<ExecutedFile>());

            // UpdateData is called internally when bool is set to true
            var database = new Database(name, path, connectionManagerMock.Object, fileMock.Object, processMock.Object, diffCreatorMock.Object, sqlFileTester.Object, initializeData: false)
            {
                CurrentVersion = DatabaseVersion.StartVersion,
            };
            connectionManagerMock.Setup(c => c.ExecuteCommandNonQuery(It.IsAny<IDatabase>(), SQLTemplates.RemoveVersion(database.CurrentVersion))).Verifiable();

            database.ReduceVersion();

            connectionManagerMock.VerifyAll();
        }

        [TestMethod]
        [DeploymentItem("Templates", "Templates")]
        public void TestSQLFileTest()
        {
            var name = "TestDB";
            var path = "testpfad";

            var fileMock = new Mock<IFileSystemAccess>();
            var connectionManagerMock = new Mock<IConnectionManager>();
            var processMock = new Mock<IProcessManager>();
            var diffCreatorMock = new Mock<IDifferenceCreator>();
            var sqlFileTester = new Mock<ISQLFileTester>();

            connectionManagerMock.Setup(c => c.ExecuteCommand<int>(It.IsAny<IDatabase>(), It.IsAny<string>())).Returns(new List<int>() { 0 });
            connectionManagerMock.Setup(c => c.ExecuteCommand<ExecutedFile>(It.IsAny<IDatabase>(), It.IsAny<string>())).Returns(new List<ExecutedFile>());
            fileMock.Setup(d => d.DirectoryExists(It.IsAny<string>())).Returns(true);
            fileMock.Setup(d => d.GetFiles(It.IsAny<string>())).Returns(new string[]
            {
                "\\Test\\0001" + SQLTemplates.DumpFile,
                "\\Test\\0002" + SQLTemplates.DiffFile,
                "\\Test\\0003" + SQLTemplates.UndoDiffFile,
                "\\TestWithOutVersion" + SQLTemplates.DiffFile,
            });

            // set this because a SQL File is created which needs this
            fileMock.Setup(f => f.FileExists(It.IsAny<string>())).Returns(true);

            // UpdateData is called internally when bool is set to true
            var database = new Database(name, path, connectionManagerMock.Object, fileMock.Object, processMock.Object, diffCreatorMock.Object, sqlFileTester.Object, initializeData: false);

            database.TestSQLFiles();
        }

        [TestMethod]
        [DeploymentItem("Templates", "Templates")]
        public void ResetTest()
        {
            var name = "TestDB";
            var path = "testpfad";

            var fileMock = new Mock<IFileSystemAccess>();
            var connectionManagerMock = new Mock<IConnectionManager>();
            var processMock = new Mock<IProcessManager>();
            var sqlFileMock = new Mock<ISQLFile>();
            var diffCreatorMock = new Mock<IDifferenceCreator>();
            var sqlFileTester = new Mock<ISQLFileTester>();

            connectionManagerMock.Setup(c => c.ExecuteCommand<int>(It.IsAny<IDatabase>(), It.IsAny<string>()))
                .Returns(new List<int>() { 0 })
                .Verifiable();
            connectionManagerMock.Setup(c => c.ExecuteCommand<ExecutedFile>(It.IsAny<IDatabase>(), It.IsAny<string>()))
                .Returns(new List<ExecutedFile>())
                .Verifiable();

            // UpdateData is called internally when bool is set to true
            var database = new Database(name, path, connectionManagerMock.Object, fileMock.Object, processMock.Object, diffCreatorMock.Object, sqlFileTester.Object, initializeData: false);

            connectionManagerMock.Setup(c => c.ExecuteCommandNonQuery(It.IsAny<IDatabase>(), SQLTemplates.CreateDatabase(name)));

            database.Reset();

            connectionManagerMock.VerifyAll();
        }

        [TestMethod]
        [DeploymentItem("Templates", "Templates")]
        public void ExportTest()
        {
            var name = "TestDB";
            var path = "testpfad";

            var fileMock = new Mock<IFileSystemAccess>();
            var connectionManagerMock = new Mock<IConnectionManager>();
            var processMock = new Mock<IProcessManager>();
            var sqlFileMock = new Mock<ISQLFile>();
            var diffCreatorMock = new Mock<IDifferenceCreator>();
            var sqlFileTester = new Mock<ISQLFileTester>();

            connectionManagerMock.Setup(c => c.ExecuteCommand<int>(It.IsAny<IDatabase>(), It.IsAny<string>())).Returns(new List<int>() { 0 });
            connectionManagerMock.Setup(c => c.ExecuteCommand<ExecutedFile>(It.IsAny<IDatabase>(), It.IsAny<string>())).Returns(new List<ExecutedFile>());
            diffCreatorMock.Setup(d => d.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            // set this because in create dump a SQL File is created which needs this
            fileMock.Setup(f => f.FileExists(It.IsAny<string>())).Returns(true);

            // UpdateData is called internally when bool is set to true
            var database = new Database(name, path, connectionManagerMock.Object, fileMock.Object, processMock.Object, diffCreatorMock.Object, sqlFileTester.Object, initializeData: false);

            fileMock.Setup(d => d.DirectoryExists(It.IsAny<string>())).Returns(true);
            fileMock.Setup(d => d.GetFiles(It.IsAny<string>())).Returns(new string[]
            {
                "\\Test\\0001" + SQLTemplates.DumpFile,
                "\\Test\\0002" + SQLTemplates.DiffFile,
                "\\Test\\0003" + SQLTemplates.DiffFile,
            });

            database.CurrentVersion = DatabaseVersion.StartVersion;
            new Action(() => database.Export(string.Empty, string.Empty, string.Empty, string.Empty)).ShouldThrow<TeamworkConnectionException>();

            connectionManagerMock.Setup(c => c.ExecuteCommand<ExecutedFile>(It.IsAny<IDatabase>(), It.IsAny<string>())).Returns(new List<ExecutedFile>() { new ExecutedFile() { Version = "0004" } });
            database.UpdateData();

            new Action(() => database.Export(string.Empty, string.Empty, string.Empty, string.Empty)).ShouldThrow<TeamworkException>();

            fileMock.Setup(f => f.ReadAllLines("testpfad\\0004.dump.sql")).Returns(new string[1]);
            database.Export(string.Empty, string.Empty, string.Empty, string.Empty);

            diffCreatorMock.Setup(d => d.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new TeamworkConnectionException(new SQLFile("\\0001.diff.sql", database, fileMock.Object), string.Empty));
            new Action(() => database.Export(string.Empty, string.Empty, string.Empty, string.Empty)).ShouldThrow<Exception>();

            diffCreatorMock.Setup(d => d.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception());
            new Action(() => database.Export(string.Empty, string.Empty, string.Empty, string.Empty)).ShouldThrow<Exception>();
        }

        [TestMethod]
        [DeploymentItem("Templates", "Templates")]
        public void CreateDiffTest()
        {
            var name = "TestDB";
            var path = "testpfad";

            var fileMock = new Mock<IFileSystemAccess>();
            var connectionManagerMock = new Mock<IConnectionManager>();
            var processMock = new Mock<IProcessManager>();
            var sqlFileMock = new Mock<ISQLFile>();
            var diffCreatorMock = new Mock<IDifferenceCreator>();
            var sqlFileTester = new Mock<ISQLFileTester>();

            connectionManagerMock.Setup(c => c.ExecuteCommand<int>(It.IsAny<IDatabase>(), It.IsAny<string>())).Returns(new List<int>() { 0 });
            connectionManagerMock.Setup(c => c.ExecuteCommand<ExecutedFile>(It.IsAny<IDatabase>(), It.IsAny<string>())).Returns(new List<ExecutedFile>());

            // UpdateData is called internally when bool is set to true
            var database = new Database(name, path, connectionManagerMock.Object, fileMock.Object, processMock.Object, diffCreatorMock.Object, sqlFileTester.Object, initializeData: false);

            new Action(() => { database.CallCreateDiffs(string.Empty, string.Empty, string.Empty, string.Empty); }).ShouldThrow<FileNotFoundException>();

            fileMock.Setup(f => f.FileExists(It.IsAny<string>())).Returns(true);
            new Action(() => { database.CallCreateDiffs(string.Empty, string.Empty, string.Empty, string.Empty); }).ShouldThrow<TeamworkException>();
            new Action(() => { database.CallCreateDiffs("0001.dump.sql", "0002.dump.sql", "path1", "path2"); }).ShouldThrow<TeamworkException>();

            diffCreatorMock.Setup(d => d.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true).Verifiable();
            database.CallCreateDiffs("0001.dump.sql", "0002.dump.sql", "\\path1\\0002.diff.sql", "path1\\0002.undodiff.sql");

            diffCreatorMock.VerifyAll();
        }
    }
}
