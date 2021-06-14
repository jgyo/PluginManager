using PluginManager.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginManager.Core.Utilities
{
    public static class ZipArchiveExtensions
    {
        public static string FullNormalName(this IArchiveEntryViewModel entry)
        {
            return entry.FullName.Trim('/').Replace('/', '\\');
        }
        public static bool IsInstalled(this IArchiveEntryViewModel entry, string path)
        {
            if (entry.IsDirectory)
                return Directory.Exists(Path.Combine(path, entry.Name));

            return File.Exists(Path.Combine(path, entry.Name));
        }

        public static bool AreAnyCheckedEntriesInstalled(this IArchiveViewModel vm, string path)
        {
            if (vm.SelectedDirectory is IArchiveEntryViewModel)
                return (vm.SelectedDirectory as IArchiveEntryViewModel).AreAnyCheckedEntriesInstalled(path);

            return vm.SortedEntries.Values.Where(e => e.WillInstall).Any(e => e.IsInstalled(path));
        }

        public static bool AreAnyCheckedEntriesInstalled(this IArchiveEntryViewModel vm, string path)
        {
            return vm.SortedEntries.Values.Where(e => e.WillInstall).Any(e => e.IsInstalled(path));
        }

        public static IEnumerable<string> InstallCheckedEntries(this IArchiveViewModel vm, string path, bool overwrite = false)
        {
            if (vm.SelectedDirectory is IArchiveEntryViewModel)
            {
                foreach(var item in (vm.SelectedDirectory as IArchiveEntryViewModel).InstallCheckedEntries(path, overwrite))
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
                entry.InstallEntry(path, overwrite);
                if (entry.IsDirectory)
                {
                    yield return entry.Name;
                    var newpath = Path.Combine(path, entry.Name);
                    entry.InstallEntries(newpath, overwrite);
                }
            }
        }

        public static IEnumerable<string> InstallCheckedEntries(this IArchiveEntryViewModel vm, string path, bool overwrite = false)
        {
            if (overwrite == false)
                Debug.Assert(vm.AreAnyCheckedEntriesInstalled(path) == false);

            foreach (var entry in vm.SortedEntries.Values.Where(e => e.WillInstall))
            {
                entry.InstallEntry(path, overwrite);
                if (entry.IsDirectory)
                {
                    yield return entry.Name;
                    var newpath = Path.Combine(path, entry.Name);
                    entry.InstallEntries(newpath, overwrite);
                }
            }
        }

        public static void InstallEntry(this IArchiveEntryViewModel vm, string path, bool overwrite = false)
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
        public static void InstallEntries(this IArchiveEntryViewModel vm, string path, bool overwrite = false)
        {
            if (overwrite == false)
                Debug.Assert(vm.AreAnyCheckedEntriesInstalled(path) == false);

            foreach (var entry in vm.SortedEntries.Values)
            {
                entry.InstallEntry(path, overwrite);
                if (entry.IsDirectory)
                {
                    var newpath = Path.Combine(path, entry.Name);
                    entry.InstallEntries(newpath, overwrite);
                }
            }
        }


    }
}
