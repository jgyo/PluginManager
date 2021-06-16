namespace PluginManager.Core.Utilities
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Diagnostics;
    using global::System.IO;
    using global::System.Linq;
    using PluginManager.Core.ViewModels;

    /// <summary>
    /// Defines the <see cref="ZipArchiveExtensions" />.
    /// </summary>
    public static class ZipArchiveExtensions
    {
        /// <summary>
        /// The Checks to see if any checked entries are installed.
        /// </summary>
        /// <param name="vm">The vm<see cref="IArchiveEntryViewModel"/>.</param>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool AreAnyCheckedEntriesInstalled(this IArchiveEntryViewModel vm, string path)
        {
            return vm.SortedEntries.Values.Where(e => e.WillInstall).Any(e => e.IsInstalled(path));
        }

        /// <summary>
        /// The Checks to see if any checked entries are installed.
        /// </summary>
        /// <param name="vm">The vm<see cref="IArchiveViewModel"/>.</param>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool AreAnyCheckedEntriesInstalled(this IArchiveViewModel vm, string path)
        {
            if (vm.SelectedDirectory is IArchiveEntryViewModel)
                return (vm.SelectedDirectory as IArchiveEntryViewModel).AreAnyCheckedEntriesInstalled(path);

            return vm.SortedEntries.Values.Where(e => e.WillInstall).Any(e => e.IsInstalled(path));
        }

        /// <summary>
        /// The Full Normal Name.
        /// </summary>
        /// <param name="entry">The entry<see cref="IArchiveEntryViewModel"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string FullNormalName(this IArchiveEntryViewModel entry)
        {
            return entry.FullName.Trim('/').Replace('/', '\\');
        }

        /// <summary>
        /// Install Checked Entries.
        /// </summary>
        /// <param name="ivm">The ivm<see cref="IArchiveViewModel"/>.</param>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <param name="overwrite">The overwrite<see cref="bool"/>.</param>
        /// <returns>The <see cref="IEnumerable{string}"/>.</returns>
        public static IEnumerable<string> InstallCheckedEntries(this IArchiveViewModel ivm, string path, bool overwrite = false)
        {
            if (ivm is ZipArchiveViewModel zvm)
            {
                return zvm.InstallCheckedEntries(path, overwrite);
            }

            if (ivm is SevenZipArchiveViewModel svm)
            {
                return svm.InstallCheckedEntries(path, overwrite);
            }

            throw new ArgumentException("Null or unknown view model.", nameof(ivm));
        }

        /// <summary>
        /// Install Checked Entries.
        /// </summary>
        /// <param name="vm">The vm<see cref="SevenZipArchiveEntryViewModel"/>.</param>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <param name="overwrite">The overwrite<see cref="bool"/>.</param>
        /// <returns>The <see cref="IEnumerable{string}"/>.</returns>
        public static IEnumerable<string> InstallCheckedEntries(this SevenZipArchiveEntryViewModel vm, string path, bool overwrite = false)
        {
            if (overwrite == false)
                Debug.Assert(vm.AreAnyCheckedEntriesInstalled(path) == false);

            foreach (var entry in vm.SortedEntries.Values.Where(e => e.WillInstall))
            {
                if (entry.IsDirectory)
                {
                    DirectoryCopy(entry.FullName, path);
                    yield return entry.Name;
                }
            }
        }

        /// <summary>
        /// Install Checked Entries.
        /// </summary>
        /// <param name="vm">The vm<see cref="SevenZipArchiveViewModel"/>.</param>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <param name="overwrite">The overwrite<see cref="bool"/>.</param>
        /// <returns>The <see cref="IEnumerable{string}"/>.</returns>
        public static IEnumerable<string> InstallCheckedEntries(this SevenZipArchiveViewModel vm, string path, bool overwrite = false)
        {
            if (vm.SelectedDirectory is SevenZipArchiveEntryViewModel svm)
            {
                foreach (var item in svm.InstallCheckedEntries(path, overwrite))
                {
                    yield return item;
                }

                yield break;
            }

            if (overwrite == false)
                Debug.Assert(vm.AreAnyCheckedEntriesInstalled(path) == false);

            var installedFolders = new List<Tuple<int, string, DateTime>>();
            foreach (var entry in vm.SortedEntries.Values.Where(e => e.WillInstall))
            {
                var sentry = entry as SevenZipArchiveEntryViewModel;
                sentry.InstallEntry(path);
            }
        }

        /// <summary>
        /// Install Checked Entries.
        /// </summary>
        /// <param name="vm">The vm<see cref="ZipArchiveEntryViewModel"/>.</param>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <param name="overwrite">The overwrite<see cref="bool"/>.</param>
        /// <returns>The <see cref="IEnumerable{string}"/>.</returns>
        public static IEnumerable<string> InstallCheckedEntries(this ZipArchiveEntryViewModel vm, string path, bool overwrite = false)
        {
            if (overwrite == false)
                Debug.Assert(vm.AreAnyCheckedEntriesInstalled(path) == false);

            foreach (var entry in vm.SortedEntries.Values.Where(e => e.WillInstall))
            {
                var zentry = entry as ZipArchiveEntryViewModel;
                zentry.InstallEntry(path, overwrite);
                if (zentry.IsDirectory)
                {
                    yield return entry.Name;
                    var newpath = Path.Combine(path, entry.Name);
                    zentry.InstallEntries(newpath, overwrite);
                }
            }
        }

        /// <summary>
        /// Install Checked Entries.
        /// </summary>
        /// <param name="vm">The vm<see cref="ZipArchiveViewModel"/>.</param>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <param name="overwrite">The overwrite<see cref="bool"/>.</param>
        /// <returns>The <see cref="IEnumerable{string}"/>.</returns>
        public static IEnumerable<string> InstallCheckedEntries(this ZipArchiveViewModel vm, string path, bool overwrite = false)
        {
            if (vm.SelectedDirectory is ZipArchiveEntryViewModel zvm)
            {
                foreach (var item in zvm.InstallCheckedEntries(path, overwrite))
                {
                    yield return item;
                }

                yield break;
            }

            if (overwrite == false)
                Debug.Assert(vm.AreAnyCheckedEntriesInstalled(path) == false);

            var installedFolders = new List<Tuple<int, string, DateTime>>();
            foreach (var entry in vm.SortedEntries.Values.Where(e => e.WillInstall))
            {
                var zentry = entry as ZipArchiveEntryViewModel;
                zentry.InstallEntry(path, overwrite);
                if (zentry.IsDirectory)
                {
                    yield return entry.Name;
                    var newpath = Path.Combine(path, entry.Name);
                    zentry.InstallEntries(newpath, overwrite);
                }
            }
        }

        /// <summary>
        /// Install Entries.
        /// </summary>
        /// <param name="vm">The vm<see cref="ZipArchiveEntryViewModel"/>.</param>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <param name="overwrite">The overwrite<see cref="bool"/>.</param>
        public static void InstallEntries(this ZipArchiveEntryViewModel vm, string path, bool overwrite = false)
        {
            if (overwrite == false)
                Debug.Assert(vm.AreAnyCheckedEntriesInstalled(path) == false);

            foreach (var entry in vm.SortedEntries.Values)
            {
                var zentry = entry as ZipArchiveEntryViewModel;
                zentry.InstallEntry(path, overwrite);
                if (entry.IsDirectory)
                {
                    var newpath = Path.Combine(path, entry.Name);
                    zentry.InstallEntries(newpath, overwrite);
                }
            }
        }

        /// <summary>
        /// Install Entries.
        /// </summary>
        /// <param name="vm">The vm<see cref="SevenZipArchiveEntryViewModel"/>.</param>
        /// <param name="path">The path<see cref="string"/>.</param>
        public static void InstallEntry(this SevenZipArchiveEntryViewModel vm, string path)
        {
            string fullName = Path.Combine(path, vm.Name);
            if (vm.IsDirectory)
            {
                DirectoryCopy(vm.FullName, fullName);
                return;
            }

            FileCopy(vm.FullName, fullName);
        }

        /// <summary>
        /// Install Entries.
        /// </summary>
        /// <param name="vm">The vm<see cref="ZipArchiveEntryViewModel"/>.</param>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <param name="overwrite">The overwrite<see cref="bool"/>.</param>
        public static void InstallEntry(this ZipArchiveEntryViewModel vm, string path, bool overwrite = false)
        {
            string fullName = Path.Combine(path, vm.Name);
            if (vm.IsDirectory)
            {
                if (overwrite && Directory.Exists(fullName))
                    return;

                Directory.CreateDirectory(fullName);
                return;
            }

            vm.Entry.ExtractToFile(fullName, overwrite);
        }

        /// <summary>
        /// Install Entries.
        /// </summary>
        /// <param name="entry">The entry<see cref="IArchiveEntryViewModel"/>.</param>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsInstalled(this IArchiveEntryViewModel entry, string path)
        {
            if (entry.IsDirectory)
                return Directory.Exists(Path.Combine(path, entry.Name));

            return File.Exists(Path.Combine(path, entry.Name));
        }

        /// <summary>
        /// Directory Copy.
        /// </summary>
        /// <param name="sourceDirName">The sourceDirName<see cref="string"/>.</param>
        /// <param name="destDirName">The destDirName<see cref="string"/>.</param>
        private static void DirectoryCopy(string sourceDirName, string destDirName)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, tempPath);
            }
        }

        /// <summary>
        /// File Copy.
        /// </summary>
        /// <param name="sourceFileName">The sourceFileName<see cref="string"/>.</param>
        /// <param name="destFileName">The destFileName<see cref="string"/>.</param>
        private static void FileCopy(string sourceFileName, string destFileName)
        {
            FileInfo fi = new(sourceFileName);

            if (fi.Exists == false)
            {
                throw new FileNotFoundException(
                    $"Source file does not exist or could not be found: {sourceFileName}");
            }

            fi.CopyTo(destFileName, true);
        }
    }
}
