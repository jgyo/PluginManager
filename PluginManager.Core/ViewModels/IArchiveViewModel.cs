using System.Collections.Generic;

namespace PluginManager.Core.ViewModels
{
    public interface IArchiveViewModel
    {
        IArchiveDirectoryEntry SelectedDirectory { get; set; }
        SortedList<string, IArchiveEntryViewModel> SortedEntries { get; }
        string FileName { get; }
        long PackageId { get; }
        string FullPath { get; }
        string Path { get; }
        List<IArchiveEntryViewModel> Entries { get; }
    }
}