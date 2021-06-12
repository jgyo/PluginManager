using System;

namespace PluginManager.Core.ViewModels
{
    public interface IArchiveEntry
    {
        string Name { get; }
        DateTimeOffset LastWriteTime { get; }
        long Length { get; }

        public void ExtractToFile(string fullName, bool overwrite);
    }
}