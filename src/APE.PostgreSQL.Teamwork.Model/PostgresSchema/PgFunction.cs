// <copyright file="PgFunction.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Collections.ObjectModel;
using System.Text;
using APE.PostgreSQL.Teamwork.Model.Utils;

namespace APE.PostgreSQL.Teamwork.Model.PostgresSchema
{
    /// <summary>
    /// Stores function information.
    /// </summary>
    public class PgFunction
    {
        /// <summary>
        /// List of arguments of the <see cref="PgFunction"/>.
        /// </summary>
        private readonly IList<Argument> arguments = new List<Argument>();

        public PgFunction(string name)
            => this.Name = name;

        /// <summary>
        /// Gets or sets the comment of the <see cref="PgFunction"/>.
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Gets the creation SQL for this <see cref="PgFunction"/>.
        /// </summary>
        public string CreationSQL
        {
            get
            {
                var creationSql = new StringBuilder(500);

                // drop the function if it exists to avoid errors
                creationSql.Append("DROP FUNCTION IF EXISTS ");
                creationSql.Append(PgDiffStringExtension.QuoteName(this.Name));
                creationSql.Append('(');
                var addComma = false;

                foreach (var argument in this.arguments)
                {
                    if (addComma)
                        creationSql.Append(", ");
                    else
                        addComma = true;

                    creationSql.Append(argument.DataType);
                }

                creationSql.Append(");");

                creationSql.AppendLine();
                creationSql.Append("CREATE OR REPLACE FUNCTION ");
                creationSql.Append(PgDiffStringExtension.QuoteName(this.Name));
                creationSql.Append('(');

                addComma = false;

                foreach (var argument in this.arguments)
                {
                    if (addComma)
                        creationSql.Append(", ");

                    creationSql.Append(argument.GetDeclaration(true));

                    addComma = true;
                }

                creationSql.Append(") ");
                creationSql.Append(this.Body);
                creationSql.Append(';');

                if (this.Comment != null && this.Comment.Length > 0)
                {
                    creationSql.Append("\n\nCOMMENT ON FUNCTION ");
                    creationSql.Append(PgDiffStringExtension.QuoteName(this.Name));
                    creationSql.Append('(');

                    addComma = false;

                    foreach (var argument in this.arguments)
                    {
                        if (addComma)
                            creationSql.Append(", ");

                        creationSql.Append(argument.GetDeclaration(false));

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
        /// Gets or sets the body of the <see cref="PgFunction"/>.
        /// </summary>
        public string? Body { get; set; }

        /// <summary>
        /// Creates and returns SQL for dropping the function.
        /// </summary>
        /// <returns> created SQL.</returns>
        public string DropSQL
        {
            get
            {
                var dropSql = new StringBuilder(100);
                dropSql.Append("DROP FUNCTION IF EXISTS ");
                dropSql.Append(PgDiffStringExtension.QuoteName(this.Name));
                dropSql.Append('(');

                var addComma = false;

                foreach (var argument in this.arguments)
                {
                    if ("OUT".Equals(argument.Mode, StringComparison.CurrentCultureIgnoreCase))
                        continue;

                    if (addComma)
                        dropSql.Append(", ");

                    dropSql.Append(argument.DataType);
                    addComma = true;
                }

                dropSql.Append(");");

                return dropSql.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the name of the <see cref="PgFunction"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets a list of all <see cref="Argument"/>s for this <see cref="PgFunction"/>.
        /// </summary>
        public IList<Argument> Arguments => new ReadOnlyCollection<Argument>(this.arguments);

        /// <summary>
        /// Returns function signature. It consists of unquoted name and argument
        /// data types.
        /// </summary>
        /// <returns> function signature. </returns>
        public string Signature
        {
            get
            {
                var signature = new StringBuilder(100);
                signature.Append(this.Name);
                signature.Append('(');

                var addComma = false;

                foreach (var argument in this.arguments)
                {
                    if ("OUT".Equals(argument.Mode, StringComparison.CurrentCultureIgnoreCase))
                        continue;

                    if (addComma)
                        signature.Append(',');

                    signature.Append(argument.DataType?.ToLowerInvariant());

                    addComma = true;
                }

                signature.Append(')');

                return signature.ToString();
            }
        }

        /// <summary>
        /// Adds argument to the list of arguments.
        /// </summary>
        public void AddArgument(Argument argument) => this.arguments.Add(argument);

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is not PgFunction)
                return false;
            else if (obj == this)
                return true;

            return this.Equals(obj, false);
        }

        /// <summary>
        /// Compares two objects whether they are equal. If both objects are of the same class but they equal just in whitespace in
        /// <see cref="Body"/>, they are considered being equal.
        /// </summary>
        /// <param name="obj">Object to be compared.</param>
        /// <param name="ignoreFunctionWhitespace">Whether multiple whitespaces in function <see cref="Body"/> should be ignored.</param>
        /// <returns>True if given <see cref="object"/> is <see cref="PgFunction"/> and its code is the same when compared ignoring whitespace, otherwise returns false.</returns>
        public bool Equals(object obj, bool ignoreFunctionWhitespace)
        {
            var equals = false;

            if (this == obj)
            {
                equals = true;
            }
            else if (obj is PgFunction function)
            {
                if ((this.Name == null && function.Name != null) || (this.Name != null && !this.Name.Equals(function.Name)))
                    return false;

                string? thisBody;
                string? thatBody;

                if (ignoreFunctionWhitespace)
                {
                    thisBody = this.Body?.Replace("\\s+", " ");
                    thatBody = function.Body?.Replace("\\s+", " ");
                }
                else
                {
                    thisBody = this.Body;
                    thatBody = function.Body;
                }

                if ((thisBody == null && thatBody != null) || (thisBody != null && !thisBody.Equals(thatBody)))
                    return false;

                if (this.arguments.Count != function.Arguments.Count)
                {
                    return false;
                }
                else
                {
                    for (var i = 0; i < this.arguments.Count; i++)
                    {
                        if (!this.arguments[i].Equals(function.Arguments[i]))
                            return false;
                    }
                }

                return true;
            }

            return equals;
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

            foreach (var argument in this.arguments)
            {
                hashCode.Append('|');
                hashCode.Append(argument.GetDeclaration(true));
            }

            return hashCode.ToString().GetHashCode();
        }

        /// <summary>
        /// Function argument information.
        /// </summary>
        public class Argument
        {
            /// <summary>
            /// Argument mode.
            /// </summary>
            private string mode = "IN";

            /// <summary>
            /// Gets or sets the data type of the <see cref="Argument"/>.
            /// </summary>
            public string? DataType { get; set; }

            /// <summary>
            /// Gets or sets the default expression of the <see cref="Argument"/>.
            /// </summary>
            public string? DefaultExpression { get; set; }

            /// <summary>
            /// Gets or sets the mode of the <see cref="Argument"/>.
            /// </summary>
            public string? Mode
            {
                get => this.mode;
                set => this.mode = value == null || value.Length == 0 ? "IN" : value;
            }

            /// <summary>
            /// Gets or sets the name of the <see cref="Argument"/>.
            /// </summary>
            public string? Name { get; set; }

            /// <summary>
            /// Creates argument declaration.
            /// </summary>
            /// <param name="includeDefaultValue">Whether to include default value.</param>
            public string GetDeclaration(bool includeDefaultValue)
            {
                var declarationSql = new StringBuilder(50);

                if (this.mode != null && !"IN".Equals(this.mode, StringComparison.CurrentCultureIgnoreCase))
                {
                    declarationSql.Append(this.mode);
                    declarationSql.Append(' ');
                }

                if (this.Name != null && this.Name.Length > 0)
                {
                    declarationSql.Append(PgDiffStringExtension.QuoteName(this.Name));
                    declarationSql.Append(' ');
                }

                declarationSql.Append(this.DataType);

                if (includeDefaultValue && this.DefaultExpression != null && this.DefaultExpression.Length > 0)
                {
                    declarationSql.Append(" = ");
                    declarationSql.Append(this.DefaultExpression);
                }

                return declarationSql.ToString();
            }

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
                return (this.DataType == null ? argument.DataType == null : this.DataType.Equals(argument.DataType, StringComparison.CurrentCultureIgnoreCase))
                    && (this.DefaultExpression == null ? argument.DefaultExpression == null : this.DefaultExpression.Equals(this.DefaultExpression))
                    && (this.mode == null ? argument.Mode == null : this.mode.Equals(argument.Mode, StringComparison.CurrentCultureIgnoreCase))
                    && (this.Name == null ? argument.Name == null : this.Name.Equals(argument.Name));
            }

            /// <summary>
            /// Serves as the default hash function.
            /// </summary>
            /// <returns>A hash code for the current object.</returns>
            public override int GetHashCode()
            {
                var hashCode = new StringBuilder(50);
                hashCode.Append(this.mode?.ToUpper(new System.Globalization.CultureInfo("en")));
                hashCode.Append('|');
                hashCode.Append(this.Name);
                hashCode.Append('|');
                hashCode.Append(this.DataType?.ToUpper(new System.Globalization.CultureInfo("en")));
                hashCode.Append('|');
                hashCode.Append(this.DefaultExpression);

                return hashCode.ToString().GetHashCode();
            }
        }
    }
}