// <copyright file="TeamworkTestException.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>

using System.Runtime.Serialization;

namespace APE.PostgreSQL.Teamwork.ViewModel.Exceptions
{
    [Serializable]
    public class TeamworkTestException : Exception
    {
        public TeamworkTestException(string message, TeamworkConnectionException ex)
            : base(message, ex)
        {
            if (ex.File == null)
                throw new ArgumentNullException($"The {nameof(TeamworkConnectionException)}s file was null.");

            this.ConnectionException = ex;
            this.FileWithExceptionSql = ex.File.Path;
        }

        public TeamworkConnectionException ConnectionException { get; set; }

        public string FileWithExceptionSql { get; set; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            if (info == null)
                throw new ArgumentNullException(nameof(info));

            info.AddValue(nameof(this.FileWithExceptionSql), this.FileWithExceptionSql);
            info.AddValue(nameof(this.ConnectionException), this.ConnectionException);
        }
    }
}
