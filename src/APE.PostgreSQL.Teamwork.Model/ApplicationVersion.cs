// <copyright file="ApplicationVersion.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Xml.Serialization;

namespace APE.CodeGeneration.Model
{
    public class ApplicationVersion
    {
        public ApplicationVersion(Version version)
        {
            this.Major = version.Major;
            this.Minor = version.Minor;
            this.Build = version.Build;
            this.Revision = version.Revision;
        }

        protected ApplicationVersion()
        {
        }

        public int Major { get; set; } = 0;

        public int Minor { get; set; } = 0;

        public int Build { get; set; } = 0;

        public int Revision { get; set; } = 0;

        [XmlIgnore]
        public Version Version => new(this.Major, this.Minor, this.Build, this.Revision);

        public override string ToString() => $"{this.Major}.{this.Minor}.{this.Build}.{this.Revision}";
    }
}
