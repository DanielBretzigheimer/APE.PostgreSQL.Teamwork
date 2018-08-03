// <copyright file="DatabaseVersionTest.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace APE.PostgreSQL.Teamwork.Model.Test
{
    [TestClass]
    public class DatabaseVersionTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var target = new DatabaseVersion(1);
            target.Should().NotBeNull();
            target.Main.Should().Be(1);
            target.Minor.Should().BeNullOrWhiteSpace();
        }

        [TestMethod]
        public void OperatorTest()
        {
            var version1 = new DatabaseVersion(1);
            var version2 = new DatabaseVersion(2);
            var version3 = new DatabaseVersion(3);
            var version2Alt = new DatabaseVersion(2);

            (version1 <= version2).Should().BeTrue();
            (version2 >= version1).Should().BeTrue();
            (version2 == version2Alt).Should().BeTrue();
            (version3 != version2).Should().BeTrue();
            (version2 < version3).Should().BeTrue();
            (version3 > version1).Should().BeTrue();
            (version3 <= version2).Should().BeFalse();
            (version2 >= version3).Should().BeFalse();
            (version3 == version2Alt).Should().BeFalse();
            (version2 != version2Alt).Should().BeFalse();
            (version2 < version1).Should().BeFalse();
            (version1 > version2).Should().BeFalse();
        }
    }
}
