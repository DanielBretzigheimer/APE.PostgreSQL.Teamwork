// <copyright file="TeamworkParserException.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;

namespace APE.PostgreSQL.Teamwork.ViewModel.Exceptions
{
    /// <summary>
    /// Is thrown if an exception is thrown while parsing the SQL.
    /// </summary>
    public class TeamworkParserException : Exception
    {
        public TeamworkParserException()
        {
        }

        public TeamworkParserException(string msg)
            : base(msg)
        {
        }

        public TeamworkParserException(string msg, Exception cause)
            : base(msg, cause)
        {
        }
    }
}