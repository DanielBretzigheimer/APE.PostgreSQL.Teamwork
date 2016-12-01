// <copyright file="PgDatabase.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Collections.Generic;
using System.Collections.ObjectModel;
using APE.PostgreSQL.Teamwork.Model.Utils;

namespace APE.PostgreSQL.Teamwork.Model.PostgresSchema
{
    /// <summary>
    /// Stores database information.
    /// </summary>
    public class PgDatabase
    {
        /// <summary>
        /// List of database schemas.
        /// </summary>
        private readonly List<PgSchema> schemas = new List<PgSchema>();

        /// <summary>
        /// Array of ignored statements.
        /// </summary>
        private readonly List<string> ignoredStatements = new List<string>();

        /// <summary>
        /// Creates a new <see cref="PgDatabase"/> object.
        /// </summary>
        public PgDatabase(string name)
        {
            this.schemas.Add(new PgSchema("public"));
            this.DefaultSchema = this.schemas[0];
            this.Name = PgDiffStringExtension.QuoteName(name);
        }

        /// <summary>
        /// Gets or sets the comment of the <see cref="PgDatabase"/>.
        /// </summary>
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
        /// Gets a <see cref="ReadOnlyCollection{String}"/> with all ignored statements.
        /// </summary>
        public IList<string> IgnoredStatements
        {
            get
            {
                return new ReadOnlyCollection<string>(this.ignoredStatements);
            }
        }

        /// <summary>
        /// Gets a <see cref="ReadOnlyCollection{PgSchema}"/> of all <see cref="PgSchema"/>s which belong to this database.
        /// </summary>
        public IList<PgSchema> Schemas
        {
            get
            {
                return new ReadOnlyCollection<PgSchema>(this.schemas);
            }
        }

        /// <summary>
        /// Adds ignored statement to the list of ignored statements.
        /// </summary>
        public void AddIgnoredStatement(string ignoredStatement)
        {
            this.ignoredStatements.Add(ignoredStatement);
        }

        /// <summary>
        /// Returns schema of given name or null if the schema has not been found. If schema name is null then default schema is returned.
        /// </summary>
        /// <param name="name">Schema name or null which means default schema.</param>
        /// <returns>Found schema or null.</returns>
        public PgSchema GetSchema(string name)
        {
            if (name == null)
                return this.DefaultSchema;

            foreach (PgSchema schema in this.schemas)
                if (schema.Name.Equals(name))
                    return schema;

            return null;
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
            this.schemas.Add(schema);
        }
    }
}