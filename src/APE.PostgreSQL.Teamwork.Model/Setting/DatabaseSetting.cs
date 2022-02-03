// <copyright file="DatabaseSetting.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using APE.PostgreSQL.Teamwork.Model.Setting;
using APE.PostgreSQL.Teamwork.Model.Templates;

namespace APE.PostgreSQL.Teamwork.Model
{
    /// <summary>
    /// Stores all settings for a database.
    /// </summary>
    public class DatabaseSetting
    {
        public static readonly List<string> DefaultIgnoredSchemas = new()
        {
            SQLTemplates.PostgreSQLTeamworkSchemaName,
            "APE.PostgreSQL.Test.Runner",
        };

        /// <summary>
        /// Creates a new <see cref="DatabaseSetting"/>.
        /// </summary>
        /// <param name="id">The id of the setting.</param>
        /// <param name="name">The database name.</param>
        /// <param name="path">The path to the database diffs and dumps.</param>
        public DatabaseSetting(int id, string name, string path)
        {
            this.Id = id;
            this.Path = path;
            this.Name = name;
        }

        /// <summary>
        /// This is used for serializing the <see cref="DatabaseSetting"/>.
        /// </summary>
        protected DatabaseSetting()
        {
        }

        /// <summary>
        /// Gets a unique identifier for this database.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of this database.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the path to the diffs and dumps.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the ignored schemas.
        /// </summary>
        public List<string> IgnoredSchemas { get; set; } = new List<string>();

        /// <summary>
        /// Removes the database with the given id.
        /// </summary>
        /// <param name="id">Database id of the database which should be removed.</param>
        public static void Remove(int id)
        {
            var settings = GetDatabaseSettings();
            SettingsManager.Get().Setting.DatabaseSettings
                .Remove(settings.Single(s => s.Id == id));
            SettingsManager.Get().Save();
        }

        /// <summary>
        /// Adds a new database setting to the application settings.
        /// </summary>
        /// <param name="id">The identifier for the database.</param>
        /// <param name="name">The name of the database.</param>
        /// <param name="path">The path of the database.</param>
        public static void AddDatabaseSetting(int id, string name, string path)
        {
            SettingsManager.Get().Setting.DatabaseSettings
                .Add(new DatabaseSetting(id, name, path));
            SettingsManager.Get().Save();
        }

        /// <summary>
        /// Adds a new database to the settings.
        /// </summary>
        /// <param name="name">The name of the database.</param>
        /// <param name="path">The path of the database.</param>
        public static void AddDatabaseSetting(string name, string path) => AddDatabaseSetting(GetDatabaseSettings().Count == 0 ? 1 : GetDatabaseSettings().Max(d => d.Id + 1), name, path);

        /// <summary>
        /// Gets a <see cref="List{DatabaseSetting}"/> from the <see cref="ApplicationSetting.DatabaseSettings"/>.
        /// </summary>
        public static List<DatabaseSetting> GetDatabaseSettings() => SettingsManager.Get().Setting.DatabaseSettings;

        /// <summary>
        /// Gets the database with the given id from the application settings.
        /// </summary>
        /// <param name="id">The id of the database which is requested.</param>
        public static DatabaseSetting GetDatabaseSetting(int id)
        {
            return GetDatabaseSettings()
                .Single(s => s.Id == id);
        }

        /// <summary>
        /// Creates a string of the <see cref="DatabaseSetting"/> which contains the <see cref="Id"/> and <see cref="Name"/>.
        /// </summary>
        public override string ToString() => $"{this.Id} {this.Name}";
    }
}
