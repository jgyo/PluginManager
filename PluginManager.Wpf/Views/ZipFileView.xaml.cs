namespace PluginManager.Wpf.Views
{
    using Ookii.Dialogs.Wpf;
    using PluginManager.Core;
    using PluginManager.Core.ViewModels;
    using System;
    using System.Drawing;
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
        /// The Vm_BrowseZipFileRequested.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void Vm_BrowseZipFileRequested(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The Vm_DeleteZipFileRequested.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void Vm_DeleteZipFileRequested(object sender, EventArgs e)
        {

            var win = new TaskDialog()
            {
                WindowTitle = "Delete Requested",
                MainIcon = TaskDialogIcon.Custom,
                CustomMainIcon = Icon.ExtractAssociatedIcon("./Resources/question_mark_256.ico")
            };
            win.RadioButtons.Add(new TaskDialogRadioButton() { Text = "Only delete zip file." });
            win.RadioButtons.Add(new TaskDialogRadioButton() { Text = "Only delete the database record." });
            win.RadioButtons.Add(new TaskDialogRadioButton() { Text = "Delete both record and file.", Checked = true });
            win.Buttons.Add(new TaskDialogButton(ButtonType.Cancel));
            win.Buttons.Add(new TaskDialogButton(ButtonType.Ok));
            win.Content = "What do you want to delete?";

            win.ShowDialog();
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
            }

            vm = e.NewValue as ZipFileViewModel;

            if (vm != null)
            {
                vm.BrowseZipFileRequested += Vm_BrowseZipFileRequested;
                vm.DeleteZipFileRequested += Vm_DeleteZipFileRequested;
                vm.DoneEditingRequested += Vm_DoneEditingRequested;
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
            win.Closing += Win_Closing;
        }
    }
}
