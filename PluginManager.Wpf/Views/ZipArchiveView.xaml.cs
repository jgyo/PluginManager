namespace PluginManager.Wpf.Views
{
    using PluginManager.Core;
    using PluginManager.Core.Utilities;
    using PluginManager.Core.ViewModels;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for ZipArchiveView.xaml.
    /// </summary>
    public partial class ZipArchiveView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZipArchiveView"/> class.
        /// </summary>
        public ZipArchiveView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The DataGrid_MouseDoubleClick.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="MouseButtonEventArgs"/>.</param>
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
                return;

            var grid = sender as DataGrid;
            Debug.Assert(grid != null);

            if (grid.SelectedItem == null)
                return;

            var temp = grid.SelectedItem as ZipArchiveEntryViewModel;
            if (temp.SortedEntries?.Count == 0)
                return;

            var vm = DataContext as ZipArchiveViewModel;
            vm.SelectedDirectory = temp;
        }

        /// <summary>
        /// The DataGrid_SelectionChanged.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="SelectionChangedEventArgs"/>.</param>
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
            {
                (item as ZipArchiveEntryViewModel).WillInstall = true;
            }

            foreach (var item in e.RemovedItems)
            {
                (item as ZipArchiveEntryViewModel).WillInstall = false;
            }
        }

        /// <summary>
        /// The DoneButtonClick.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/>.</param>
        private void DoneButtonClick(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        /// <summary>
        /// The InstallButtonClick.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/>.</param>
        private void InstallButtonClick(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ZipArchiveViewModel;
            var directory = vm.SelectedDirectory;

            if (directory.Entries.Any(m => m.WillInstall) == false)
                return;

            var installFolder = Locator.SetupViewModel.CommunityFolder;

            // var wd = App.WaitDialog("Your selected contents are being installed.");
            IEnumerable<string> installedFolders;

            if (vm.AreAnyCheckedEntriesInstalled(installFolder))
            {
                if (App.LastChance("Overwrite Files?", "This operation will require some files and folders to be overwritten.", "Do you want to do this?", true))
                {
                    // #Note This should on a background thread
                    // wd.Show();
                    installedFolders = vm.InstallCheckedEntries(installFolder, true);
                }
                else
                {
                    return;
                }
            }
            else
            {
                // #Note See note above.
                // wd.Show();
                installedFolders = vm.InstallCheckedEntries(installFolder);
            }

            var mainVm = Locator.MainViewModel;
            var packageId = vm.PackageId;

            // This will update the DB for the new folders
            foreach (var folderName in installedFolders)
            {
                var fullName = Path.Combine(installFolder, folderName);
                var di = new DirectoryInfo(fullName);
                var installDate = di.CreationTime;

                if (mainVm.FolderCollection.Any(e => e.FolderName.ToLower() == folderName.ToLower()))
                {
                    var oldFe = mainVm.FolderCollection.First(e => e.FolderName.ToLower() == folderName.ToLower());
                    oldFe.PackageId = packageId;
                    DbCore.Update(oldFe);
                    continue;
                }

                var newFe = new FolderViewModel()
                {
                    FolderName = folderName,
                    PackageId = packageId,
                    InstallDate = installDate
                };

                DbCore.Add(newFe);
                mainVm.FolderCollection.Add(newFe);
            }

            // wd.Close();
            App.Inform("Zip Contents Installed", "The selected contents have been installed in your community folder.");
        }

        /// <summary>
        /// The UpButtonClick.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/>.</param>
        private void UpButtonClick(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ZipArchiveViewModel;
            if (vm.Equals(vm.SelectedDirectory))
                return;

            var entry = vm.SelectedDirectory as ZipArchiveEntryViewModel;
            if (entry.Parent == null)
            {
                vm.SelectedDirectory = vm;
                return;
            }

            vm.SelectedDirectory = entry.Parent;
        }
    }
}
