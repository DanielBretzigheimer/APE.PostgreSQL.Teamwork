// <copyright file="PgType.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.Text;
using APE.PostgreSQL.Teamwork.Model.PostgresSchema;
using APE.PostgreSQL.Teamwork.Model.Utils;

namespace APE.PostgreSQL.Teamwork.Model.PostgresSchema
{
    /// <summary>
    /// Stores type information.
    /// </summary>
    /// <remarks>This also contains enumerations.</remarks>
    public class PgType // todo db Teamwork split to type and enum?
    {
        /// <summary>
        /// Creates an new instance of the <see cref="PgType"/> class.
        /// </summary>
        /// <param name="name">The name of the given type.</param>
        /// <param name="isEnum">A <see cref="bool"/> indicating if this type is a enum.</param>
        public PgType(string name, bool isEnum)
        {
            this.Name = name;
            this.IsEnum = isEnum;
            this.AttributeArguments = new List<PgArgument>();
            this.EnumEntries = new List<string>();
        }

        /// <summary>
        /// Gets or sets the name of the <see cref="PgType"/>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a list of all enum entries.
        /// </summary>
        public List<string> EnumEntries { get; set; }

        /// <summary>
        /// Gets a <see cref="bool"/> indicating if this <see cref="PgType"/> is an enum.
        /// </summary>
        public bool IsEnum { get; private set; }

        /// <summary>
        /// Must not be set if <see cref="IsEnum"/> is true.
        /// </summary>
        public List<PgArgument> AttributeArguments { get; set; }

        /// <summary>
        /// Gets the SQL as string which is used to create this <see cref="PgType"/>.
        /// </summary>
        public string CreationSQL
        {
            get
            {
                StringBuilder createString = new StringBuilder(500);
                createString.Append("CREATE TYPE ");
                createString.Append(PgDiffStringExtension.QuoteName(this.Name));
                createString.Append(" AS ");

                if (!this.IsEnum)
                {
                    createString.AppendLine("(");

                    bool first = true;
                    foreach (var argument in this.AttributeArguments)
                    {
                        if (!first)
                            createString.Append(", ");

                        createString.Append(argument.Full);
                        first = false;
                    }
                }
                else
                {
                    createString.Append("ENUM");
                    createString.AppendLine("(");

                    bool first = true;
                    foreach (string entry in this.EnumEntries)
                    {
                        if (!first)
                            createString.Append(",");
                        createString.AppendLine(entry);
                        first = false;
                    }
                }

                createString.AppendLine(");");
                return createString.ToString();
            }
        }

        /// <summary>
        /// Gets the SQL as string which can be used to drop this <see cref="PgType"/>.
        /// </summary>
        /// <remarks>This only works when there are no relations for this enum.</remarks>
        public string DropSQL
        {
            get
            {
                StringBuilder dropString = new StringBuilder(100);
                dropString.AppendLine("-- Droping Enum (Type) doesn't work when there are relationships");
                dropString.Append("DROP TYPE ");
                dropString.Append(PgDiffStringExtension.QuoteName(this.Name));
                dropString.Append(";");

                return dropString.ToString();
            }
        }

        /// <summary>
        /// Gets the signature of this <see cref="PgType"/>.
        /// </summary>
        public string Signature
        {
            get
            {
                StringBuilder signature = new StringBuilder(100);
                signature.Append(this.Name);
                signature.Append('(');

                bool first = true;
                foreach (var argument in this.AttributeArguments)
                {
                    if (!first)
                        signature.Append(',');

                    signature.Append(argument.Full);
                    first = false;
                }

                signature.Append(") (");

                first = true;
                foreach (var entry in this.EnumEntries)
                {
                    if (!first)
                        signature.Append(',');

                    signature.Append(entry);
                    first = false;
                }

                signature.Append(')');

                return signature.ToString();
            }
        }

        /// <summary>
        /// Gets the SQL which can be used to alter the <see cref="PgType"/> and adds the given value to it.
        /// </summary>
        /// <param name="newValue">Value which is added to the <see cref="PgType"/>.</param>
        /// <returns>The SQL which can alter the <see cref="PgType"/>.</returns>
        public string AlterSQL(string newValue)
        {
            StringBuilder alterString = new StringBuilder(100);
            alterString.AppendLine("-- Add value only if it does not exist because if the undo diff is called the value wont be removed (not possible in postgres 9.4 without dropping the whole enum)");
            alterString.Append("ALTER TYPE ");
            alterString.Append(PgDiffStringExtension.QuoteName(this.Name));
            alterString.Append(" ADD VALUE IF NOT EXISTS ");
            alterString.Append(newValue);
            alterString.Append(";");

            return alterString.ToString();
        }
    }
}