// <copyright file="TeamworkTestException.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>

using System;

namespace APE.PostgreSQL.Teamwork.ViewModel.Exceptions
{
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
    }
}
