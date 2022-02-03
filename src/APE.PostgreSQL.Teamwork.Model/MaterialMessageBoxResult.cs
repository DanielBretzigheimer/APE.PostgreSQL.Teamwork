// <copyright file="MaterialMessageBoxResult.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>

namespace APE.PostgreSQL.Teamwork.Model
{
    /// <summary>
    /// Contains all possible results of the Material message box.
    /// </summary>
    public enum MaterialMessageBoxResult
    {
        /// <summary>
        /// The dialog was canceled by clicking the corresponding button or dismissing the dialog by another user action (e.g. pressing escape).
        /// </summary>
        Cancel,

        /// <summary>
        /// The user pressed the "no" button.
        /// </summary>
        No,

        /// <summary>
        /// No result at all.
        /// </summary>
        None,

        /// <summary>
        /// The user pressed the "ok" button.
        /// </summary>
        OK,

        /// <summary>
        /// The user pressed the "yes" button.
        /// </summary>
        Yes,
    }
}
