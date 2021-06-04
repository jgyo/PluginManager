using PluginManager.Core.Utilities;
using PluginManager.Core.ViewModels;
using PluginManager.Wpf.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PluginManager.Wpf.Views
{
    /// <summary>
    /// Interaction logic for ZipArchiveView.xaml
    /// </summary>
    public partial class ZipArchiveView : UserControl
    {

        public ZipArchiveView()
        {
            InitializeComponent();
        }

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

        private void UpButtonClick(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ZipArchiveViewModel;
            if (vm.Equals(vm.SelectedDirectory))
                return;

            var entry = vm.SelectedDirectory as ZipArchiveEntryViewModel;
            if(entry.Parent == null)
            {
                vm.SelectedDirectory = vm;
                return;
            }

            vm.SelectedDirectory = entry.Parent;
        }

        private void DoneButtonClick(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void InstallButtonClick(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ZipArchiveViewModel;
            var directory = vm.SelectedDirectory;

            if (directory.Entries.Any(m => m.WillInstall) == false)
                return;

            var installFolder = Locator.SetupViewModel.CommunityFolder;
            
            // var wd = App.WaitDialog("Your selected contents are being installed.");

            if (vm.AreAnyCheckedEntriesInstalled(installFolder))
            {
                if (App.LastChance("Overwrite Files?", "This operation will require some files and folders to be overwritten.", "Do you want to do this?", true))
                {
                    // #Note This should on a background thread
                    // wd.Show();
                    vm.InstallCheckedEntries(installFolder, true);
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
                vm.InstallCheckedEntries(installFolder);
            }

            // wd.Close();
            App.Inform("Zip Contents Installed", "The selected contents have been installed in your community folder.");
        }
    }
}
