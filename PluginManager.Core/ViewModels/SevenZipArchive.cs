

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
        private string tempPath;

        /// <summary>
        /// Defines the entries.
        /// </summary>
        private List<SevenZipArchiveEntry> entries = new List<SevenZipArchiveEntry>();

        /// <summary>
        /// Defines the path.
        /// </summary>
        private string path;

        /// <summary>
        /// Prevents a default instance of the <see cref="SevenZipArchive"/> class from being created.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        private SevenZipArchive(string path)
        {
            this.path = path;
            Entries = new ReadOnlyCollection<SevenZipArchiveEntry>(entries);
            this.tempPath = Path.GetTempPath() + "pluginManager";
            if(Directory.Exists(tempPath))
            {
                Directory.Delete(tempPath, true);
            }

            var pi = new ProcessStartInfo("7z.exe", $"x -o{tempPath} \"{path}\"");
            pi.CreateNoWindow = true;
            pi.UseShellExecute = true;

            using (var pr = Process.Start(pi))
            {
                pr.WaitForExit();
            }

        }

        /// <summary>
        /// The Open.
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
