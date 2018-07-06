// <copyright file="PgSchema.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        [NullGuard.AllowNull]
        public string Authorization { get; set; }

        /// <summary>
        /// Gets or sets the comment for this <see cref="PgSchema"/>
        /// </summary>
        [NullGuard.AllowNull]
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
                var creationSql = new StringBuilder(50);
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
        public List<PgFunction> Functions { get; private set; } = new List<PgFunction>();

        /// <summary>
        /// Gets a list with all <see cref="PgAggregate"/>s.
        /// </summary>
        public List<PgAggregate> Aggregates { get; private set; } = new List<PgAggregate>();

        /// <summary>
        /// Gets a list with all <see cref="PgType"/>s.
        /// </summary>
        /// <remarks>This will also contain all <see cref="PgType"/>s where <see cref="PgType.IsEnum"/> is set.</remarks>
        public List<PgType> Types { get; private set; } = new List<PgType>();

        /// <summary>
        /// Gets the name of the <see cref="PgSchema"/>.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets a list with all <see cref="PgIndex"/>.
        /// </summary>
        public List<PgIndex> Indexes { get; private set; } = new List<PgIndex>();

        /// <summary>
        /// Gets a list with all primary key <see cref="PgConstraint"/>s.
        /// </summary>
        public List<PgConstraint> PrimaryKeys { get; private set; } = new List<PgConstraint>();

        public List<PgPrivilege> Privileges { get; private set; } = new List<PgPrivilege>();

        /// <summary>
        /// Gets a list with all <see cref="PgSequence"/>s.
        /// </summary>
        public List<PgSequence> Sequences { get; private set; } = new List<PgSequence>();

        /// <summary>
        /// Gets a list of all <see cref="PgView"/>s.
        /// </summary>
        public List<PgView> Views { get; private set; } = new List<PgView>();

        /// <summary>
        /// Gets a list of all <see cref="PgTable"/>s in this <see cref="PgSchema"/>.
        /// </summary>
        public List<PgTable> Tables { get; private set; } = new List<PgTable>();

        /// <summary>
        /// Gets a list of all <see cref="PgRule"/>s in this <see cref="PgSchema"/>.
        /// </summary>
        public List<PgRule> Rules { get; private set; } = new List<PgRule>();

        /// <summary>
        /// Finds function according to specified function signature.
        /// </summary>
        /// <param name="signature">Signature of the function to be searched.</param>
        /// <returns>Found function or null if no such function has been found.</returns>
        [return: NullGuard.AllowNull]
        public PgFunction GetFunction(string signature)
        {
            foreach (PgFunction function in this.Functions)
            {
                if (function.Signature.Equals(signature))
                {
                    return function;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the <see cref="PgAggregate"/> with the given signature.
        /// </summary>
        [return: NullGuard.AllowNull]
        public PgAggregate GetAggregate(string signature)
        {
            foreach (PgAggregate aggregate in this.Aggregates)
            {
                if (aggregate.Signature.Equals(signature))
                {
                    return aggregate;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the <see cref="PgType"/> with the given name.
        /// </summary>
        [return: NullGuard.AllowNull]
        public PgType GetEnum(string name)
        {
            foreach (PgType type in this.Types)
            {
                if (type.Name == name && type.IsEnum)
                {
                    return type;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the <see cref="PgType"/> with the given signature.
        /// </summary>
        [return: NullGuard.AllowNull]
        public PgType GetPgType(string signature)
        {
            foreach (PgType type in this.Types)
            {
                if (type.Signature.Equals(signature))
                {
                    return type;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds index according to specified index name.
        /// </summary>
        /// <param name="name">Name of the index to be searched.</param>
        /// <returns>Found index or null if no such index has been found.</returns>
        [return: NullGuard.AllowNull]
        public PgIndex GetIndex(string name)
        {
            foreach (PgIndex index in this.Indexes)
            {
                if (index.Name.Equals(name))
                {
                    return index;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds primary key according to specified primary key name.
        /// </summary>
        /// <param name="name">Name of the primary key to be searched.</param>
        /// <returns>Found primary key or null if no such primary key has been found.</returns>
        [return: NullGuard.AllowNull]
        public PgConstraint GetPrimaryKey(string name)
        {
            foreach (PgConstraint constraint in this.PrimaryKeys)
            {
                if (constraint.Name.Equals(name))
                {
                    return constraint;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds sequence according to specified sequence name.
        /// </summary>
        /// <param name="name">Name of the sequence to be searched.</param>
        /// <returns>Found sequence or null if no such sequence has been found.</returns>
        [return: NullGuard.AllowNull]
        public PgSequence GetSequence(string name)
        {
            foreach (PgSequence sequence in this.Sequences)
            {
                if (sequence.Name.Equals(name))
                {
                    return sequence;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds table according to specified table name.
        /// </summary>
        /// <param name="name">Name of the table to be searched.</param>
        /// <returns>Found table or null if no such table has been found.</returns>
        [return: NullGuard.AllowNull]
        public PgTable GetTable(string name)
        {
            foreach (PgTable table in this.Tables)
            {
                if (table.Name.Equals(name))
                {
                    return table;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds view according to specified view name.
        /// </summary>
        /// <param name="name">Name of the view to be searched.</param>
        /// <returns>Found view or null if no such view has been found.</returns>
        [return: NullGuard.AllowNull]
        public PgView GetView(string name)
        {
            foreach (PgView view in this.Views)
            {
                if (view.Name.Equals(name))
                {
                    return view;
                }
            }

            return null;
        }

        /// <summary>
        /// Adds index to the list of indexes.
        /// </summary>
        public void Add(PgIndex index)
        {
            this.Indexes.Add(index);
        }

        /// <summary>
        /// Adds the given privilege to the list of privileges in this schema.
        /// </summary>
        /// <param name="privilege">The privilege which is added.</param>
        public void Add(PgPrivilege privilege)
        {
            this.Privileges.Add(privilege);
        }

        /// <summary>
        /// Adds primary key to the list of primary keys.
        /// </summary>
        public void Add(PgConstraint primaryKey)
        {
            this.PrimaryKeys.Add(primaryKey);
        }

        /// <summary>
        /// Adds function to the list of functions.
        /// </summary>
        public void Add(PgFunction function)
        {
            this.Functions.Add(function);
        }

        /// <summary>
        /// Adds the given <see cref="PgAggregate"/> to this <see cref="PgSchema"/>.
        /// </summary>
        public void Add(PgAggregate aggregate)
        {
            this.Aggregates.Add(aggregate);
        }

        /// <summary>
        /// Adds the given <see cref="PgType"/> to this <see cref="PgSchema"/>.
        /// </summary>
        public void Add(PgType type)
        {
            this.Types.Add(type);
        }

        /// <summary>
        /// Adds sequence to the list of sequences.
        /// </summary>
        public void Add(PgSequence sequence)
        {
            this.Sequences.Add(sequence);
        }

        /// <summary>
        /// Adds table to the list of tables.
        /// </summary>
        public void Add(PgTable table)
        {
            this.Tables.Add(table);
        }

        /// <summary>
        /// Adds view to the list of views.
        /// </summary>
        public void Add(PgView view)
        {
            // check if view is overriden in dump
            if (this.Views.Any(v => v.Name == view.Name))
            {
                var oldView = this.Views.Single(v => v.Name == view.Name);
                this.Views.Remove(oldView);
            }

            this.Views.Add(view);
        }

        public bool Contains(PgRule rule)
        {
            foreach (var r in this.Rules)
            {
                if (r.Name == rule.Name)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if schema contains function with given signature, otherwise false.
        /// </summary>
        /// <param name="signature">Signature of the function.</param>
        /// <returns>True if schema contains function with given signature, otherwise false.</returns>
        public bool ContainsFunction(string signature)
        {
            foreach (PgFunction function in this.Functions)
            {
                if (function.Signature.Equals(signature))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the <see cref="PgSchema"/> contains the given <see cref="PgAggregate"/> signature.
        /// </summary>
        public bool ContainsAggregate(string signature)
        {
            foreach (PgAggregate aggregate in this.Aggregates)
            {
                if (aggregate.Signature.Equals(signature))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets a list of all new entries for the given <see cref="PgType"/> or null if it was not found.
        /// </summary>
        [return: NullGuard.AllowNull]
        public List<string> TypeEntriesChanged(PgType oldType)
        {
            foreach (PgType type in this.Types)
            {
                if (type.Name == oldType.Name)
                {
                    var newEntries = new List<string>();

                    foreach (var entry in oldType.EnumEntries)
                    {
                        if (!type.EnumEntries.Contains(entry))
                        {
                            return null; // returns null because the old type can't contain more than the new one (means it was deleted and new created)
                        }
                    }

                    foreach (var entry in type.EnumEntries)
                    {
                        if (!oldType.EnumEntries.Contains(entry))
                        {
                            newEntries.Add(entry);
                        }
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
            foreach (PgType type in this.Types)
            {
                if (type.Signature.Equals(signature))
                {
                    return true;
                }
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
                {
                    return true;
                }
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
            foreach (PgSequence sequence in this.Sequences)
            {
                if (sequence.Name.Equals(name))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if schema contains table with given name, otherwise false.
        /// </summary>
        public bool ContainsTable(string name)
        {
            foreach (PgTable table in this.Tables)
            {
                if (table.Name.Equals(name))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if schema contains view with given name, otherwise false.
        /// </summary>
        public bool ContainsView(string name)
        {
            foreach (PgView view in this.Views)
            {
                if (view.Name.Equals(name))
                {
                    return true;
                }
            }

            return false;
        }

        [return: NullGuard.AllowNull]
        public override string ToString()
        {
            return $"{this.GetType().Name} {this.Name}";
        }
    }
}