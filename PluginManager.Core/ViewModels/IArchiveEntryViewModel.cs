namespace PluginManager.Core.ViewModels
{
    using global::System;
    using global::System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="IArchiveEntryViewModel" />.
    /// </summary>
    public interface IArchiveEntryViewModel
    {
        /// <summary>
        /// Gets or sets the Entry.
        /// </summary>
        IArchiveEntry Entry { get; set; }

        /// <summary>
        /// Gets the FullName.
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Gets the IsDirectory
        /// Gets a value indicating whether IsDirectory..
        /// </summary>
        bool IsDirectory { get; }

        /// <summary>
        /// Gets the LastWriteTime.
        /// </summary>
        DateTimeOffset LastWriteTime { get; }

        /// <summary>
        /// Gets the Length.
        /// </summary>
        long? Length { get; }

        /// <summary>
        /// Gets the Name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the Parent.
        /// </summary>
        IArchiveEntryViewModel Parent { get; }

        /// <summary>
        /// Gets the SortedEntries.
        /// </summary>
        SortedList<string, IArchiveEntryViewModel> SortedEntries { get; }

        /// <summary>
        /// Gets or sets the WillInstall
        /// Gets or sets a value indicating whether WillInstall..
        /// </summary>
        bool WillInstall { get; set; }

        /// <summary>
        /// The SaveBranchAndNode.
        /// </summary>
        /// <param name="entry">The entry<see cref="IArchiveEntry"/>.</param>
        /// <param name="fullName">The fullName<see cref="string"/>.</param>
        /// <param name="pathParts">The pathParts<see cref="List{string}"/>.</param>
        void SaveBranchAndNode(IArchiveEntry entry, string fullName, List<string> pathParts);
    }
}
