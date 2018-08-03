// <copyright file="PgConstraint.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Text;
using System.Text.RegularExpressions;
using APE.PostgreSQL.Teamwork.Model.Utils;

namespace APE.PostgreSQL.Teamwork.Model.PostgresSchema
{
    /// <summary>
    /// Stores table constraint information.
    /// </summary>
    public class PgConstraint
    {
        /// <summary>
        /// Regex for checking whether the constraint is PRIMARY KEY constraint.
        /// </summary>
        private static readonly Regex PatternPrimaryKey = new Regex(".*PRIMARY[\\s]+KEY.*");
        private static readonly Regex PatternForeignKey = new Regex(".*FOREIGN[\\s]+KEY.*");

        /// <summary>
        /// Creates a new <see cref="PgConstraint"/> object.
        /// </summary>
        /// <param name="name">The name of the <see cref="PgConstraint"/>.</param>
        public PgConstraint(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Creates and returns SQL for creation of the constraint.
        /// </summary>
        /// <returns> created SQL.</returns>
        public string CreationSQL
        {
            get
            {
                var creationSql = new StringBuilder(100);
                creationSql.Append("ALTER TABLE ");
                creationSql.Append(PgDiffStringExtension.QuoteName(this.TableName));
                creationSql.Append("\n\tADD CONSTRAINT ");
                creationSql.Append(PgDiffStringExtension.QuoteName(this.Name));
                creationSql.Append(' ');
                creationSql.Append(this.Definition);
                creationSql.Append(';');

                if (this.Comment != null && this.Comment.Length > 0)
                {
                    creationSql.Append("\n\nCOMMENT ON CONSTRAINT ");
                    creationSql.Append(PgDiffStringExtension.QuoteName(this.Name));
                    creationSql.Append(" ON ");
                    creationSql.Append(PgDiffStringExtension.QuoteName(this.TableName));
                    creationSql.Append(" IS ");
                    creationSql.Append(this.Comment);
                    creationSql.Append(';');
                }

                return creationSql.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the comment of the <see cref="PgConstraint"/>.
        /// </summary>
        [NullGuard.AllowNull]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the definition of the <see cref="PgConstraint"/>.
        /// </summary>
        public string Definition { get; set; }

        /// <summary>
        /// Creates and returns SQL for dropping the constraint.
        /// </summary>
        public string DropSQL
        {
            get
            {
                var dropSql = new StringBuilder(100);
                dropSql.Append("ALTER TABLE ");
                dropSql.AppendLine(PgDiffStringExtension.QuoteName(this.TableName));
                dropSql.Append("\tDROP CONSTRAINT ");
                dropSql.Append(PgDiffStringExtension.QuoteName(this.Name));
                dropSql.Append(';');

                return dropSql.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the name of the <see cref="PgConstraint"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Returns true if this is a PRIMARY KEY constraint, otherwise false.
        /// </summary>
        public bool PrimaryKeyConstraint
        {
            get
            {
                return PatternPrimaryKey.Matches(this.Definition).Count != 0;
            }
        }

        /// <summary>
        /// Returns true if this is a foreign key constraint, otherwise false.
        /// </summary>
        public bool ForeignKeyConstraint
        {
            get
            {
                return PatternForeignKey.Matches(this.Definition).Count != 0;
            }
        }

        /// <summary>
        /// Gets or sets the name of the table to which this <see cref="PgConstraint"/> belongs.
        /// </summary>
        public string TableName { get; set; }

        public string RenameToSql(string newName)
        {
            var renameSql = new StringBuilder("ALTER TABLE ");
            renameSql.Append(PgDiffStringExtension.QuoteName(this.TableName));
            renameSql.Append("\tRENAME CONSTRAINT ");
            renameSql.Append(PgDiffStringExtension.QuoteName(this.Name));
            renameSql.Append(" TO ");
            renameSql.Append(PgDiffStringExtension.QuoteName(newName));
            renameSql.Append(';');

            return renameSql.ToString();
        }

        public bool IsRenamed(PgConstraint constraint)
        {
            if (this.Definition.Equals(constraint.Definition)
                && this.TableName.Equals(constraint.TableName))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        public override bool Equals([NullGuard.AllowNull] object obj)
        {
            if (this == obj)
                return true;

            if (obj is PgConstraint constraint)
            {
                return this.Definition.Equals(constraint.Definition)
                    && this.Name.Equals(constraint.Name)
                    && this.TableName.Equals(constraint.TableName);
            }

            return false;
        }

        [return: NullGuard.AllowNull]
        public override string ToString()
        {
            return $"{this.GetType().Name} {this.Name} for {this.TableName}";
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return (this.GetType().FullName + "|" + this.Definition + "|" + this.Name + "|" + this.TableName).GetHashCode();
        }
    }
}