

namespace PluginManager.Core.ViewModels
{
    using global::System.Collections.ObjectModel;

    public interface IArchiveDirectoryEntry
    {
        ObservableCollection<ZipArchiveEntryViewModel> Entries { get; }
        string FullName { get; }
    }
}
