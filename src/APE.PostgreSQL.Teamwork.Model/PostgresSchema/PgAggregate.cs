// <copyright file="PgAggregate.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Text;
using APE.PostgreSQL.Teamwork.Model.Utils;

namespace APE.PostgreSQL.Teamwork.Model.PostgresSchema
{
    /// <summary>
    /// C# class for the Postgres Aggregate.
    /// </summary>
    public class PgAggregate
    {
        /// <summary>
        /// Creates a new instance of the <see cref="PgAggregate"/> class.
        /// </summary>
        public PgAggregate() => this.Arguments = new List<Argument>();

        /// <summary>
        /// Returns creation SQL of the aggregate.
        /// </summary>
        /// <returns> creation SQL.</returns>
        public string CreationSQL
        {
            get
            {
                var creationSql = new StringBuilder(500);

                creationSql.AppendLine();
                creationSql.Append("CREATE AGGREGATE ");
                creationSql.Append(PgDiffStringExtension.QuoteName(this.Name));
                creationSql.Append('(');

                var addComma = false;

                foreach (var argument in this.Arguments)
                {
                    if (addComma)
                    {
                        creationSql.Append(", ");
                    }

                    creationSql.Append(argument.DataType);

                    addComma = true;
                }

                creationSql.Append(") ");
                creationSql.Append(this.Body);
                creationSql.Append(';');

                if (this.Comment != null && this.Comment.Length > 0)
                {
                    creationSql.Append("\n\nCOMMENT ON AGGREGATE ");
                    creationSql.Append(PgDiffStringExtension.QuoteName(this.Name));
                    creationSql.Append('(');

                    addComma = false;

                    foreach (var argument in this.Arguments)
                    {
                        if (addComma)
                        {
                            creationSql.Append(", ");
                        }

                        creationSql.Append(argument.DataType);

                        addComma = true;
                    }

                    creationSql.Append(") IS ");
                    creationSql.Append(this.Comment);
                    creationSql.Append(';');
                }

                return creationSql.ToString();
            }
        }

        /// <summary>
        /// Creates and returns SQL for dropping the aggregate.
        /// </summary>
        /// <returns> created SQL. </returns>
        public string DropSQL
        {
            get
            {
                var dropSql = new StringBuilder(100);
                dropSql.Append("DROP AGGREGATE IF EXISTS ");
                dropSql.Append(PgDiffStringExtension.QuoteName(this.Name));
                dropSql.Append('(');

                var addComma = false;
                foreach (var argument in this.Arguments)
                {
                    if (addComma)
                    {
                        dropSql.Append(", ");
                    }

                    dropSql.Append(argument.DataType);
                    addComma = true;
                }

                dropSql.Append(");");
                return dropSql.ToString();
            }
        }

        /// <summary>
        /// Gets the comment.
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Gets the whole definition of the aggregate from RETURNS keyword.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the name of the aggregate including argument types.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets a <see cref="IList{Argument}"/> of this <see cref="PgAggregate"/>.
        /// </summary>
        public IList<Argument> Arguments { get; private set; }

        /// <summary>
        /// Returns aggregate signature. It consists of unquoted name and argument
        /// data types.
        /// </summary>
        /// <returns> aggregate signature. </returns>
        public string Signature
        {
            get
            {
                var signature = new StringBuilder(100);
                signature.Append(this.Name);
                signature.Append('(');

                var addComma = false;

                foreach (var argument in this.Arguments)
                {
                    if (addComma)
                        signature.Append(',');

                    signature.Append(argument.DataType?.ToLowerInvariant() ?? string.Empty);

                    addComma = true;
                }

                signature.Append(')');

                return signature.ToString();
            }
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is not PgAggregate)
                return false;
            else if (obj == this)
                return true;

            return this.Equals(obj, false);
        }

        /// <summary>
        /// Compares two objects whether they are equal. If both objects are of the same class but they equal just in whitespace in
        /// there body, they are considered being equal.
        /// </summary>
        /// <param name="obj">Object to be compared.</param>
        /// <param name="ignoreAggregateWhitespace">Whether multiple whitespaces in aggregate.</param>
        /// <returns>
        /// True if the given <see cref="object"/> is a <see cref="PgAggregate"/> and its code is the same when compared ignoring whitespace, otherwise returns false.
        /// </returns>
        public bool Equals(object obj, bool ignoreAggregateWhitespace)
        {
            if (this == obj)
            {
                return true;
            }
            else if (obj is PgAggregate aggregate)
            {
                if ((this.Name == null && aggregate.Name != null)
                    || (this.Name != null && !this.Name.Equals(aggregate.Name)))
                {
                    return false;
                }

                string body;
                string otherBody;

                if (ignoreAggregateWhitespace)
                {
                    body = this.Body.Replace("\\s+", " ");
                    otherBody = aggregate.Body.Replace("\\s+", " ");
                }
                else
                {
                    body = this.Body;
                    otherBody = aggregate.Body;
                }

                if ((body == null && otherBody != null) || (body != null && !body.Equals(otherBody)))
                    return false;

                if (this.Arguments.Count != aggregate.Arguments.Count)
                {
                    return false;
                }
                else
                {
                    for (var i = 0; i < this.Arguments.Count; i++)
                    {
                        if (!this.Arguments[i].Equals(aggregate.Arguments[i]))
                            return false;
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hashCode = new StringBuilder(500);
            hashCode.Append(this.Body);
            hashCode.Append('|');
            hashCode.Append(this.Name);

            foreach (var argument in this.Arguments)
            {
                hashCode.Append('|');
                hashCode.Append(argument.DataType);
            }

            return hashCode.ToString().GetHashCode();
        }

        /// <summary>
        /// Aggregate argument information.
        /// </summary>
        public class Argument
        {
            /// <summary>
            /// Gets or sets the <see cref="DataType"/> of this <see cref="Argument"/>.
            /// </summary>
            public string? DataType { get; set; }

            /// <summary>
            /// Determines whether the specified object is equal to the current object.
            /// </summary>
            public override bool Equals(object? obj)
            {
                if (obj is not Argument)
                    return false;
                else if (this == obj)
                    return true;

                var argument = (Argument)obj;

                return this.DataType == null ?
                    argument.DataType == null :
                    this.DataType.Equals(argument.DataType, StringComparison.CurrentCultureIgnoreCase);
            }

            /// <summary>
            /// Serves as the default hash function.
            /// </summary>
            /// <returns>A hash code for the current object.</returns>
            public override int GetHashCode()
            {
                var hashCode = new StringBuilder(50);
                hashCode.Append(this.DataType?.ToUpper(new System.Globalization.CultureInfo("en")));
                return hashCode.ToString().GetHashCode();
            }
        }
    }
}
