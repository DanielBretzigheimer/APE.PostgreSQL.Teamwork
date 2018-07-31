// <copyright file="PgPrivilege.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema.Enums;

namespace APE.PostgreSQL.Teamwork.Model.PostgresSchema
{
    /// <summary>
    /// Stores information about an postgres privilege.
    /// </summary>
    public class PgPrivilege
    {
        /// <summary>
        /// Gets or sets the command which can be GRANT or REVOKE.
        /// </summary>
        public PgPrivilegeCommand Command { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="PgPrivilegeKind"/>
        /// </summary>
        public PgPrivilegeKind Privilege { get; set; }

        /// <summary>
        /// Gets or sets the postgres name to which the <see cref="PgPrivilege"/> is applied to.
        /// </summary>
        public string OnName { get; set; }

        /// <summary>
        /// Gets or sets the postgres type to which the <see cref="PgPrivilege"/> is applied to.
        /// </summary>
        public string OnType { get; set; }

        /// <summary>
        /// Gets or sets the role for which the <see cref="PgPrivilege"/> is applied.
        /// </summary>
        public string Role { get; set; }

        public string Create()
        {
            var command = this.Command.ToString().ToUpper();
            var privilege = this.Privilege.ToString().ToUpper();
            var tofrom = this.Command == PgPrivilegeCommand.Grant
                ? "TO"
                : "FROM";

            return $"{command} {privilege} ON {this.OnType} {this.OnName} {tofrom} {this.Role};";
        }

        public string CreateRevert()
        {
            var revertedCommand = this.Command == PgPrivilegeCommand.Grant
                ? PgPrivilegeCommand.Revoke
                : PgPrivilegeCommand.Grant;

            var command = revertedCommand.ToString().ToUpper();
            var privilege = this.Privilege.ToString().ToUpper();
            var tofrom = revertedCommand == PgPrivilegeCommand.Grant
                ? "TO"
                : "FROM";

            return $"{command} {privilege} ON {this.OnType} {this.OnName} {tofrom} {this.Role};";
        }

        public override bool Equals([NullGuard.AllowNull] object obj)
        {
            if (obj == null || obj.GetType() != typeof(PgPrivilege))
            {
                return false;
            }

            var other = (PgPrivilege)obj;
            return this.Command == other.Command
                && this.Privilege == other.Privilege
                && this.OnName == other.OnName
                && this.OnType == other.OnType
                && this.Role == other.Role;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [return: NullGuard.AllowNull]
        public override string ToString()
        {
            return $"{this.GetType().Name} {this.Create()}";
        }
    }
}
