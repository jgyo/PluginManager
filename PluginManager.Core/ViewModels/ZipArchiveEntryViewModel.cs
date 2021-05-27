﻿namespace PluginManager.Core.ViewModels
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Collections.ObjectModel;
    using global::System.IO.Compression;
    using global::System.Linq;

    /// <summary>
    /// Defines the <see cref="ZipArchiveEntryViewModel" />.
    /// </summary>
    public class ZipArchiveEntryViewModel : ViewModel, IArchiveDirectoryEntry
    {
        /// <summary>
        /// Defines the willInstall.
        /// </summary>
        private bool willInstall;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZipArchiveEntryViewModel"/> class.
        /// </summary>
        /// <param name="archive">The archive<see cref="ZipArchiveViewModel"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="parent">The parent<see cref="ZipArchiveEntryViewModel"/>.</param>
        public ZipArchiveEntryViewModel(ZipArchiveViewModel archive, string name, bool isDirectory, ZipArchiveEntryViewModel parent = null)
        {
            Archive = archive;
            Parent = parent;
            Name = name;
            var sortingName = isDirectory ? $"_{Name}" : name;
            if (parent == null)
                archive.SortedEntries.Add(sortingName, this);
            else
                Parent.SortedEntries.Add(sortingName, this);
        }

        /// <summary>
        /// Gets the Archive.
        /// </summary>
        public ZipArchiveViewModel Archive { get; }

        /// <summary>
        /// Gets the CompressedLength.
        /// </summary>
        public long CompressedLength
        {
            get
            {
                if (Entry != null && Entry.CompressedLength != 0)
                    return Entry.CompressedLength;

                if (SortedEntries.Count == 0)
                    return 0;

                return SortedEntries.Values.Select(v => v.CompressedLength).Aggregate(0L, (t, n) => t + n);
            }
        }

        /// <summary>
        /// Gets the Entries.
        /// </summary>
        public SortedList<string, ZipArchiveEntryViewModel> SortedEntries { get; } = new SortedList<string, ZipArchiveEntryViewModel>();

        /// <summary>
        /// Gets the Entry.
        /// </summary>
        public ZipArchiveEntry Entry { get; set; }

        /// <summary>
        /// Gets the FullName.
        /// </summary>
        public string FullName
        {
            get { return $"{RelativePath}{Name}"; }
        }

        /// <summary>
        /// Gets a value indicating whether IsDirectory.
        /// </summary>
        public bool IsDirectory
        {
            get
            {
                if (Entry == null)
                    return true;

                return Entry.Name == "";
            }
        }

        /// <summary>
        /// Gets the LastWriteTime.
        /// </summary>
        public DateTimeOffset LastWriteTime
        {
            get
            {
                if (Entry != null)
                    return Entry.LastWriteTime;

                if (SortedEntries.Count == 0)
                    return DateTimeOffset.MinValue;

                return SortedEntries.Values.Select(m => m.LastWriteTime).Aggregate(DateTimeOffset.MinValue, (m, t) => m > t ? m : t);
            }
        }

        /// <summary>
        /// Gets the Length.
        /// </summary>
        public long Length
        {
            get
            {
                if (Entry != null && Entry.Length != 0)
                    return Entry.Length;

                if (SortedEntries.Count == 0)
                    return 0;

                return SortedEntries.Values.Select(v => v.Length).Aggregate(0L, (m, t) => m + t);
            }
        }

        /// <summary>
        /// Gets the Name.
        /// </summary>
        public string Name { get; private set; }

        public string SortingName { get => IsDirectory ? $"_{Name}" : Name; }

        /// <summary>
        /// Gets the Parent.
        /// </summary>
        public ZipArchiveEntryViewModel Parent { get; }

        /// <summary>
        /// Gets the RelativePath.
        /// </summary>
        public string RelativePath
        {
            get { return Parent == null ? "" : Parent.FullName + "\\"; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether WillInstall.
        /// </summary>
        public bool WillInstall
        {
            get { return willInstall; }
            set { SetProperty(ref willInstall, value); }
        }

        public List<ZipArchiveEntryViewModel> Entries 
            => new Lazy<List<ZipArchiveEntryViewModel>>(() => new List<ZipArchiveEntryViewModel>(SortedEntries.Values)).Value;

        /// <summary>
        /// The SaveBranchAndNode.
        /// </summary>
        /// <param name="zipArchiveViewModel">The zipArchiveViewModel<see cref="ZipArchiveViewModel"/>.</param>
        /// <param name="entry">The entry<see cref="ZipArchiveEntry"/>.</param>
        /// <param name="fullName">The fullName<see cref="string"/>.</param>
        /// <param name="pathParts">The pathParts<see cref="List{string}"/>.</param>
        internal void SaveBranchAndNode(ZipArchiveEntry entry, string fullName, List<string> pathParts)
        {
            ZipArchiveEntryViewModel parent = this;

            var vm = SortedEntries.Values.Where(e => fullName == e.FullName).SingleOrDefault();
            if (vm == null)
            {
                vm = new ZipArchiveEntryViewModel(Archive, pathParts[0], entry.Name=="", parent);
                // Entries.Add(vm);
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