namespace PluginManager.Core.ViewModels
{
    using global::System;
    using global::System.IO.Compression;

    /// <summary>
    /// Defines the <see cref="MyZipArchiveEntry" />.
    /// </summary>
    internal class MyZipArchiveEntry : IArchiveEntry
    {
        /// <summary>
        /// Defines the entry.
        /// </summary>
        private ZipArchiveEntry entry;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyZipArchiveEntry"/> class.
        /// </summary>
        /// <param name="entry">The entry<see cref="ZipArchiveEntry"/>.</param>
        public MyZipArchiveEntry(ZipArchiveEntry entry)
        {
            this.entry = entry;
        }

        /// <summary>
        /// Gets the LastWriteTime.
        /// </summary>
        public DateTimeOffset LastWriteTime => entry.LastWriteTime;

        /// <summary>
        /// Gets the Length.
        /// </summary>
        public long Length { get => entry.Length; }

        /// <summary>
        /// Gets the Name.
        /// </summary>
        public string Name { get => entry.Name; }

        /// <summary>
        /// The ExtractToFile.
        /// </summary>
        /// <param name="fullName">The fullName<see cref="string"/>.</param>
        /// <param name="overwrite">The overwrite<see cref="bool"/>.</param>
        public void ExtractToFile(string fullName, bool overwrite)
        {
            entry.ExtractToFile(fullName, overwrite);
        }
    }
}
