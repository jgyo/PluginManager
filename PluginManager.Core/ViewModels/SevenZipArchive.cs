namespace PluginManager.Core.ViewModels
{
    using global::System.Collections.Generic;
    using global::System.Collections.ObjectModel;
    using global::System.Diagnostics;
    using global::System.IO;

    /// <summary>
    /// Defines the <see cref="SevenZipArchive" />.
    /// </summary>
    public class SevenZipArchive
    {
        /// <summary>
        /// Defines the Entries.
        /// </summary>
        public ReadOnlyCollection<SevenZipArchiveEntry> Entries;

        /// <summary>
        /// Defines the entries.
        /// </summary>
        private readonly List<SevenZipArchiveEntry> entries = new();

        /// <summary>
        /// Temporary path for the extracted files.
        /// </summary>
        private readonly string extractedFiles;

        /// <summary>
        /// Prevents a default instance of the <see cref="SevenZipArchive"/> class from being created.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        private SevenZipArchive(string path)
        {
            Entries = new ReadOnlyCollection<SevenZipArchiveEntry>(entries);
            // Extracted files path
            this.extractedFiles = Path.GetTempPath() + "pluginManager";
            if (Directory.Exists(extractedFiles))
            {
                Directory.Delete(extractedFiles, true);
            }

            var pi = new ProcessStartInfo("7z.exe", $"x -o{extractedFiles} \"{path}\"")
            {
                CreateNoWindow = true,
                UseShellExecute = false
            };

            using (var pr = Process.Start(pi))
            {
                pr.WaitForExit();
            }

            var dirNames = Directory.GetDirectories(extractedFiles);
            var fileNames = Directory.GetFiles(extractedFiles);

            foreach (var item in dirNames)
            {
                var di = new DirectoryInfo(Path.Combine(extractedFiles, item));
                entries.Add(new SevenZipArchiveEntry(di));
            }

            foreach (var item in fileNames)
            {
                var fi = new FileInfo(Path.Combine(extractedFiles, item));
                entries.Add(new SevenZipArchiveEntry(fi));
            }
        }

        /// <summary>
        /// Gets the Extracted Files path.
        /// </summary>
        public string ExtractedFiles { get => extractedFiles; }

        /// <summary>
        /// Open a seven zip archive.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="SevenZipArchive"/>.</returns>
        public static SevenZipArchive Open(string path)
        {
            try
            {
                return new SevenZipArchive(path);
            }
            catch
            {
                return null;
            }
        }
    }
}
