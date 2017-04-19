// <copyright file="PgIndex.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Text;
using APE.PostgreSQL.Teamwork.Model.Utils;

namespace APE.PostgreSQL.Teamwork.Model.PostgresSchema
{
    /// <summary>
    /// Stores table index information.
    /// </summary>
    public class PgIndex
    {
        /// <summary>
        /// Creates a new <see cref="PgIndex"/> object.
        /// </summary>
        /// <param name="name">The name of the <see cref="PgIndex"/></param>
        public PgIndex(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets the comment of the <see cref="PgIndex"/>.
        /// </summary>
        [NullGuard.AllowNull]
        public string Comment { get; set; }

        /// <summary>
        /// Creates and returns SQL for creation of the index.
        /// </summary>
        /// <returns> created SQL </returns>
        public string CreationSQL
        {
            get
            {
                var creationSql = new StringBuilder(100);
                creationSql.Append("CREATE ");

                if (this.Unique)
                {
                    creationSql.Append("UNIQUE ");
                }

                creationSql.Append("INDEX ");
                creationSql.Append(PgDiffStringExtension.QuoteName(this.Name));
                creationSql.Append(" ON ");
                creationSql.Append(PgDiffStringExtension.QuoteName(this.TableName));
                creationSql.Append(' ');
                creationSql.Append(this.Definition);
                creationSql.Append(';');

                if (this.Comment != null && this.Comment.Length > 0)
                {
                    creationSql.Append("\n\nCOMMENT ON INDEX ");
                    creationSql.Append(PgDiffStringExtension.QuoteName(this.Name));
                    creationSql.Append(" IS ");
                    creationSql.Append(this.Comment);
                    creationSql.Append(';');
                }

                return creationSql.ToString();
            }
        }

        /// <summary>
        /// Creates and returns SQL statement for dropping the index.
        /// </summary>
        /// <returns> created SQL statement </returns>
        public string DropSQL
        {
            get
            {
                return "DROP INDEX " + PgDiffStringExtension.QuoteName(this.Name) + ";";
            }
        }

        /// <summary>
        /// Gets or sets the definition of the <see cref="PgIndex"/>.
        /// </summary>
        public string Definition { get; set; }

        /// <summary>
        /// Gets or sets the name of the <see cref="PgIndex"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the table of the <see cref="PgIndex"/>.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> which indicates if the <see cref="PgIndex"/> is unique.
        /// </summary>
        public bool Unique { get; set; }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        public override bool Equals([NullGuard.AllowNull] object obj)
        {
            var equals = false;

            if (this == obj)
            {
                equals = true;
            }
            else if (obj is PgIndex index)
            {
                equals = this.Definition.Equals(index.Definition) && this.Name.Equals(index.Name) && this.TableName.Equals(index.TableName) && this.Unique == index.Unique;
            }

            return equals;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return (this.GetType().FullName + "|" + this.Definition + "|" + this.Name + "|" + this.TableName + "|" + this.Unique).GetHashCode();
        }
    }
}