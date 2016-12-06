// <copyright file="PgSchema.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using APE.PostgreSQL.Teamwork.Model.Utils;

namespace APE.PostgreSQL.Teamwork.Model.PostgresSchema
{
    /// <summary>
    /// Stores schema information.
    /// </summary>
    public class PgSchema
    {
        /// <summary>
        /// List of functions defined in the schema.
        /// </summary>
        private readonly IList<PgFunction> functions = new List<PgFunction>();

        /// <summary>
        /// List of aggregates defined in the schema.
        /// </summary>
        private readonly IList<PgAggregate> aggregates = new List<PgAggregate>();

        /// <summary>
        /// List of types defined in the schema.
        /// </summary>
        private readonly IList<PgType> types = new List<PgType>();

        /// <summary>
        /// List of sequences defined in the schema.
        /// </summary>
        private readonly IList<PgSequence> sequences = new List<PgSequence>();

        /// <summary>
        /// List of tables defined in the schema.
        /// </summary>
        private readonly IList<PgTable> tables = new List<PgTable>();

        /// <summary>
        /// List of views defined in the schema.
        /// </summary>
        private readonly IList<PgView> views = new List<PgView>();

        /// <summary>
        /// List of indexes defined in the schema.
        /// </summary>
        private readonly IList<PgIndex> indexes = new List<PgIndex>();

        /// <summary>
        /// List of primary keys defined in the schema.
        /// </summary>
        private readonly IList<PgConstraint> primaryKeys = new List<PgConstraint>();

        /// <summary>
        /// List of privileges in the schema.
        /// </summary>
        private readonly IList<PgPrivilege> privileges = new List<PgPrivilege>();

