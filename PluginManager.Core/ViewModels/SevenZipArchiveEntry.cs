namespace PluginManager.Core.ViewModels
{
    using global::System;
    using global::System.IO;

    /// <summary>
    /// Defines the <see cref="SevenZipArchiveEntry" />.
    /// </summary>
    public class SevenZipArchiveEntry : IArchiveEntry
    {

        /// <summary>
        /// Defines the directoryInfo.
        /// </summary>
        private readonly DirectoryInfo directoryInfo;

        /// <summary>
        /// Defines the fileInfo.
        /// </summary>
        private readonly FileInfo fileInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="SevenZipArchiveEntry"/> class.
        /// </summary>
        /// <param name="di">The di<see cref="DirectoryInfo"/>.</param>
        public SevenZipArchiveEntry(DirectoryInfo di)
        {
            directoryInfo = di;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SevenZipArchiveEntry"/> class.
        /// </summary>
        /// <param name="fi">The fi<see cref="FileInfo"/>.</param>
        public SevenZipArchiveEntry(FileInfo fi)
        {
            fileInfo = fi;
        }

        /// <summary>
        /// Gets a value indicating whether IsDirectory.
        /// </summary>
        public bool IsDirectory { get => directoryInfo != null; }

        /// <summary>
        /// Gets the LastWriteTime.
        /// </summary>
        public DateTimeOffset LastWriteTime { get => IsDirectory ? directoryInfo.CreationTime : fileInfo.CreationTime; }

        /// <summary>
        /// Gets the Length.
        /// </summary>
        public long Length { get => IsDirectory ? -1L : fileInfo.Length; }

        /// <summary>
        /// Gets the Name.
        /// </summary>
        public string Name { get => IsDirectory ? directoryInfo.Name : fileInfo.Name; }

        public string ArchivePath
        {
            get
            {
                string fullName;
                if (IsDirectory)
                {
                    fullName = directoryInfo.FullName;
                }
                else
                {
                    fullName = fileInfo.FullName;
                }

                return fullName;
            }
        }
        /// <summary>
        /// Extract To File.
        /// </summary>
        /// <param name="destination">The destination<see cref="string"/>.</param>
        /// <param name="overwrite">The overwrite<see cref="bool"/>.</param>
        public void ExtractToFile(string destination, bool overwrite)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }
            if (string.IsNullOrWhiteSpace(destination) || IsInvalidPath(destination))
            {
                throw new ArgumentException("destination is a zero-length string, contains only white space, or contains one or more invalid characters", nameof(destination));
            }

            var filename = Path.Combine(destination, fileInfo.Name);
            fileInfo.CopyTo(filename, overwrite);
        }

        /// <summary>
        /// The IsInvalidPath.
        /// </summary>
        /// <param name="destination">The destination<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private static bool IsInvalidPath(string destination)
        {
            foreach (var item in Path.GetInvalidPathChars())
            {
                if (destination.Contains(item))
                    return true;
            }
            return false;
        }
    }
}
