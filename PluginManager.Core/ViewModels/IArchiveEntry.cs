namespace PluginManager.Core.ViewModels
{
    using global::System;

    /// <summary>
    /// Defines the <see cref="IArchiveEntry" />.
    /// </summary>
    public interface IArchiveEntry
    {
        /// <summary>
        /// Gets a value indicating whether IsDirectory.
        /// </summary>
        bool IsDirectory { get; }

        /// <summary>
        /// Gets the LastWriteTime.
        /// </summary>
        DateTimeOffset LastWriteTime { get; }

        /// <summary>
        /// Gets the Length.
        /// </summary>
        long Length { get; }

        /// <summary>
        /// Gets the Name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The ExtractToFile.
        /// </summary>
        /// <param name="fullName">The fullName<see cref="string"/>.</param>
        /// <param name="overwrite">The overwrite<see cref="bool"/>.</param>
        public void ExtractToFile(string fullName, bool overwrite);
    }
}
