using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APE.PostgreSQL.Teamwork.Model.PostgresSchema
{
    public interface IPgObject
    {
        /// <summary>
        /// Gets the SQL for the creation of this <see cref="IPgObject"/>.
        /// </summary>
        string CreationSql { get; }

        /// <summary>
        /// Gets the SQL for the drop of this <see cref="IPgObject"/>.
        /// </summary>
        string DropSql { get; }
    }
}
