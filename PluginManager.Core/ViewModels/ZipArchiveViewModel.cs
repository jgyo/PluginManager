

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
    public class ZipArchiveViewModel : ViewModel, IArchiveDirectoryEntry, IArchiveViewModel
    {
        private IArchiveDirectoryEntry selectedDirectory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZipArchiveViewModel"/> class.
        /// </summary>
        /// <param name="fileName">The fileName<see cref="string"/>.</param>
        /// <param name="path">The path<see cref="string"/>.</param>
        public ZipArchiveViewModel(string fileName, string path, long packageId)
        {
            SelectedDirectory = this;

            FileName = fileName;
            Path = global::System.IO.Path.GetFullPath(path);
            PackageId = packageId;

            var archive = ZipFile.Open(FullPath, ZipArchiveMode.Read);

            var spliters = new[] {'\\', '/'};

            foreach (var entry in archive.Entries)
            {
                var pathParts = entry.FullName.Split(spliters, StringSplitOptions.RemoveEmptyEntries).ToList();
                var fullName = pathParts[0];
                var e = new MyZipArchiveEntry(entry);
                SaveBranchAndNode(e, fullName, pathParts);
            }

            archive.Dispose();
        }

        public List<IArchiveEntryViewModel> Entries
            => new Lazy<List<IArchiveEntryViewModel>>(() => new List<IArchiveEntryViewModel>(SortedEntries.Values)).Value;

        /// <summary>
        /// Gets the FileName.
        /// </summary>
        public string FileName { get; private set; }

        public string FullName { get => "{Root}"; }

        /// <summary>
        /// Gets the FullPath.
        /// </summary>
        public string FullPath
        {
            get { return global::System.IO.Path.Combine(Path, FileName); }
        }

        public long PackageId { get; private set; }

        /// <summary>
        /// Gets the Path.
        /// </summary>
        public string Path { get; private set; }

        public IArchiveDirectoryEntry SelectedDirectory
        {
            get => selectedDirectory; set => SetProperty(ref selectedDirectory, value);
        }
        /// <summary>
        /// Gets the Entries.
        /// </summary>
        public SortedList<string, IArchiveEntryViewModel> SortedEntries { get; } = new SortedList<string, IArchiveEntryViewModel>();
        /// <summary>
        /// The SaveBranchAndNode.
        /// </summary>
        /// <param name="entry">The entry<see cref="ZipArchiveEntry"/>.</param>
        /// <param name="fullName">The fullName<see cref="string"/>.</param>
        /// <param name="pathParts">The pathParts<see cref="List{string}"/>.</param>
        private void SaveBranchAndNode(IArchiveEntry entry, string fullName, List<string> pathParts)
        {
            ZipArchiveEntryViewModel parent = null;

            var vm = SortedEntries.Values.Where(e => "\\" + fullName == e.FullName).SingleOrDefault();
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
