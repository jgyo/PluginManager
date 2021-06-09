namespace PluginManager.Wpf.Views
{
    using Microsoft.Win32;
    using Ookii.Dialogs.Wpf;
    using PluginManager.Core;
    using PluginManager.Core.Logging;
    using PluginManager.Core.ViewModels;
    using PluginManager.Wpf.Utilities;
    using PluginManager.Wpf.Windows;
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for ZipFileView.xaml.
    /// </summary>
    public partial class ZipFileView : UserControl
    {
        /// <summary>
        /// Defines the viewModel.
        /// </summary>
        private ZipFileViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZipFileView"/> class.
        /// </summary>
        public ZipFileView()
        {
            InitializeComponent();

            DataContextChanged += ZipFileView_DataContextChanged;
            this.Loaded += ZipFileView_Loaded;
        }

        /// <summary>
        /// The DeleteRecord.
        /// </summary>
        /// <param name="item">The item<see cref="ZipFileViewModel"/>.</param>
        private void DeleteRecord(ZipFileViewModel item)
        {
            // DbCore.Delete(item);
            Locator.MainViewModel.ZipFileFolderCollection.Remove(item);
            Window.GetWindow(this).Close();
        }

        /// <summary>
        /// The DeleteZipFile.
        /// </summary>
        /// <param name="item">The item<see cref="ZipFileViewModel"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "No benefit.")]
        private bool DeleteZipFile(ZipFileViewModel item)
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
        /// The Vm_BrowseZipFileRequested.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void Vm_BrowseZipFileRequested(object sender, EventArgs e)
        {
            var vm = sender as ZipFileViewModel;
            Debug.Assert(vm != null);

            var dia = new OpenFileDialog
            {
                AddExtension = false,
                CheckFileExists = true,
                Multiselect = false,
                Title = "Find a Plugin Package to Add",
                DereferenceLinks = true,
                Filter = "Zip files (*.zip, *.7z)|*.zip;*.7z|Exe files (*.exe)|*.exe|All files (*.*)|*.*",
                FilterIndex = 1,
                InitialDirectory = Locator.SetupViewModel.ZipFilesFolder
            };

            var result = dia.ShowDialog();
            if (result == null || result == false)
                return;

            var fullpath = dia.FileName;
            var filename = Path.GetFileName(fullpath);
            var path = Path.GetDirectoryName(fullpath);
            var fi = new FileInfo(fullpath);

            if (vm.Filename != filename || vm.FileDate != fi.CreationTime)
            {
                vm.FileDate = fi.CreationTime;
                vm.AddedDate = DateTime.Now;
                vm.Filename = filename;
                vm.FileSize = fi.Length;
            }

            vm.FilePath = path;

            DbCore.Update(vm);
        }

        /// <summary>
        /// The Vm_DeleteZipFileRequested.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void Vm_DeleteZipFileRequested(object sender, EventArgs e)
        {
            var a = Assembly.GetExecutingAssembly();
            var st = a.GetManifestResourceStream("PluginManager.Wpf.Resources.qmark.ico");
            var qmark = new Icon(st);

            var win = new TaskDialog()
            {
                WindowTitle = "Delete Requested",
                MainIcon = TaskDialogIcon.Custom,
                CustomMainIcon = qmark
            };

            win.RadioButtons.Add(new TaskDialogRadioButton() { Text = "Only delete the zip file." });
            win.RadioButtons.Add(new TaskDialogRadioButton() { Text = "Only delete the database record." });
            win.RadioButtons.Add(new TaskDialogRadioButton() { Text = "Delete both record and file.", Checked = true });
            win.Buttons.Add(new TaskDialogButton(ButtonType.Cancel));
            win.Buttons.Add(new TaskDialogButton(ButtonType.Ok));
            win.Content = "What do you want to delete?";

            var result = win.ShowDialog();

            if (result == null || result.ButtonType == ButtonType.Cancel)
                return;

            var vm = sender as ZipFileViewModel;
            Debug.Assert(vm != null);

            var option = win.RadioButtons[0].Checked ? 1 : win.RadioButtons[1].Checked ? 2 : 3;
            switch (option)
            {
                case 1:
                    if (!DeleteZipFile(vm))
                        MessageBox.Show($"Unable to delete {vm.Filename}.", "Exception");
                    break;
                case 2:
                    DeleteRecord(vm);
                    break;
                case 3:
                    if (!DeleteZipFile(vm))
                    {
                        var dr = MessageBox.Show($"Unable to delete {vm.Filename}. Do you want to delete the record anyway?", "Exception", MessageBoxButton.YesNo);
                        if (dr != MessageBoxResult.Yes)
                            return;
                    }
                    DeleteRecord(vm);
                    break;
                default:
                    throw new ArgumentException("Invalid radio button.");
            }
        }

        /// <summary>
        /// The Vm_DoneEditingRequested.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void Vm_DoneEditingRequested(object sender, EventArgs e)
        {
            DbCore.Update(viewModel);
            var win = Window.GetWindow(this);
            win.Close();
        }

        /// <summary>
        /// The Vm_OpenZipArchiveRequested.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void Vm_OpenZipArchiveRequested(object sender, EventArgs e)
        {
            var zfr = sender as ZipFileViewModel;
            Debug.Assert(zfr != null);

            try
            {
                WpfHelper.SetWindowSettings(Window.GetWindow(this));
                var win = new ZipArchiveWindow(zfr);
                win.ShowDialog();
            }
            catch (Exception ex)
            {
                App.Inform("Exception Encountered", "An exception occurred while trying to open the file.");
                var log = FileLogProvider.Instance.GetLogFor<ZipFileView>();
                log.DebugException($"Unable to open {zfr.Filename}", ex);
            }
        }

        /// <summary>
        /// Event handler cleanup.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="System.ComponentModel.CancelEventArgs"/>.</param>
        private void Win_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var vm = DataContext as ZipFileViewModel;
            vm.BrowseZipFileRequested -= Vm_BrowseZipFileRequested;
            vm.DeleteZipFileRequested -= Vm_DeleteZipFileRequested;
            vm.DoneEditingRequested -= Vm_DoneEditingRequested;
            vm.OpenZipArchiveRequested -= Vm_OpenZipArchiveRequested;
        }

        /// <summary>
        /// The ZipFileView_DataContextChanged.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="DependencyPropertyChangedEventArgs"/>.</param>
        private void ZipFileView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is ZipFileViewModel vm)
            {
                vm.BrowseZipFileRequested -= Vm_BrowseZipFileRequested;
                vm.DeleteZipFileRequested -= Vm_DeleteZipFileRequested;
                vm.DoneEditingRequested -= Vm_DoneEditingRequested;
                vm.OpenZipArchiveRequested -= Vm_OpenZipArchiveRequested;
            }

            vm = e.NewValue as ZipFileViewModel;

            if (vm != null)
            {
                vm.BrowseZipFileRequested += Vm_BrowseZipFileRequested;
                vm.DeleteZipFileRequested += Vm_DeleteZipFileRequested;
                vm.DoneEditingRequested += Vm_DoneEditingRequested;
                vm.OpenZipArchiveRequested += Vm_OpenZipArchiveRequested;
            }

            viewModel = DataContext as ZipFileViewModel;
        }

        /// <summary>
        /// Called with the control is loaded into its window.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/>.</param>
        private void ZipFileView_Loaded(object sender, RoutedEventArgs e)
        {
            var win = Window.GetWindow(this);
            if (win == null)
                return;
            win.Closing += Win_Closing;
        }
    }
}
