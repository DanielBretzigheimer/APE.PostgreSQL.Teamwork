using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APE.PostgreSQL.Teamwork.Model.Utils;

namespace APE.PostgreSQL.Teamwork.Model.PostgresSchema
{
    public class PgRule : IPgObject
    {
        public PgRule(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets the name of the <see cref="PgRule"/>.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the ON part of the <see cref="PgRule"/>.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Gets the DO INSTEAD part of the <see cref="PgRule"/>.
        /// </summary>
        public string Command { get; set; } = "NOTHING";

        /// <summary>
        /// Gets the event type of the rule. Can be one of SELECT | INSERT | UPDATE | DELETE.
        /// </summary>
        public string EventType { get; set; } = "SELECT";

        public string Do { get; set; } = "INSTEAD";

        /// <summary>
        /// Gets a optional condition which can be null.
        /// </summary>
        [NullGuard.AllowNull]
        public string Condition { get; set; } = null;

        public string CreationSql
        {
            get
            {
                var sql = new StringBuilder();
                sql.Append("CREATE RULE ");
                sql.Append(this.Name.QuoteName());
                sql.Append(" AS ON ");
                sql.AppendLine(this.EventType);

                sql.Append(" TO ");
                sql.Append(this.TableName);

                if (this.Condition != null)
                {
                    sql.Append(" WHERE ");
                    sql.AppendLine(this.Condition);
                }

                sql.Append(" DO ");
                sql.Append(this.Do);
                sql.Append(" ");
                sql.Append(this.Command);
                sql.Append(";");

                return sql.ToString();
            }
        }

        public string DropSql => $"DROP RULE {this.Name.QuoteName()} ON {this.TableName};";
    }
}
