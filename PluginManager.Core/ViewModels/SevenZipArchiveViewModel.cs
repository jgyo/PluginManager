namespace PluginManager.Core.ViewModels
{
    using global::System;
    using global::System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="SevenZipArchiveViewModel" />.
    /// </summary>
    public class SevenZipArchiveViewModel : ViewModel, IArchiveDirectoryEntry, IArchiveViewModel
    {
        /// <summary>
        /// Defines the selectedDirectory.
        /// </summary>
        private IArchiveDirectoryEntry selectedDirectory;

        /// <summary>
        /// Initializes a new instance of the <see cref="SevenZipArchiveViewModel"/> class.
        /// </summary>
        /// <param name="filename">The filename<see cref="string"/>.</param>
        /// <param name="filePath">The filePath<see cref="string"/>.</param>
        /// <param name="packageId">The packageId<see cref="long"/>.</param>
        public SevenZipArchiveViewModel(string filename, string filePath, long packageId)
        {
            SelectedDirectory = this;
            FileName = filename;
            // Path to zip file
            Path = global::System.IO.Path.GetFullPath(filePath);
            PackageId = packageId;

            var archive = SevenZipArchive.Open(FullPath);
            SourcePath = archive.ExtractedFiles;

            foreach (var entry in archive.Entries)
            {
                SaveBranchAndNode(entry);
            }
        }

        /// <summary>
        /// Gets the Entries.
        /// </summary>
        public List<IArchiveEntryViewModel> Entries => new Lazy<List<IArchiveEntryViewModel>>(() => new List<IArchiveEntryViewModel>(SortedEntries.Values)).Value;

        /// <summary>
        /// Gets the FileName.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Gets the FullName.
        /// </summary>
        public string FullName { get => "{Root}"; }

        /// <summary>
        /// Gets the FullPath.
        /// </summary>
        public string FullPath
        {
            get { return global::System.IO.Path.Combine(Path, FileName); }
        }

        /// <summary>
        /// Gets the PackageId.
        /// </summary>
        public long PackageId { get; private set; }

        /// <summary>
        /// Gets the Path.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Gets or sets the SelectedDirectory.
        /// </summary>
        public IArchiveDirectoryEntry SelectedDirectory { get => selectedDirectory; set => SetProperty(ref selectedDirectory, value); }

        /// <summary>
        /// Gets the SortedEntries.
        /// </summary>
        public SortedList<string, IArchiveEntryViewModel> SortedEntries { get; } = new SortedList<string, IArchiveEntryViewModel>();

        /// <summary>
        /// Gets or sets the SourcePath.
        /// </summary>
        public string SourcePath { get; internal set; }

        /// <summary>
        /// The SaveBranchAndNode.
        /// </summary>
        /// <param name="entry">The entry<see cref="IArchiveEntry"/>.</param>
        private void SaveBranchAndNode(IArchiveEntry entry)
        {
            var vm = new SevenZipArchiveEntryViewModel(this, entry);
            SortedEntries.Add(entry.IsDirectory ? $"_{entry.Name}" : entry.Name, vm);
        }
    }
}
