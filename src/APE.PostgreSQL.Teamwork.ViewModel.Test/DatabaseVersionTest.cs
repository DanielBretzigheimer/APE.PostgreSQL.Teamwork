// <copyright file="DatabaseVersionTest.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using APE.PostgreSQL.Teamwork.Model;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace APE.PostgreSQL.Teamwork.ViewModel.Test
{
	[TestClass]
	public class DatabaseVersionTest
	{
		[TestMethod]
		public void ConstructorTest()
		{
			DatabaseVersion dv = new DatabaseVersion("\\Test\\Path\\0001.diff.sql");
			dv.Should().NotBeNull();
			dv.Main.Should().Be(1);
			dv.Minor.Should().Be(string.Empty);
			dv.Full.Should().Be("0001");

			dv = new DatabaseVersion("\\Test\\Path\\8534.a.diff.sql");
			dv.Should().NotBeNull();
			dv.Main.Should().Be(8534);
			dv.Minor.Should().Be(".a");
			dv.Full.Should().Be("8534.a");

			dv = new DatabaseVersion("\\Test0002\\Path\\0008.diff.sql");
			dv.Should().NotBeNull();
			dv.Main.Should().Be(8);
			dv.Minor.Should().Be(string.Empty);
			dv.Full.Should().Be("0008");

			new Action(() => new DatabaseVersion(string.Empty)).ShouldThrow<ArgumentException>();
		}

        [TestMethod]
        public void OperatorTest()
        {
            var dv1 = new DatabaseVersion("\\0001.diff.sql");
            var dv1a = new DatabaseVersion("\\0001.a.diff.sql");
            var dv1b = new DatabaseVersion("\\0001.b.diff.sql");
            var dv2 = new DatabaseVersion("\\0002.diff.sql");
            var dv2a = new DatabaseVersion("\\0002.a.diff.sql");
            var dv3 = new DatabaseVersion("\\0003.diff.sql");
            var dv3alternative = new DatabaseVersion("\\0003.diff.sql");

            (dv1 < dv1a).Should().BeTrue();
            (dv1a < dv1b).Should().BeTrue();
            (dv1b < dv2).Should().BeTrue();
            (dv1 < dv2).Should().BeTrue();
            (dv2 > dv1).Should().BeTrue();
            (dv2 > dv1a).Should().BeTrue();
            (dv2 > dv1b).Should().BeTrue();
            (dv2 < dv3).Should().BeTrue();
            (dv2a < dv3).Should().BeTrue();
            (dv2a > dv2).Should().BeTrue();

            (dv1 != dv1a).Should().BeTrue();
            (dv2 != dv1).Should().BeTrue();
            (dv3 == dv3alternative).Should().BeTrue();
        }
	}
}
