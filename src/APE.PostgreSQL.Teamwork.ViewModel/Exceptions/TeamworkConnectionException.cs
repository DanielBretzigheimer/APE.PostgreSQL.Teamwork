// <copyright file="TeamworkConnectionException.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Runtime.Serialization;
using Npgsql;

namespace APE.PostgreSQL.Teamwork.ViewModel.Exceptions
{
    /// <summary>
    /// Exception which is thrown when an error occurs with the connection and contains the file and statement for which the error occurred (if available).
    /// </summary>
    [Serializable]
    public class TeamworkConnectionException : Exception
    {
        /// <summary>
        /// Creates a new Exception with the given parameters and a user defined message.
        /// </summary>
        public TeamworkConnectionException(ISQLFile? file, string message)
            : base(message) => this.File = file;

        /// <summary>
        /// Creates a new Exception with the given parameters, a user defined message and a
        /// inner exception which is used for the stack trace.
        /// </summary>
        public TeamworkConnectionException(ISQLFile? file, string message, NpgsqlException innerException)
            : base(message, innerException)
        {
            this.SqlException = innerException;
            this.File = file;
        }

        /// <summary>
        /// SQLFile in which the exception occurred (can be null).
        /// </summary>
        public ISQLFile? File { get; set; }

        public NpgsqlException? SqlException { get; set; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context) => base.GetObjectData(info, context);
    }
}
