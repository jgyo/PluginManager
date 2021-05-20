namespace PluginManager.Core.ViewModels
{
    /// <summary>
    /// Defines the <see cref="Locator" />.
    /// </summary>
    public static class Locator
    {
        /// <summary>
        /// Gets the MainViewModel.
        /// </summary>
        public static MainViewModel MainViewModel { get; } = new MainViewModel();

        /// <summary>
        /// Gets the SetupViewModel.
        /// </summary>
        public static SetupViewModel SetupViewModel { get; } = new SetupViewModel();

        /// <summary>
        /// The GetFolderViewModel.
        /// </summary>
        /// <returns>The <see cref="FolderViewModel"/>.</returns>
        public static FolderViewModel GetFolderViewModel() => new();

        /// <summary>
        /// The GetZipFileViewModel.
        /// </summary>
        /// <returns>The <see cref="ZipFileViewModel"/>.</returns>
        public static ZipFileViewModel GetZipFileViewModel() => new();
    }
}
