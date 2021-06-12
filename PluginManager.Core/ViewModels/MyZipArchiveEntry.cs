using System;
using System.IO.Compression;

namespace PluginManager.Core.ViewModels
{
    internal class MyZipArchiveEntry : IArchiveEntry
    {
        private ZipArchiveEntry entry;

        public MyZipArchiveEntry(ZipArchiveEntry entry)
        {
            this.entry = entry;
        }

        public string Name { get => entry.Name; }

        public DateTimeOffset LastWriteTime => entry.LastWriteTime;

        public long Length { get => entry.Length; }

        public void ExtractToFile(string fullName, bool overwrite)
        {
            entry.ExtractToFile(fullName, overwrite);
        }
    }
}