namespace PluginManager.Core.ViewModels.DesignTime
{
    using PluginManager.Core.Logging;

    /// <summary>
    /// Defines the <see cref="SetupViewModelDesignTime" />.
    /// </summary>
    public class SetupViewModelDesignTime : SetupViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetupViewModelDesignTime"/> class.
        /// </summary>
        public SetupViewModelDesignTime()
        {
            this.CommunityFolder = "community";
            this.HiddenFilesFolder = "hidden";
            this.LoggingEnabled = true;
            this.LoggingLevel = LogLevel.Exception;
            this.ZipFilesFolder = "Downloads";
        }
    }
}
