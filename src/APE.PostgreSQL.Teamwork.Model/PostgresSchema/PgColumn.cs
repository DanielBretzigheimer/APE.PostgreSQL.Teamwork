// <copyright file="PgColumn.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Text;
using System.Text.RegularExpressions;
using APE.PostgreSQL.Teamwork.Model.Utils;

namespace APE.PostgreSQL.Teamwork.Model.PostgresSchema
{
    /// <summary>
    /// Stores column information.
    /// </summary>
    public class PgColumn
    {
        /// <summary>
        /// Regex for parsing NULL arguments.
        /// </summary>
        private static readonly Regex PatternNull = new Regex("^(.+)[\\s]+NULL$");

        /// <summary>
        /// Regex for parsing NOT NULL arguments.
        /// </summary>
        private static readonly Regex PatternNotNull = new Regex("^(.+)[\\s]+NOT[\\s]+NULL$");

        /// <summary>
        /// Regex for parsing DEFAULT value.
        /// </summary>
        private static readonly Regex PatternDefault = new Regex("^(.+)[\\s]+DEFAULT[\\s]+(.+)$");

        /// <summary>
        /// Creates a new <see cref="PgColumn"/> object.
        /// </summary>
        /// <param name="name">Name of the column.</param>
        public PgColumn(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets the comment of the <see cref="PgColumn"/>.
        /// </summary>
        [NullGuard.AllowNull]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the default value of the <see cref="PgColumn"/>.
        /// </summary>
        [NullGuard.AllowNull]
        public string DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the name of the <see cref="PgColumn"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> which indicates if the current <see cref="PgColumn"/> allows null values.
        /// </summary>
        public bool NullValue { get; set; }

        /// <summary>
        /// Gets or sets the statistics of the <see cref="PgColumn"/>.
        /// </summary>
        [NullGuard.AllowNull]
        public int? Statistics { get; set; }

        /// <summary>
        /// Gets or sets the storage of the <see cref="PgColumn"/>.
        /// </summary>
        [NullGuard.AllowNull]
        public string Storage { get; set; }

        /// <summary>
        /// Gets or sets the type of the <see cref="PgColumn"/>.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Returns full definition of the column.
        /// </summary>
        /// <param name="addDefaults">Whether default value should be added in case NOT NULL constraint is specified but no default value is set.</param>
        /// <returns>Full definition of the column.</returns>
        public string GetFullDefinition(bool addDefaults)
        {
            var definitionSql = new StringBuilder(100);
            definitionSql.Append(PgDiffStringExtension.QuoteName(this.Name));
            definitionSql.Append(' ');
            definitionSql.Append(this.Type);

            if (this.DefaultValue != null && this.DefaultValue.Length > 0)
            {
                definitionSql.Append(" DEFAULT ");
                definitionSql.Append(this.DefaultValue);
            }
            else if (!this.NullValue && addDefaults)
            {
                var defaultColValue = PgColumnUtils.GetDefaultValue(this.Type);

                if (defaultColValue != null)
                {
                    definitionSql.Append(" DEFAULT ");
                    definitionSql.Append(defaultColValue);
                }
            }

            if (!this.NullValue)
            {
                definitionSql.Append(" NOT NULL");
            }

            return definitionSql.ToString();
        }

        /// <summary>
        /// Parses definition of the column.
        /// </summary>
        /// <param name="definition">Definition of the column.</param>
        public void ParseDefinition(string definition)
        {
            if (PatternNotNull.Matches(definition).Count != 0)
            {
                definition = PatternNotNull.Matches(definition)[0].Groups[1].ToString().Trim();
                this.NullValue = false;
            }
            else
            {
                if (PatternNull.Matches(definition).Count != 0)
                {
                    definition = PatternNull.Matches(definition)[0].Groups[1].ToString().Trim();
                    this.NullValue = true;
                }
            }

            if (PatternDefault.Matches(definition).Count != 0)
            {
                Match match = PatternDefault.Matches(definition)[0];
                definition = match.Groups[1].ToString().Trim();
                this.DefaultValue = match.Groups[2].ToString().Trim();
            }

            this.Type = definition;
        }
    }
}