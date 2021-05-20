namespace PluginManager.Core.ViewModels.DesignTime
{
    using global::System;
    using global::System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="MainViewModelDesignTime" />.
    /// </summary>
    public class MainViewModelDesignTime : MainViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModelDesignTime"/> class.
        /// </summary>
        public MainViewModelDesignTime()
        {
            var array = new List<FolderViewModel>()
            {
                new FolderViewModel()
                {
                    FolderId = 1,
                    FolderName = "Thunder",
                    InstallDate = DateTime.Today,
                    IsHidden = false,
                },
                new FolderViewModel()
                {
                    FolderId = 2,
                    FolderName = "CatsAndDogs",
                    InstallDate = DateTime.Today,
                    IsHidden = true,
                },
                new FolderViewModel()
                {
                    FolderId = 3,
                    FolderName = "BushPilotPro",
                    InstallDate = DateTime.Today,
                    IsHidden = false,
                }
            };

            int i = 0;
            while (i < array.Count)
            {
                this.FolderCollection.Add(array[i++]);
            }
        }
    }
}