        /// <summary>
        /// Creates a new <see cref="PgSchema"/> object.
        /// </summary>
        /// <param name="name">The name of the <see cref="PgSchema"/>.</param>
        public PgSchema(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets the authorization for this <see cref="PgSchema"/>
        /// </summary>
        public string Authorization { get; set; }

        /// <summary>
        /// Gets or sets the comment for this <see cref="PgSchema"/>
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the definition for this <see cref="PgSchema"/>
        /// </summary>
        public string Definition { get; set; }

        /// <summary>
        /// Creates and returns SQL for creation of the schema.
        /// </summary>
        /// <returns> created SQL </returns>
        public string CreationSQL
        {
            get
            {
                StringBuilder creationSql = new StringBuilder(50);
                creationSql.Append("CREATE SCHEMA ");
                creationSql.Append(PgDiffStringExtension.QuoteName(this.Name));

                if (this.Authorization != null)
                {
                    creationSql.Append(" AUTHORIZATION ");
                    creationSql.Append(PgDiffStringExtension.QuoteName(this.Authorization));
                }

                creationSql.Append(';');

                if (this.Comment != null && this.Comment.Length > 0)
                {
                    creationSql.Append("\n\nCOMMENT ON SCHEMA ");
                    creationSql.Append(PgDiffStringExtension.QuoteName(this.Name));
                    creationSql.Append(" IS ");
                    creationSql.Append(this.Comment);
                    creationSql.Append(';');
                }

                return creationSql.ToString();
            }
        }

        /// <summary>
        /// Gets a list with all <see cref="PgFunction"/>s.
        /// </summary>
        public IList<PgFunction> Functions
        {
            get
            {
                return new ReadOnlyCollection<PgFunction>(this.functions);
            }
        }

        /// <summary>
        /// Gets a list with all <see cref="PgAggregate"/>s.
        /// </summary>
        public IList<PgAggregate> Aggregates
        {
            get
            {
                return new ReadOnlyCollection<PgAggregate>(this.aggregates);
            }
        }

        /// <summary>
        /// Gets a list with all <see cref="PgType"/>s.
        /// </summary>
        /// <remarks>This will also contain all <see cref="PgType"/>s where <see cref="PgType.IsEnum"/> is set.</remarks>
        public IList<PgType> Types
        {
            get
            {
                return new ReadOnlyCollection<PgType>(this.types);
            }
        }

        /// <summary>
        /// Gets the name of the <see cref="PgSchema"/>.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets a list with all <see cref="PgIndex"/>.
        /// </summary>
        public IList<PgIndex> Indexes
        {
            get
            {
                return new ReadOnlyCollection<PgIndex>(this.indexes);
            }
        }

        /// <summary>
        /// Gets a list with all primary key <see cref="PgConstraint"/>s.
        /// </summary>
        public IList<PgConstraint> PrimaryKeys
        {
            get
            {
                return new ReadOnlyCollection<PgConstraint>(this.primaryKeys);
            }
        }

        public IList<PgPrivilege> Privileges
        {
            get
            {
                return new ReadOnlyCollection<PgPrivilege>(this.privileges);
            }
        }

        /// <summary>
        /// Gets a list with all <see cref="PgSequence"/>s.
        /// </summary>
        public IList<PgSequence> Sequences
        {
            get
            {
                return new ReadOnlyCollection<PgSequence>(this.sequences);
            }
        }

        /// <summary>
        /// Gets a list of all <see cref="PgView"/>s.
        /// </summary>
        public IList<PgView> Views
        {
            get
            {
                return new ReadOnlyCollection<PgView>(this.views);
            }
        }

        /// <summary>
        /// Gets a list of all <see cref="PgTable"/>s.
        /// </summary>
        public IList<PgTable> Tables
        {
            get
            {
                return new ReadOnlyCollection<PgTable>(this.tables);
            }
        }

        /// <summary>
        /// Finds function according to specified function signature.
        /// </summary>
        /// <param name="signature">Signature of the function to be searched.</param>
        /// <returns>Found function or null if no such function has been found.</returns>
        public PgFunction GetFunction(string signature)
        {
            foreach (PgFunction function in this.functions)
            {
                if (function.Signature.Equals(signature))
                    return function;
            }

            return null;
        }

        /// <summary>
        /// Gets the <see cref="PgAggregate"/> with the given signature.
        /// </summary>
        public PgAggregate GetAggregate(string signature)
        {
            foreach (PgAggregate aggregate in this.aggregates)
            {
                if (aggregate.Signature.Equals(signature))
                    return aggregate;
            }

            return null;
        }

        /// <summary>
        /// Gets the <see cref="PgType"/> with the given name.
        /// </summary>
        public PgType GetEnum(string name)
        {
            foreach (PgType type in this.types)
            {
                if (type.Name == name && type.IsEnum)
                    return type;
            }

            return null;
        }

        /// <summary>
        /// Gets the <see cref="PgType"/> with the given signature.
        /// </summary>
        public PgType GetPgType(string signature)
        {
            foreach (PgType type in this.types)
            {
                if (type.Signature.Equals(signature))
                    return type;
            }

            return null;
        }

        /// <summary>
        /// Finds index according to specified index name.
        /// </summary>
        /// <param name="name">Name of the index to be searched.</param>
        /// <returns>Found index or null if no such index has been found.</returns>
        public PgIndex GetIndex(string name)
        {
            foreach (PgIndex index in this.indexes)
            {
                if (index.Name.Equals(name))
                    return index;
            }

            return null;
        }

        /// <summary>
        /// Finds primary key according to specified primary key name.
        /// </summary>
        /// <param name="name">Name of the primary key to be searched.</param>
        /// <returns>Found primary key or null if no such primary key has been found.</returns>
        public PgConstraint GetPrimaryKey(string name)
        {
            foreach (PgConstraint constraint in this.primaryKeys)
            {
                if (constraint.Name.Equals(name))
                    return constraint;
            }

            return null;
        }

        /// <summary>
        /// Finds sequence according to specified sequence name.
        /// </summary>
        /// <param name="name">Name of the sequence to be searched.</param>
        /// <returns>Found sequence or null if no such sequence has been found.</returns>
        public PgSequence GetSequence(string name)
        {
            foreach (PgSequence sequence in this.sequences)
            {
                if (sequence.Name.Equals(name))
                    return sequence;
            }

            return null;
        }

        /// <summary>
        /// Finds table according to specified table name.
        /// </summary>
        /// <param name="name">Name of the table to be searched.</param>
        /// <returns>Found table or null if no such table has been found.</returns>
        public PgTable GetTable(string name)
        {
            foreach (PgTable table in this.tables)
            {
                if (table.Name.Equals(name))
                    return table;
            }

            return null;
        }

        /// <summary>
        /// Finds view according to specified view name.
        /// </summary>
        /// <param name="name">Name of the view to be searched.</param>
        /// <returns>Found view or null if no such view has been found.</returns>
        public PgView GetView(string name)
        {
            foreach (PgView view in this.views)
            {
                if (view.Name.Equals(name))
                    return view;
            }

            return null;
        }

        /// <summary>
        /// Adds index to the list of indexes.
        /// </summary>
        public void AddIndex(PgIndex index)
        {
            this.indexes.Add(index);
        }

        /// <summary>
        /// Adds the given privilege to the list of privileges in this schema.
        /// </summary>
        /// <param name="privilege">The privilege which is added.</param>
        public void AddPrivilege(PgPrivilege privilege)
        {
            this.privileges.Add(privilege);
        }

        /// <summary>
        /// Adds primary key to the list of primary keys.
        /// </summary>
        public void AddPrimaryKey(PgConstraint primaryKey)
        {
            this.primaryKeys.Add(primaryKey);
        }

        /// <summary>
        /// Adds function to the list of functions.
        /// </summary>
        public void AddFunction(PgFunction function)
        {
            this.functions.Add(function);
        }

        /// <summary>
        /// Adds the given <see cref="PgAggregate"/> to this <see cref="PgSchema"/>.
        /// </summary>
        public void AddAggregate(PgAggregate aggregate)
        {
            this.aggregates.Add(aggregate);
        }

        /// <summary>
        /// Adds the given <see cref="PgType"/> to this <see cref="PgSchema"/>.
        /// </summary>
        public void AddType(PgType type)
        {
            this.types.Add(type);
        }

        /// <summary>
        /// Adds sequence to the list of sequences.
        /// </summary>
        public void AddSequence(PgSequence sequence)
        {
            this.sequences.Add(sequence);
        }

        /// <summary>
        /// Adds table to the list of tables.
        /// </summary>
        public void AddTable(PgTable table)
        {
            this.tables.Add(table);
        }

        /// <summary>
        /// Adds view to the list of views.
        /// </summary>
        public void AddView(PgView view)
        {
            this.views.Add(view);
        }

        /// <summary>
        /// Returns true if schema contains function with given signature, otherwise false.
        /// </summary>
        /// <param name="signature">Signature of the function.</param>
        /// <returns>True if schema contains function with given signature, otherwise false.</returns>
        public bool ContainsFunction(string signature)
        {
            foreach (PgFunction function in this.functions)
            {
                if (function.Signature.Equals(signature))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the <see cref="PgSchema"/> contains the given <see cref="PgAggregate"/> signature.
        /// </summary>
        public bool ContainsAggregate(string signature)
        {
            foreach (PgAggregate aggregate in this.aggregates)
            {
                if (aggregate.Signature.Equals(signature))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Gets a list of all new entries for the given <see cref="PgType"/> or null if it was not found.
        /// </summary>
        public List<string> TypeEntriesChanged(PgType oldType)
        {
            foreach (PgType type in this.types)
            {
                if (type.Name == oldType.Name)
                {
                    List<string> newEntries = new List<string>();

                    foreach (string entry in oldType.EnumEntries)
                    {
                        if (!type.EnumEntries.Contains(entry))
                            return null; // returns null because the old type can't contain more than the new one (means it was deleted and new created)
                    }

                    foreach (string entry in type.EnumEntries)
                    {
                        if (!oldType.EnumEntries.Contains(entry))
                            newEntries.Add(entry);
                    }

                    return newEntries;
                }
            }

            return null;
        }

        /// <summary>
        /// Checks if a type with the same signature is already existing in this schema.
        /// </summary>
        /// <param name="signature">Signature of the other type.</param>
        /// <returns>A bool indicating whether it existed or not.</returns>
        public bool ContainsType(string signature)
        {
            foreach (PgType type in this.types)
            {
                if (type.Signature.Equals(signature))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if schema contains privilege with given name, otherwise false.
        /// </summary>
        /// <param name="other">Checks if this privilege is also in this schema.</param>
        /// <returns>True if schema contains privilege with given name, otherwise false.</returns>
        public bool ContainsPrivilege(PgPrivilege other)
        {
            foreach (var privilege in this.Privileges)
            {
                if (privilege.Equals(other))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if schema contains sequence with given name, otherwise false.
        /// </summary>
        /// <param name="name">Name of the sequence.</param>
        /// <returns>True if schema contains sequence with given name, otherwise false.</returns>
        public bool ContainsSequence(string name)
        {
            foreach (PgSequence sequence in this.sequences)
            {
                if (sequence.Name.Equals(name))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if schema contains table with given name, otherwise false.
        /// </summary>
        public bool ContainsTable(string name)
        {
            foreach (PgTable table in this.tables)
            {
                if (table.Name.Equals(name))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if schema contains view with given name, otherwise false.
        /// </summary>
        public bool ContainsView(string name)
        {
            foreach (PgView view in this.views)
            {
                if (view.Name.Equals(name))
                    return true;
            }

            return false;
        }

        public override string ToString()
        {
            return $"{this.GetType().Name} {this.Name}";
        }
    }
}