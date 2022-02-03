// <copyright file="ExecutedFile.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>

namespace APE.PostgreSQL.Teamwork.Model
{
    /// <summary>
    /// Model class of the database table.
    /// </summary>
    public class ExecutedFile
    {
        private DatabaseVersion? version = null;

        /// <summary>
        /// Gets the <see cref="DateTime"/> when the file was executed.
        /// </summary>
        public DateTime ExecutionDate { get; private set; }

        /// <summary>
        /// Message which can contain the error message if one occurred.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets the <see cref="DatabaseVersion"/> of the current file.
        /// </summary>
        public DatabaseVersion DatabaseVersion
        {
            get
            {
                if (this.version == null)
                    this.version = new DatabaseVersion(this);

                return this.version;
            }
        }

        public string Version { get; set; } = string.Empty;

        /// <summary>
        ///  Returns a string that represents the current object.
        /// </summary>
        public override string ToString() => $"{this.GetType().Name} Version: {this.DatabaseVersion.Full} Executed: {this.ExecutionDate}";
    }
}
