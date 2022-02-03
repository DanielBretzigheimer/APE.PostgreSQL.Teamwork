// <copyright file="GUIRegistry.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>

using APE.PostgreSQL.Teamwork.ViewModel;
using Lamar;
using Serilog;

namespace APE.PostgreSQL.Teamwork.GUI
{
    /// <summary>
    /// Initializes the <see cref="Registry"/> for the GUI project.
    /// </summary>
    public class GUIRegistry : ServiceRegistry
    {
        public GUIRegistry()
        {
            Log.Debug("Start initializing GUI Registry");

            this.IncludeRegistry<ViewModelRegistry>();

            Log.Debug("Finished initializing GUI Registry");
        }
    }
}
