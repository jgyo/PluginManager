namespace PluginManager.Wpf.Views
{
    using Microsoft.Win32;
    using Ookii.Dialogs.Wpf;
    using PluginManager.Core;
    using PluginManager.Core.EventHandlers;
    using PluginManager.Core.Logging;
    using PluginManager.Core.System;
    using PluginManager.Core.ViewModels;
    using PluginManager.Core.ViewModels.DesignTime;
    using PluginManager.Wpf.Windows;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for MainView.xaml.
    /// </summary>
    public partial class MainView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainView"/> class.
        /// </summary>
        public MainView()
        {
            InitializeComponent();

            folderControl.SelectionChanged += FolderControl_SelectionChanged;
            zipfilesControl.SelectionChanged += ZipfilesControl_SelectionChanged;

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                this.DataContext = new MainViewModelDesignTime();
            }
            else
            {
                var vm = Locator.MainViewModel;
                MainViewSetup(vm);

                this.DataContext = vm;
            }
        }

        /// <summary>
        /// The DeleteRecord.
        /// </summary>
        /// <param name="item">The item<see cref="ZipFileViewModel"/>.</param>
        private static void DeleteRecord(ZipFileViewModel item)
        {
            DbCore.Delete(item);
            Locator.MainViewModel.ZipFileFolderCollection.Remove(item);
        }

        /// <summary>
        /// The DeleteZipFile.
        /// </summary>
        /// <param name="item">The item<see cref="ZipFileViewModel"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private static bool DeleteZipFile(ZipFileViewModel item)
        {
            try
            {
                File.Delete(Path.Combine(item.FilePath, item.Filename));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Converts a long path name into a simple folder name.
        /// </summary>
        /// <param name="folder">The v<see cref="string"/>.</param>
        private static void ShortenName(ref string folder)
        {
            folder = Path.GetFileName(folder);
        }

        /// <summary>
        /// Handles the SelectionChanged event for folders.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="SelectionChangedEventArgs"/>.</param>
        private void FolderControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext as MainViewModel == null)
                return;

            var removed = e.RemovedItems;
            var added = e.AddedItems;
            var vm = DataContext as MainViewModel;

            foreach (var item in added)
            {
                vm.SelectedFoldersCollection.Add(item as FolderViewModel);
            }

            foreach (var item in removed)
            {
                vm.SelectedFoldersCollection.Remove(item as FolderViewModel);
            }
        }

        /// <summary>
        /// The MainViewSetup.
        /// </summary>
        /// <param name="vm">The vm<see cref="MainViewModel"/>.</param>
        private void MainViewSetup(MainViewModel vm)
        {
            if (vm != null)
            {
                vm.DeleteSelectedFoldersRequested += Vm_DeleteSelectedFoldersRequested;
                vm.HideSelectedFoldersRequested += Vm_HideSelectedFoldersRequested;
                vm.RestoreSelectedItemsRequested += Vm_RestoreSelectedFoldersRequested;
                vm.SynchronizeDataBaseRequested += Vm_SynchronizeDataBaseRequested;
                vm.OpenSetupRequested += Vm_OpenSetupRequested;
                vm.EditSelectedFolderRequested += Vm_EditSelectedFolderRequested;

                vm.EditSelectedZipFileRequested += Vm_EditZipFileRequested;
                vm.AddNewZipFileRequested += Vm_AddNewZipFileRequested;
                vm.DeleteSelectedZipFilesRequested += Vm_DeleteSelectedZipFilesRequested;
                vm.OpenZipArchiveRequested += Vm_OpenZipArchiveRequested;
            }
        }

        /// <summary>
        /// The Vm_AddNewZipFileRequested.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void Vm_AddNewZipFileRequested(object sender, EventArgs e)
        {
            var dia = new OpenFileDialog
            {
                AddExtension = false,
                CheckFileExists = true,
                Multiselect = true,
                Title = "Find Zip Files to Add",
                DereferenceLinks = true,
                Filter = "Zip files (*.zip, *.7z)|*.zip;*.7z|Exe files (*.exe)|*.exe|All files (*.*)|*.*",
                FilterIndex = 1,
                InitialDirectory = Locator.SetupViewModel.ZipFilesFolder
            };
            var result = dia.ShowDialog();

            if (result == true)
            {
                var zipList = new List<ZipFileViewModel>();

                foreach (var fullpath in dia.FileNames)
                {
                    var filename = Path.GetFileName(fullpath);
                    var path = Path.GetDirectoryName(fullpath);
                    var di = new FileInfo(fullpath);

                    var zipFile = new ZipFileViewModel()
                    {
                        FileDate = di.CreationTime,
                        Filename = filename,
                        FilePath = path,
                        AddedDate = DateTime.Now,
                        FileSize = di.Length
                    };

                    zipList.Add(zipFile);
                }

                DbCore.Add(zipList);
                string fileItems = "";
                foreach (var item in zipList)
                {
                    Locator.MainViewModel.ZipFileFolderCollection.Add(item);
                    fileItems += Environment.NewLine + item.Filename;
                }

                string message;
                string title;

                var isSingle = zipList.Count == 1;

                if (isSingle)
                {
                    message = $"One zip file was added:\n{fileItems}";
                    title = "Zip File Added";
                }
                else
                {
                    message = $"These {zipList.Count} zip files were added:\n{fileItems}";
                    title = "Zip Files Added";
                }

                App.Inform(title, message);
            }
        }

        /// <summary>
        /// Handles the DeleteSelectedItemsRequested event.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void Vm_DeleteSelectedFoldersRequested(object sender, EventArgs e)
        {
            Debug.Assert(sender is MainViewModel);
            var vm = sender as MainViewModel;

            var selectedToDelete = new List<FolderViewModel>();
            foreach (var item in vm.SelectedFoldersCollection)
            {
                selectedToDelete.Add(item);
            }

            var folderNames = Environment.NewLine +
                Environment.NewLine;

            foreach (var item in selectedToDelete)
            {
                folderNames += item.FolderName + Environment.NewLine;
            }

            var proceed = App.LastChance("Delete Folders Request", $"Deleting Selected Folders", $"This operation will delete these (this) {selectedToDelete.Count} folder(s) from the community or hidden directories:" +
                folderNames +
                Environment.NewLine +
                "Is this what you want to do?");

            if (proceed)
            {
                var hiddenFolder = AppSettings.Default.HiddenFilesFolder;
                var communityFolder = AppSettings.Default.CommunityFolder;

                foreach (var item in selectedToDelete)
                {
                    FileOps.Delete(item.FolderName, item.IsHidden ? hiddenFolder : communityFolder);
                    vm.FolderCollection.Remove(item);
                }

                DbCore.Delete(selectedToDelete);

            }
        }

        /// <summary>
        /// The Vm_DeleteSelectedZipFilesRequested.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void Vm_DeleteSelectedZipFilesRequested(object sender, EventArgs e)
        {
            var vm = DataContext as MainViewModel;
            var count = vm.SelectedZipFilesCollection.Count;

            var message = $"You have {count} zip file(s) selected and have initiated the delete function. What do you want to do?";
            var a = Assembly.GetExecutingAssembly();
            var st = a.GetManifestResourceStream("PluginManager.Wpf.Resources.qmark.ico");
            var qmark = new Icon(st);

            var win = new TaskDialog()
            {
                WindowTitle = "Delete Requested",
                MainIcon = TaskDialogIcon.Custom,
                Content = message,
                CustomMainIcon = qmark
            };

            win.RadioButtons.Add(new TaskDialogRadioButton() { Text = "Only delete zip files." });
            win.RadioButtons.Add(new TaskDialogRadioButton() { Text = "Only delete database records." });
            win.RadioButtons.Add(new TaskDialogRadioButton() { Text = "Delete both records and files.", Checked = true });
            win.Buttons.Add(new TaskDialogButton(ButtonType.Cancel));
            win.Buttons.Add(new TaskDialogButton(ButtonType.Ok));

            var result = win.ShowDialog();

            if (result == null || result.ButtonType == ButtonType.Cancel)
                return;

            // #Switch
            var option = win.RadioButtons[0].Checked ? 1 : win.RadioButtons[1].Checked ? 2 : 3;
            foreach (var item in vm.SelectedZipFilesCollection)
            {
                switch (option)
                {
                    case 1:
                        if (!DeleteZipFile(item))
                            MessageBox.Show($"Unable to delete {item.Filename}.", "Exception");
                        break;
                    case 2:
                        DeleteRecord(item);
                        break;
                    case 3:
                        if (!DeleteZipFile(item))
                        {
                            var dr = MessageBox.Show($"Unable to delete {item.Filename}. Do you want to delete the record anyway?", "Exception", MessageBoxButton.YesNo);
                            if (dr != MessageBoxResult.Yes)
                                continue;
                        }
                        DeleteRecord(item);
                        break;
                    default:
                        throw new ArgumentException("Invalid radio button.");
                }
            }
        }

        /// <summary>
        /// Handles the EditSelectedFolderRequested event.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="ViewModelEventArgs"/>.</param>
        private void Vm_EditSelectedFolderRequested(object sender, ViewModelEventArgs e)
        {
            var vm = e.ViewModel as FolderViewModel;
            var win = new FolderWindow(vm);
            win.ShowDialog();
            var dc = Locator.MainViewModel;
            dc.FolderViewModelClosed(vm);
        }

        /// <summary>
        /// Handles the EditZipFileRequested event.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="ViewModelEventArgs"/>.</param>
        private void Vm_EditZipFileRequested(object sender, ViewModelEventArgs e)
        {
            var vm = e.ViewModel as ZipFileViewModel;
            var win = new ZipFileWindow(vm);
            win.ShowDialog();
            var dc = Locator.MainViewModel;
            dc.ZipFileViewModelClosed(vm);
        }

        /// <summary>
        /// Handles the HideSelectedItemsRequested event.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void Vm_HideSelectedFoldersRequested(object sender, EventArgs e)
        {
            Debug.Assert(sender is MainViewModel);
            var vm = sender as MainViewModel;

            var selectedToHide = new List<FolderViewModel>();
            foreach (var item in vm.SelectedFoldersCollection)
            {
                if (item.IsHidden == false)
                    selectedToHide.Add(item);
            }

            if (selectedToHide.Count == 0)
            {
                App.Inform("No Folders to Hide", "There are no unhidden folders to hide in the current selection.");
                return;
            }

            var folderNames = Environment.NewLine +
                Environment.NewLine;

            foreach (var item in selectedToHide)
            {
                folderNames += item.FolderName + Environment.NewLine;
            }

            var proceed = App.LastChance("Hide Folders Request", $"Hiding Selected Folders", $"This operation will move these (this) {selectedToHide.Count} folder(s) from the community directory to the hidden directory:" +
                folderNames +
                Environment.NewLine +
                "Is this what you want to do?");

            int count = 0;
            if (proceed)
            {
                var su = Locator.SetupViewModel;
                foreach (var item in selectedToHide)
                {
                    if (FileOps.Hide(item.FolderName, su.CommunityFolder, su.HiddenFilesFolder))
                    {
                        item.IsHidden = true;
                        count++;
                    }
                }

                DbCore.Update(selectedToHide);

                if (count == selectedToHide.Count)
                    App.Inform("Hide Folders Request Results", "All unhidden folders were successfully hidden.");
                else
                    App.Inform("Hide Folders Request Results", $"{selectedToHide.Count - count} folder(s) could not be hidden. Synchronization may be necessary.");
            }
        }

        /// <summary>
        /// Handles the OpenSetupRequested event.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="ViewModelEventArgs"/>.</param>
        private void Vm_OpenSetupRequested(object sender, ViewModelEventArgs e)
        {
            var win = new SetupWindow();
            win.ShowDialog();
        }

        /// <summary>
        /// The Vm_OpenZipArchiveRequested.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void Vm_OpenZipArchiveRequested(object sender, EventArgs e)
        {
            var vm = DataContext as MainViewModel;
            Debug.Assert(vm != null);
            Debug.Assert(vm.SelectedZipFilesCollection.Count == 1);
            var zfr = vm.SelectedZipFilesCollection[0];
            Debug.Assert(zfr != null);

            try
            {
                var win = new ZipArchiveWindow(zfr);
                win.ShowDialog();
            }
            catch (Exception ex)
            {
                App.Inform("Exception Encountered", "An exception occured while trying to open the file.");
                var log = LogProvider.Instance.GetLogFor<MainView>();
                log.DebugException($"Unable to open {zfr.Filename}", ex);
            }
        }

        /// <summary>
        /// Handles the RestoreSelectedItemsRequested event.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void Vm_RestoreSelectedFoldersRequested(object sender, EventArgs e)
        {
            Debug.Assert(sender is MainViewModel);
            var vm = sender as MainViewModel;

            var selectedToRestore = new List<FolderViewModel>();
            foreach (var item in vm.SelectedFoldersCollection)
            {
                if (item.IsHidden == true)
                    selectedToRestore.Add(item);
            }

            if (selectedToRestore.Count == 0)
            {
                App.Inform("No Folders to Restore", "There are no hidden folders to restore in the current selection.");
                return;
            }

            var folderNames = Environment.NewLine +
                Environment.NewLine;

            foreach (var item in selectedToRestore)
            {
                folderNames += item.FolderName + Environment.NewLine;
            }

            var proceed = App.LastChance("Restore Folders Request", $"Restoring Selected Folders", $"This operation will move these (this) {selectedToRestore.Count} folder(s) from the hiddent directory to the community directory:" +
                folderNames +
                Environment.NewLine +
                "Is this what you want to do?");

            int count = 0;
            if (proceed)
            {
                var su = Locator.SetupViewModel;
                foreach (var item in selectedToRestore)
                {
                    if (FileOps.Restore(item.FolderName, su.CommunityFolder, su.HiddenFilesFolder))
                    {
                        item.IsHidden = false;
                        count++;
                    }
                }
                DbCore.Update(selectedToRestore);

                if (count == selectedToRestore.Count)
                    App.Inform("Hide Folders Request Results", "All unhidden folders were successfully hidden.");
                else
                    App.Inform("Hide Folders Request Results", $"{selectedToRestore.Count - count} folder(s) could not be hidden. Synchronization may be necessary.");
            }
        }

        /// <summary>
        /// Handles the SynchronizeDataBaseRequested event.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void Vm_SynchronizeDataBaseRequested(object sender, EventArgs e)
        {
            var su = Locator.SetupViewModel;
            var cf = su.CommunityFolder;
            var hf = su.HiddenFilesFolder;

            var communityFolderNames = Directory.GetDirectories(cf);
            var hiddenFolderNames = Directory.GetDirectories(hf);

            for (int i = 0; i < communityFolderNames.Length; i++)
            {
                ShortenName(ref communityFolderNames[i]);
            }

            for (int i = 0; i < hiddenFolderNames.Length; i++)
            {
                ShortenName(ref hiddenFolderNames[i]);
            }

            var allNames = new List<string>();
            allNames.AddRange(communityFolderNames);
            allNames.AddRange(hiddenFolderNames);

            var temp = new List<FolderViewModel>();

            foreach (var folder in Locator.MainViewModel.FolderCollection)
            {
                if (allNames.Contains(folder.FolderName))
                    continue;

                temp.Add(folder);
            }

            foreach (var folder in temp)
            {
                Locator.MainViewModel.FolderCollection.Remove(folder);
            }

            DbCore.Delete(temp);
            temp.Clear();

            foreach (var item in hiddenFolderNames)
            {
                var folder = Locator.MainViewModel.FolderCollection.Where(m => m.FolderName == item).FirstOrDefault();
                if (folder == null || folder.IsHidden)
                    continue;

                folder.IsHidden = true;
                temp.Add(folder);
            }

            foreach (var item in communityFolderNames)
            {
                var folder = Locator.MainViewModel.FolderCollection.Where(m => m.FolderName == item).FirstOrDefault();
                if (folder == null || !folder.IsHidden)
                    continue;

                folder.IsHidden = false;
                temp.Add(folder);
            }

            DbCore.Update(temp);
            temp.Clear();

            foreach (var item in communityFolderNames.Where(m => !Locator.MainViewModel.FolderCollection.Any(v => v.FolderName == m)))
            {
                var di = new DirectoryInfo(Path.Combine(Locator.SetupViewModel.HiddenFilesFolder, item));
                var vm = new FolderViewModel()
                {
                    FolderName = item,
                    IsHidden = false,
                    InstallDate = di.CreationTime
                };

                temp.Add(vm);
            }

            foreach (var item in hiddenFolderNames.Where(m => !Locator.MainViewModel.FolderCollection.Any(v => v.FolderName == m)))
            {
                var di = new DirectoryInfo(Path.Combine(Locator.SetupViewModel.HiddenFilesFolder, item));
                var vm = new FolderViewModel()
                {
                    FolderName = item,
                    IsHidden = true,
                    InstallDate = di.CreationTime
                };

                temp.Add(vm);
            }

            DbCore.Add(temp);
            foreach (var item in temp)
            {
                Locator.MainViewModel.FolderCollection.Add(item);
            }
        }

        /// <summary>
        /// Handles the SelectionChanged for Zip files.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="SelectionChangedEventArgs"/>.</param>
        private void ZipfilesControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext as MainViewModel == null)
                return;

            var removed = e.RemovedItems;
            var added = e.AddedItems;
            var vm = DataContext as MainViewModel;

            foreach (var item in added)
            {
                vm.SelectedZipFilesCollection.Add(item as ZipFileViewModel);
            }

            foreach (var item in removed)
            {
                vm.SelectedZipFilesCollection.Remove(item as ZipFileViewModel);
            }
        }
    }
}
