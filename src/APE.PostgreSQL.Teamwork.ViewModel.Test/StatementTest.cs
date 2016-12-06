// <copyright file="StatementTest.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using APE.PostgreSQL.Teamwork.Model;
using APE.PostgreSQL.Teamwork.TestHelper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace APE.PostgreSQL.Teamwork.ViewModel.Test
{
    [TestClass]
    public class StatementTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var databaseMock = new Mock<IDatabase>();

            var searchPath = "-- search path";
            var sql = "SELECT * FROM \"TEST\"";
            var statement = new Statement(searchPath, sql, databaseMock.Object);
            statement.Should().NotBeNull();
            statement.SQL.Should().Be(sql);
            statement.SupportsTransaction.Should().BeTrue();
            statement.SearchPath.Should().Be(searchPath);

            sql = "ALTER TYPE \"TestType\" ADD VALUE newVal;";
            statement = new Statement(searchPath, sql, databaseMock.Object);
            statement.Should().NotBeNull();
            statement.SQL.Should().Be(sql);
            statement.SupportsTransaction.Should().BeFalse();
            statement.SearchPath.Should().Be(searchPath);
        }

        [TestMethod]
        public void ExecuteTest()
        {
            var databaseMock = new Mock<IDatabase>();

            var searchPath = "-- search path";
            string sql = "SELECT * FROM \"TEST\"";
            Statement statement = new Statement(searchPath, sql, databaseMock.Object);
            statement.Execute();
            databaseMock.Setup(d => d.ExecuteCommandNonQuery(sql)).Throws(new Exception());
            new Action(() => statement.Execute()).ShouldThrow<Exception>();
            statement.SearchPath.Should().Be(searchPath);
        }
    }
}
