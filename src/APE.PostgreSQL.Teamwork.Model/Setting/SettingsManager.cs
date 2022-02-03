// <copyright file="SettingsManager.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>

using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using APE.CodeGeneration.Model;
using Serilog;

namespace APE.PostgreSQL.Teamwork.Model.Setting
{
    /// <summary>
    /// Loads the application settings from the settings file in the
    /// applications directory.
    /// </summary>
    public class SettingsManager
    {
        private static readonly string SettingsPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\settings.ocignore.xml";

        private static SettingsManager? instance = null;

        private SettingsManager() =>
            this.Setting = new ApplicationSetting();

        /// <summary>
        /// Gets or sets the settings of the whole application.
        /// </summary>
        public ApplicationSetting Setting { get; set; }

        /// <summary>
        /// Loads the settings from the xml file or creates new settings and saves them.
        /// </summary>
        /// <param name="refresh">Indicates if the current settings are reseted.</param>
        /// <returns>A instance of the setting manager which contains the <see cref="ApplicationSetting"/>.</returns>
        public static SettingsManager Get(bool refresh = false)
        {
            if (instance != null && !refresh)
                return instance;

            if (!File.Exists(SettingsPath))
            {
                instance = new SettingsManager();
                instance.Save();
                return instance;
            }

            return LoadFile(SettingsPath);
        }

        /// <summary>
        /// Saves the application settings.
        /// </summary>
        public void Save()
        {
            // serialize
            var xmlDocument = new XmlDocument();
            var serializer = new XmlSerializer(this.GetType());
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, this);
                stream.Position = 0;
                xmlDocument.Load(stream);
                xmlDocument.Save(SettingsPath);
            }
        }

        public void UpdateVersion(Version assemblyVersion)
        {
            this.Setting.ApplicationVersion = new ApplicationVersion(assemblyVersion);
            this.Save();
        }

        private static SettingsManager LoadFile(string path)
        {
            var xmlDocument = new XmlDocument();

            try
            {
                xmlDocument.Load(path);
            }
            catch (Exception ex)
            {
                var newPath = path.Replace(".xml", ".old.xml");
                File.Move(path, newPath);
                Log.Error(ex, $"Xml File {path} was not correctly formatted and renamed to {newPath}");

                // load new setting
                return Get();
            }

            var xmlString = xmlDocument.OuterXml;

            using var read = new StringReader(xmlString);
            var serializer = new XmlSerializer(typeof(SettingsManager));
            var reader = new XmlTextReader(read);

            if (serializer != null && reader != null)
                instance = (SettingsManager?)serializer.Deserialize(reader);

            return instance ?? new SettingsManager();
        }
    }
}
