// <copyright file="TeamworkException.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Runtime.Serialization;

namespace APE.PostgreSQL.Teamwork.ViewModel.Exceptions
{
    /// <summary>
    /// Exception which is thrown from the teamwork tool and caught with specific care.
    /// </summary>
    [Serializable]
    public class TeamworkException : Exception
    {
        public TeamworkException(string msg)
            : this(msg, true)
        {
        }

        public TeamworkException(string msg, bool showAsError)
            : base(msg) => this.ShowAsError = showAsError;

        /// <summary>
        /// Indicates if the exception should be displayed as an error to the user or as an information.
        /// </summary>
        public bool ShowAsError { get; set; }

        /// <summary>
        /// Gets the <see cref="TeamworkException"/> which should be thrown if no changes were detected.
        /// </summary>
        public static TeamworkException NoChanges(string previousDump, string dump) => new($"No changes found between {previousDump} and {dump}", false);

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(this.ShowAsError), this.ShowAsError);
        }
    }
}
