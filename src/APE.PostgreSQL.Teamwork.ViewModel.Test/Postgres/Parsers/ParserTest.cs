// <copyright file="ParserTest.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>

using APE.PostgreSQL.Teamwork.ViewModel.Postgres.Parsers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace APE.PostgreSQL.Teamwork.ViewModel.Test.Postgres.Parsers
{
    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void ParseIdentifierTest()
        {
            var commentSql1 = "COMMENT ON COLUMN \"Supplier\".\"PrimaryContactId\" IS 'Gets or sets the Id of the primary contact.';";
            var parser = new Parser(commentSql1);
            parser.Expect("COMMENT", "ON", "COLUMN");
            parser.ParseIdentifier().Should().Be("\"Supplier\".\"PrimaryContactId\"");

            var commentSql2 = "COMMENT ON COLUMN public.\"Supplier\".\"PrimaryContactId\" IS 'Gets or sets the Id of the primary contact.';";
            parser = new Parser(commentSql2);
            parser.Expect("COMMENT", "ON", "COLUMN");
            parser.ParseIdentifier().Should().Be("public.\"Supplier\".\"PrimaryContactId\"");
        }
    }
}
