

namespace PluginManager.Core.ViewModels
{
    using global::System.Collections.Generic;
    using global::System.Collections.ObjectModel;

    public interface IArchiveDirectoryEntry
    {
        List<ZipArchiveEntryViewModel> Entries { get; }
        string FullName { get; }
    }
}
