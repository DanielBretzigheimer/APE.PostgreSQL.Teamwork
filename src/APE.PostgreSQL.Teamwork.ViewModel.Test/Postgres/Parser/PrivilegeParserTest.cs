using System;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema.Enums;
using APE.PostgreSQL.Teamwork.ViewModel.Postgres.Parsers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace APE.PostgreSQL.Teamwork.ViewModel.Test.Postgres.Parser
{
    [TestClass]
    public class PrivilegeParserTest
    {
        [TestMethod]
        public void GrantParseTest()
        {
            var db = new PgDatabase("Name");
            db.DefaultSchema.Privileges.Count.Should().Be(0);
            PrivilegeParser.Parse(db, "GRANT ALL ON FUNCTION generate_csharp_pocos() TO postgres;", PgPrivilegeCommand.Grant);
            db.DefaultSchema.Privileges.Count.Should().Be(1);
            var priv = db.DefaultSchema.Privileges[0];
            priv.Command.Should().Be(PgPrivilegeCommand.Grant);
            priv.Privilege.Should().Be(PgPrivilegeKind.All);
            priv.Role.Should().Be("postgres");
            priv.OnType.Should().Be("FUNCTION");
            priv.OnName.Should().Be("generate_csharp_pocos()");

            PrivilegeParser.Parse(db, "GRANT ALL ON FUNCTION generate_ids(sequence_name text, count integer) TO postgres;", PgPrivilegeCommand.Grant);
            db.DefaultSchema.Privileges.Count.Should().Be(2);
            priv = db.DefaultSchema.Privileges[1];
            priv.Command.Should().Be(PgPrivilegeCommand.Grant);
            priv.Privilege.Should().Be(PgPrivilegeKind.All);
            priv.Role.Should().Be("postgres");
            priv.OnType.Should().Be("FUNCTION");
            priv.OnName.Should().Be("generate_ids(sequence_name text, count integer)");

            PrivilegeParser.Parse(db, "GRANT ALL ON FUNCTION postgrestype_to_csharptype(pg_type text) TO postgres;", PgPrivilegeCommand.Grant);
            db.DefaultSchema.Privileges.Count.Should().Be(3);
            priv = db.DefaultSchema.Privileges[2];
            priv.Command.Should().Be(PgPrivilegeCommand.Grant);
            priv.Privilege.Should().Be(PgPrivilegeKind.All);
            priv.Role.Should().Be("postgres");
            priv.OnType.Should().Be("FUNCTION");
            priv.OnName.Should().Be("postgrestype_to_csharptype(pg_type text)");
        }

        [TestMethod]
        public void RevokeParseTest()
        {
            var db = new PgDatabase("Name");
            db.DefaultSchema.Privileges.Count.Should().Be(0);
            PrivilegeParser.Parse(db, "REVOKE ALL ON FUNCTION generate_csharp_pocos() FROM PUBLIC;", PgPrivilegeCommand.Revoke);
            db.DefaultSchema.Privileges.Count.Should().Be(1);
            var priv = db.DefaultSchema.Privileges[0];
            priv.Command.Should().Be(PgPrivilegeCommand.Revoke);
            priv.Privilege.Should().Be(PgPrivilegeKind.All);
            priv.Role.Should().Be("public");
            priv.OnType.Should().Be("FUNCTION");
            priv.OnName.Should().Be("generate_csharp_pocos()");

            PrivilegeParser.Parse(db, "REVOKE ALL ON FUNCTION generate_ids(sequence_name text, count integer) FROM PUBLIC;", PgPrivilegeCommand.Revoke);
            db.DefaultSchema.Privileges.Count.Should().Be(2);
            priv = db.DefaultSchema.Privileges[1];
            priv.Command.Should().Be(PgPrivilegeCommand.Revoke);
            priv.Privilege.Should().Be(PgPrivilegeKind.All);
            priv.Role.Should().Be("public");
            priv.OnType.Should().Be("FUNCTION");
            priv.OnName.Should().Be("generate_ids(sequence_name text, count integer)");

            PrivilegeParser.Parse(db, "REVOKE ALL ON FUNCTION postgrestype_to_csharptype(pg_type text) FROM postgres;", PgPrivilegeCommand.Revoke);
            db.DefaultSchema.Privileges.Count.Should().Be(3);
            priv = db.DefaultSchema.Privileges[2];
            priv.Command.Should().Be(PgPrivilegeCommand.Revoke);
            priv.Privilege.Should().Be(PgPrivilegeKind.All);
            priv.Role.Should().Be("postgres");
            priv.OnType.Should().Be("FUNCTION");
            priv.OnName.Should().Be("postgrestype_to_csharptype(pg_type text)");
        }
    }
}
