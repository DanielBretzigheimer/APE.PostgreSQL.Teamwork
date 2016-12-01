// <copyright file="SettingsManager.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using log4net;

namespace APE.PostgreSQL.Teamwork.Model.Setting
{
    /// <summary>
    /// Loads the application settings from the settings file in the
    /// applications directory
    /// </summary>
    public class SettingsManager
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static string settingsPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\settings.ocignore.xml";

        private static SettingsManager instance = null;

        private SettingsManager()
        {
            // initialize default settings
            this.Setting = new ApplicationSetting()
            {
                ConnectionStringTemplate = "User Id=[Id];Password=[Password];host=[Host];database=[Database];Port=[Port];",
                PgDumpLocation = @"C:\Program Files\PostgreSQL\9.5\bin\pg_dump.exe",
                Host = "localhost",
                Id = "postgres",
                Port = 5432,
                OpenFilesInDefaultApplication = false,
                AutoRefresh = false,
            };
        }

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

            if (!File.Exists(settingsPath))
            {
                instance = new SettingsManager();
                instance.Save();
                return instance;
            }

            return LoadFile(settingsPath);
        }

        private static SettingsManager LoadFile(string path)
        {
            // deserialize
            string attributeXml = string.Empty;

            XmlDocument xmlDocument = new XmlDocument();

            try
            {
                xmlDocument.Load(path);
            }
            catch (Exception ex)
            {
                var newPath = path.Replace(".xml", ".old.xml");
                File.Move(path, newPath);
                Log.Error(string.Format("Xml File {0} was not correctly formatted and renamed to {1}", path, newPath), ex);

                // load new setting
                return Get();
            }

            string xmlString = xmlDocument.OuterXml;

            using (StringReader read = new StringReader(xmlString))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SettingsManager));
                using (XmlReader reader = new XmlTextReader(read))
                {
                    instance = (SettingsManager)serializer.Deserialize(reader);
                    reader.Close();
                }

                read.Close();
            }

            return instance;
        }

        /// <summary>
        /// Saves the application settings.
        /// </summary>
        public void Save()
        {
            // serialize
            XmlDocument xmlDocument = new XmlDocument();
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, this);
                stream.Position = 0;
                xmlDocument.Load(stream);
                xmlDocument.Save(settingsPath);
                stream.Close();
            }
        }
    }
}
