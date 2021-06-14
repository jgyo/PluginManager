namespace PluginManager.Core.ViewModels
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.IO;

    /// <summary>
    /// Defines the <see cref="SevenZipArchiveEntryViewModel" />.
    /// </summary>
    public class SevenZipArchiveEntryViewModel : ViewModel, IArchiveDirectoryEntry, IArchiveEntryViewModel
    {
        /// <summary>
        /// Defines the archiveRoot.
        /// </summary>
        private SevenZipArchiveViewModel archiveRoot;

        /// <summary>
        /// Defines the entries.
        /// </summary>
        private List<IArchiveEntryViewModel> entries;

        /// <summary>
        /// Defines the sortedEntries.
        /// </summary>
        private SortedList<string, IArchiveEntryViewModel> sortedEntries;

        /// <summary>
        /// Defines the willInstall.
        /// </summary>
        private bool willInstall;

        /// <summary>
        /// Initializes a new instance of the <see cref="SevenZipArchiveEntryViewModel"/> class.
        /// </summary>
        /// <param name="archive">The archive<see cref="SevenZipArchiveViewModel"/>.</param>
        /// <param name="entry">The entry<see cref="IArchiveEntry"/>.</param>
        /// <param name="parent">The parent<see cref="SevenZipArchiveEntryViewModel"/>.</param>
        public SevenZipArchiveEntryViewModel(SevenZipArchiveViewModel archive, IArchiveEntry entry, SevenZipArchiveEntryViewModel parent = null)
        {
            this.archiveRoot = archive;
            Entry = entry;
            Parent = parent;
        }

        /// <summary>
        /// Gets the Entries.
        /// </summary>
        public List<IArchiveEntryViewModel> Entries
        {
            get
            {
                if (sortedEntries == null)
                    return null;

                if (entries == null)
                {
                    entries = new List<IArchiveEntryViewModel>(SortedEntries.Values);
                }

                return entries;
            }
        }

        /// <summary>
        /// Gets or sets the Entry.
        /// </summary>
        public IArchiveEntry Entry { get; set; }

        /// <summary>
        /// Gets the FullName.
        /// </summary>
        public string FullName => archiveRoot.FullName == "{root}" ? Entry.Name : $"{archiveRoot}\\{Entry.Name}";

        /// <summary>
        /// Gets a value indicating whether IsDirectory.
        /// </summary>
        public bool IsDirectory => Entry.IsDirectory;

        /// <summary>
        /// Gets the LastWriteTime.
        /// </summary>
        public DateTimeOffset LastWriteTime => Entry.LastWriteTime;

        /// <summary>
        /// Gets the Length.
        /// </summary>
        public long? Length => Entry.IsDirectory ? null : Entry.Length;

        /// <summary>
        /// Gets the Name.
        /// </summary>
        public string Name => Entry.Name;

        /// <summary>
        /// Gets the Parent.
        /// </summary>
        public IArchiveEntryViewModel Parent { get; private set; }

        /// <summary>
        /// Gets the SortedEntries.
        /// </summary>
        public SortedList<string, IArchiveEntryViewModel> SortedEntries
        {
            get
            {
                if (IsDirectory == false)
                {
                    return sortedEntries;
                }

                if (sortedEntries == null)
                {
                    sortedEntries = new SortedList<string, IArchiveEntryViewModel>();

                    var dirnames = Directory.GetDirectories(Path.Combine(SourcePath, Name));
                    var filenames = Directory.GetFiles(Path.Combine(SourcePath, Name));

                    foreach (var item in dirnames)
                    {
                        var di = new DirectoryInfo(item);
                        sortedEntries.Add($"_{di.Name}", new SevenZipArchiveEntryViewModel(archiveRoot, new SevenZipArchiveEntry(di), this));
                    }

                    foreach (var item in filenames)
                    {
                        var fi = new FileInfo(item);
                        sortedEntries.Add(fi.Name, new SevenZipArchiveEntryViewModel(archiveRoot, new SevenZipArchiveEntry(fi), this));
                    }
                }

                return sortedEntries;
            }
        }

        /// <summary>
        /// Gets the SourcePath.
        /// </summary>
        public string SourcePath
        {
            get
            {
                if (Parent == null)
                    return archiveRoot.SourcePath;
                return Path.Combine((Parent as SevenZipArchiveEntryViewModel).SourcePath, Parent.Name);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether WillInstall.
        /// </summary>
        public bool WillInstall { get => willInstall; set => SetProperty(ref willInstall, value); }

        /// <summary>
        /// The SaveBranchAndNode.
        /// </summary>
        /// <param name="entry">The entry<see cref="IArchiveEntry"/>.</param>
        /// <param name="fullName">The fullName<see cref="string"/>.</param>
        /// <param name="pathParts">The pathParts<see cref="List{string}"/>.</param>
        public void SaveBranchAndNode(IArchiveEntry entry, string fullName, List<string> pathParts)
        {
            throw new NotImplementedException();
        }
    }
}
