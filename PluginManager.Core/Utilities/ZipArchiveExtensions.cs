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
        public static string FullNormalName(this ZipArchiveEntryViewModel entry)
        {
            return entry.FullName.Trim('/').Replace('/', '\\');
        }
        public static bool IsInstalled(this ZipArchiveEntryViewModel entry, string path)
        {
            if (entry.IsDirectory)
                return Directory.Exists(Path.Combine(path, entry.FullNormalName()));

            return File.Exists(Path.Combine(path, entry.FullNormalName()));
        }

        public static bool AreAnyCheckedEntriesInstalled(this ZipArchiveViewModel vm, string path)
        {
            if (vm.SelectedDirectory is ZipArchiveEntryViewModel)
                return (vm.SelectedDirectory as ZipArchiveEntryViewModel).AreAnyCheckedEntriesInstalled(path);

            return vm.SortedEntries.Values.Where(e => e.WillInstall).Any(e => e.IsInstalled(path));
        }

        public static bool AreAnyCheckedEntriesInstalled(this ZipArchiveEntryViewModel vm, string path)
        {
            return vm.SortedEntries.Values.Where(e => e.WillInstall).Any(e => e.IsInstalled(path));
        }

        public static void InstallCheckedEntries(this ZipArchiveViewModel vm, string path, bool overwrite = false)
        {
            if (vm.SelectedDirectory is ZipArchiveEntryViewModel)
            {
                (vm.SelectedDirectory as ZipArchiveEntryViewModel).InstallCheckedEntries(path, overwrite);
                return;
            }

            if (overwrite == false)
                Debug.Assert(vm.AreAnyCheckedEntriesInstalled(path) == false);

            foreach (var entry in vm.SortedEntries.Values.Where(e => e.WillInstall))
            {
                entry.InstallEntry(path, overwrite);
                entry.InstallEntries(path, overwrite);
            }
        }

        public static void InstallCheckedEntries(this ZipArchiveEntryViewModel vm, string path, bool overwrite = false)
        {
            if (overwrite == false)
                Debug.Assert(vm.AreAnyCheckedEntriesInstalled(path) == false);

            foreach (var entry in vm.SortedEntries.Values.Where(e => e.WillInstall))
            {
                entry.InstallEntry(path, overwrite);
                entry.InstallEntries(path, overwrite);
            }
        }

        public static void InstallEntry(this ZipArchiveEntryViewModel vm, string path, bool overwrite = false)
        {
            string fullName = Path.Combine(path, vm.FullName);
            if (vm.IsDirectory)
            {
                if (overwrite && Directory.Exists(fullName))
                    return;

                Directory.CreateDirectory(fullName);
                return;
            }

            vm.Entry.ExtractToFile(fullName, overwrite);
        }
        public static void InstallEntries(this ZipArchiveEntryViewModel vm, string path, bool overwrite = false)
        {
            if (overwrite == false)
                Debug.Assert(vm.AreAnyCheckedEntriesInstalled(path) == false);

            foreach (var entry in vm.SortedEntries.Values)
            {
                entry.InstallEntry(path, overwrite);
                entry.InstallEntries(path, overwrite);
            }
        }


    }
}
