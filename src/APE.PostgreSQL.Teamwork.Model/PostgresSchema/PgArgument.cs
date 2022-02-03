// <copyright file="PgArgument.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>

namespace APE.PostgreSQL.Teamwork.Model.PostgresSchema
{
    /// <summary>
    /// C# class for the Postgres Argument.
    /// </summary>
    public class PgArgument
    {
        /// <summary>
        /// Creates a new Instance of an <see cref="PgArgument"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="PgArgument"/>.</param>
        /// <param name="datatype">The type of the <see cref="PgArgument"/>.</param>
        public PgArgument(string name, string datatype)
        {
            this.Name = name;
            this.DataType = datatype;
        }

        /// <summary>
        /// The name of this <see cref="PgArgument"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of this <see cref="PgArgument"/>.
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Gets a string which contains the <see cref="Name"/> and <see cref="DataType"/>.
        /// </summary>
        public string Full => string.Format("{0} {1}", this.Name, this.DataType);
    }
}
