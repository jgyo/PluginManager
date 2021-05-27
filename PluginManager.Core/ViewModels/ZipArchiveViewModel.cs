

namespace PluginManager.Core.ViewModels
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Collections.ObjectModel;
    using global::System.IO.Compression;
    using global::System.Linq;
    using PluginManager.Core.Utilities;

    /// <summary>
    /// Defines the <see cref="ZipArchiveViewModel" />.
    /// </summary>
    public class ZipArchiveViewModel : ViewModel, IArchiveDirectoryEntry
    {
        private IArchiveDirectoryEntry selectedDirectory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZipArchiveViewModel"/> class.
        /// </summary>
        /// <param name="fileName">The fileName<see cref="string"/>.</param>
        /// <param name="path">The path<see cref="string"/>.</param>
        public ZipArchiveViewModel(string fileName, string path)
        {
            SelectedDirectory = this;

            FileName = fileName;
            Path = global::System.IO.Path.GetFullPath(path);
            var archive = ZipFile.Open(FullPath, ZipArchiveMode.Read);

            var spliters = new[] {'\\', '/'};

            foreach (var entry in archive.Entries)
            {
                var pathParts = entry.FullNormalName().Split(spliters, StringSplitOptions.RemoveEmptyEntries).ToList();
                var fullName = pathParts[0];
                SaveBranchAndNode(entry, fullName, pathParts);
            }
        }

        public IArchiveDirectoryEntry SelectedDirectory
        {
            get => selectedDirectory; set => SetProperty(ref selectedDirectory, value);
        }

        public string FullName { get => "{Root}"; }

        /// <summary>
        /// Gets the Entries.
        /// </summary>
        public SortedList<string, ZipArchiveEntryViewModel> SortedEntries { get; } = new SortedList<string, ZipArchiveEntryViewModel>();

        /// <summary>
        /// Gets the FileName.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Gets the FullPath.
        /// </summary>
        public string FullPath
        {
            get { return global::System.IO.Path.Combine(Path, FileName); }
        }

        /// <summary>
        /// Gets the Path.
        /// </summary>
        public string Path { get; private set; }

        public List<ZipArchiveEntryViewModel> Entries
            => new Lazy<List<ZipArchiveEntryViewModel>>(() => new List<ZipArchiveEntryViewModel>(SortedEntries.Values)).Value;

        /// <summary>
        /// The SaveBranchAndNode.
        /// </summary>
        /// <param name="entry">The entry<see cref="ZipArchiveEntry"/>.</param>
        /// <param name="fullName">The fullName<see cref="string"/>.</param>
        /// <param name="pathParts">The pathParts<see cref="List{string}"/>.</param>
        private void SaveBranchAndNode(ZipArchiveEntry entry, string fullName, List<string> pathParts)
        {
            ZipArchiveEntryViewModel parent = null;

            var vm = SortedEntries.Values.Where(e => fullName == e.FullName).SingleOrDefault();
            if (vm == null)
            {
                vm = new ZipArchiveEntryViewModel(this, pathParts[0], entry.Name=="", parent);
                //Entries.Add(vm);
            }

            pathParts.RemoveAt(0);
            if (pathParts.Count == 0)
            {
                vm.Entry = entry;
                return;
            }

            fullName = $"{vm.FullName}\\{pathParts[0]}";
            vm.SaveBranchAndNode(entry, fullName, pathParts);
        }
    }
}
