namespace PluginManager.Core.ViewModels.DesignTime
{
    using global::System;

    /// <summary>
    /// Defines the <see cref="FolderViewModelDesignTime" />.
    /// </summary>
    public class FolderViewModelDesignTime : FolderViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FolderViewModelDesignTime"/> class.
        /// </summary>
        public FolderViewModelDesignTime()
        {
            this.FolderId = 1234;
            this.FolderName = "PrideAndJoy";
            this.IsHidden = true;
            this.InstallDate = DateTime.Now;
            this.FolderName = "c:\\Downloads";
        }
    }
}
