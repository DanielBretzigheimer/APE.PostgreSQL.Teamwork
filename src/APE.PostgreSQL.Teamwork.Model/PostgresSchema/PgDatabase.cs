// <copyright file="PgDatabase.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using APE.PostgreSQL.Teamwork.Model.Utils;

namespace APE.PostgreSQL.Teamwork.Model.PostgresSchema
{
    /// <summary>
    /// Stores database information.
    /// </summary>
    public class PgDatabase
    {
        private readonly List<string> ignoredSchemas = new List<string>();

        /// <summary>
        /// Creates a new <see cref="PgDatabase"/> object.
        /// </summary>
        public PgDatabase(string name, List<string> ignoredSchemas)
        {
            this.Schemas.Add(PgSchema.Public);
            this.DefaultSchema = this.Schemas[0];
            this.Name = name.QuoteName();
            this.ignoredSchemas = ignoredSchemas;
        }

        /// <summary>
        /// Gets or sets the comment of the <see cref="PgDatabase"/>.
        /// </summary>
        [NullGuard.AllowNull]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the name of the <see cref="PgDatabase"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the default schema for this <see cref="PgDatabase"/>.
        /// </summary>
        public PgSchema DefaultSchema { get; private set; }

        /// <summary>
        /// List of database schemas.
        /// </summary>
        public List<PgSchema> Schemas { get; } = new List<PgSchema>();

        /// <summary>
        /// Array of ignored statements.
        /// </summary>
        public List<string> IgnoredStatements { get; } = new List<string>();

        public bool SchemaIsIgnored(string schemaName) => this.ignoredSchemas.Contains(schemaName);

        /// <summary>
        /// Adds ignored statement to the list of ignored statements.
        /// </summary>
        public void AddIgnoredStatement(string ignoredStatement)
        {
            this.IgnoredStatements.Add(ignoredStatement);
        }

        /// <summary>
        /// Returns schema of given name or null if the schema has not been found. If schema name is null then default schema is returned.
        /// </summary>
        /// <param name="name">Schema name or null which means default schema.</param>
        /// <returns>Found schema or null.</returns>
        [return: NullGuard.AllowNull]
        public PgSchema GetSchema([NullGuard.AllowNull] string name)
        {
            if (name == null)
                return this.DefaultSchema;

            return this.Schemas.SingleOrDefault(s => s.Name == name);
        }

        /// <summary>
        /// Sets default schema according to the name of the schema.
        /// </summary>
        public void SetDefaultSchema(string name)
        {
            this.DefaultSchema = this.GetSchema(name);
        }

        /// <summary>
        /// Adds <see cref="PgSchema"/> to the lists of schemas.
        /// </summary>
        public void AddSchema(PgSchema schema)
        {
            this.Schemas.Add(schema);
        }
    }
}