// <copyright file="PgSequence.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Text;
using APE.PostgreSQL.Teamwork.Model.Utils;

namespace APE.PostgreSQL.Teamwork.Model.PostgresSchema
{
    /// <summary>
    /// Stores sequence information.
    /// </summary>
    public class PgSequence
    {
        /// <summary>
        /// Creates a new <see cref="PgSequence"/> object.
        /// </summary>
        public PgSequence(string name) => this.Name = name;

        /// <summary>
        /// Gets or sets the cache of the <see cref="PgSequence"/>.
        /// </summary>
        public string? Cache { get; set; }

        /// <summary>
        /// Gets or sets the comment of the <see cref="PgSequence"/>.
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Creates and returns SQL statement for creation of the sequence.
        /// </summary>
        /// <returns> created SQL statement. </returns>
        public string CreationSQL
        {
            get
            {
                var createSequenceSQL = new StringBuilder(100);
                createSequenceSQL.Append("CREATE SEQUENCE ");
                createSequenceSQL.Append(PgDiffStringExtension.QuoteName(this.Name));

                if (this.StartWith != null)
                {
                    createSequenceSQL.Append("\n\tSTART WITH ");
                    createSequenceSQL.Append(this.StartWith);
                }

                if (this.Increment != null)
                {
                    createSequenceSQL.Append("\n\tINCREMENT BY ");
                    createSequenceSQL.Append(this.Increment);
                }

                createSequenceSQL.Append("\n\t");

                if (this.MaxValue == null)
                {
                    createSequenceSQL.Append("NO MAXVALUE");
                }
                else
                {
                    createSequenceSQL.Append("MAXVALUE ");
                    createSequenceSQL.Append(this.MaxValue);
                }

                createSequenceSQL.Append("\n\t");

                if (this.MinValue == null)
                {
                    createSequenceSQL.Append("NO MINVALUE");
                }
                else
                {
                    createSequenceSQL.Append("MINVALUE ");
                    createSequenceSQL.Append(this.MinValue);
                }

                if (this.Cache != null)
                {
                    createSequenceSQL.Append("\n\tCACHE ");
                    createSequenceSQL.Append(this.Cache);
                }

                if (this.Cycle)
                {
                    createSequenceSQL.Append("\n\tCYCLE");
                }

                createSequenceSQL.Append(';');

                if (this.Comment != null && this.Comment.Length > 0)
                {
                    createSequenceSQL.Append("\n\nCOMMENT ON SEQUENCE ");
                    createSequenceSQL.Append(PgDiffStringExtension.QuoteName(this.Name));
                    createSequenceSQL.Append(" IS ");
                    createSequenceSQL.Append(this.Comment);
                    createSequenceSQL.Append(';');
                }

                return createSequenceSQL.ToString();
            }
        }

        /// <summary>
        /// Creates and returns SQL statement for modification "OWNED BY" parameter.
        /// </summary>
        /// <returns> created SQL statement. </returns>
        public string OwnedBySQL
        {
            get
            {
                var ownedBySql = new StringBuilder(100);

                ownedBySql.Append("ALTER SEQUENCE ");
                ownedBySql.Append(PgDiffStringExtension.QuoteName(this.Name));

                if (this.Owner != null && this.Owner.Length > 0)
                {
                    ownedBySql.Append("\n\tOWNED BY ");
                    ownedBySql.Append(this.Owner);
                }

                ownedBySql.Append(';');

                return ownedBySql.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the cycle of the <see cref="PgSequence"/>.
        /// </summary>
        public bool Cycle { get; set; }

        /// <summary>
        /// Creates and returns SQL statement for dropping the sequence.
        /// </summary>
        /// <returns> created SQL. </returns>
        public string DropSQL => "DROP SEQUENCE IF EXISTS " + PgDiffStringExtension.QuoteName(this.Name) + ";";

        /// <summary>
        /// Gets or sets the increment of the <see cref="PgSequence"/>.
        /// </summary>
        public string? Increment { get; set; }

        /// <summary>
        /// Gets or sets the max value of the <see cref="PgSequence"/>.
        /// </summary>
        public string? MaxValue { get; set; }

        /// <summary>
        /// Gets or sets the min value of the <see cref="PgSequence"/>.
        /// </summary>
        public string? MinValue { get; set; }

        /// <summary>
        /// Gets or sets the name of the <see cref="PgSequence"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the start value of the <see cref="PgSequence"/>.
        /// </summary>
        public string? StartWith { get; set; }

        /// <summary>
        /// Gets or sets the owner of the <see cref="PgSequence"/>.
        /// </summary>
        public string? Owner { get; set; }
    }
}