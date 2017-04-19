// <copyright file="ApplicationSetting.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APE.PostgreSQL.Teamwork.Model.Setting
{
    /// <summary>
    /// Contains basic settings for the application.
    /// </summary>
    public class ApplicationSetting
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ApplicationSetting"/> class.
        /// </summary>
        internal ApplicationSetting()
        {
            this.DatabaseSettings = new List<DatabaseSetting>();
        }

        /// <summary>
        /// Gets or sets a template of the connection string.
        /// </summary>
        public string ConnectionStringTemplate { get; set; } = "User Id=[Id];Password=[Password];host=[Host];database=[Database];Port=[Port];";

        /// <summary>
        /// Gets or sets the path to the pgdump.exe.
        /// </summary>
        public string PgDumpLocation { get; set; } = @"C:\Program Files\PostgreSQL\9.5\bin\pg_dump.exe";

        /// <summary>
        /// Gets or sets the password of the database which is used in <see cref="ConnectionStringTemplate"/>.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the hosts ip address (or localhost) which is used in <see cref="ConnectionStringTemplate"/>.
        /// </summary>
        public string Host { get; set; } = "localhost";

        /// <summary>
        /// Gets or sets the postgres id of the default user (default: postgres) which is used in <see cref="ConnectionStringTemplate"/>.
        /// </summary>
        public string Id { get; set; } = "postgres";

        /// <summary>
        /// Gets or sets the postgres port of the database (default: 5432) which is used in <see cref="ConnectionStringTemplate"/>.
        /// </summary>
        public int Port { get; set; } = 5432;

        /// <summary>
        /// Gets or sets a bool which indicates if the application should open the default application. This is used to open a text editor or
        /// opens the files in a own view.
        /// </summary>
        public bool OpenFilesInDefaultApplication { get; set; } = false;

        /// <summary>
        /// Gets or sets a bool which indicates if the overview is refreshed automatically after a fixed time period.
        /// </summary>
        public bool AutoRefresh { get; set; } = false;

        /// <summary>
        /// Gets or sets a path which is used as default location when specifing the location of the database diff folder.
        /// </summary>
        public string DefaultDatabaseFolderPath { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a list with all database settings.
        /// </summary>
        [NullGuard.AllowNull]
        public List<DatabaseSetting> DatabaseSettings { get; set; }
    }
}
