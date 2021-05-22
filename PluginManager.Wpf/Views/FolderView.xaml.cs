namespace PluginManager.Wpf.Views
{
    using PluginManager.Core;
    using PluginManager.Core.System;
    using PluginManager.Core.ViewModels;
    using PluginManager.Wpf.Utilities;
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for FolderView.xaml.
    /// </summary>
    public partial class FolderView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FolderView"/> class.
        /// </summary>
        public FolderView()
        {
            InitializeComponent();

            DataContextChanged += FolderView_DataContextChanged;
            this.Loaded += FolderView_Loaded;
        }

        /// <summary>
        /// Handles the DataContextChanged event.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="DependencyPropertyChangedEventArgs"/>.</param>
        private void FolderView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is FolderViewModel vm)
            {
                vm.DeleteFolderRequested -= Vm_DeleteFolderRequested;
                vm.HideFolderRequested -= Vm_HideFolderRequested;
                vm.RestoreFolderRequested -= Vm_RestoreFolderRequested;
                vm.DoneEditingRequested -= Vm_DoneEditingRequested;
            }

            vm = e.NewValue as FolderViewModel;

            if (vm != null)
            {
                vm.DeleteFolderRequested += Vm_DeleteFolderRequested;
                vm.HideFolderRequested += Vm_HideFolderRequested;
                vm.RestoreFolderRequested += Vm_RestoreFolderRequested;
                vm.DoneEditingRequested += Vm_DoneEditingRequested;
            }
        }

        private void Vm_DoneEditingRequested(object sender, EventArgs e)
        {
            var win = Window.GetWindow(this);
            win.Close();
        }

        /// <summary>
        /// Called once the control has been loaded into the window.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/>.</param>
        private void FolderView_Loaded(object sender, RoutedEventArgs e)
        {
            var win = Window.GetWindow(this);
            if (win == null)
                return;
            win.Closing += Win_Closing;
        }

        /// <summary>
        /// Handles the DeleteFolderRequested event.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void Vm_DeleteFolderRequested(object sender, EventArgs e)
        {
            Debug.Assert(sender is FolderViewModel);
            var vm = sender as FolderViewModel;

            var proceed = App.LastChance("Delete Folder Request", $"Deleting {vm.FolderName}!", $"This operation will delete the {vm.FolderName} folder and remove this entry from the database The operation is not reversable! Do you really want to do this?", true);
            var su = Locator.SetupViewModel;

            if (proceed)
            {
                vm.DeleteScheduled = true;
                FileOps.Delete(vm.FolderName, vm.IsHidden ? su.HiddenFilesFolder : su.CommunityFolder);
                DbCore.Delete(vm);

                var win = Window.GetWindow(this);
                Debug.Assert(win != null);
                win.Close();
            }
        }

        /// <summary>
        /// Handles the HideFolderRequested event.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void Vm_HideFolderRequested(object sender, EventArgs e)
        {
            Debug.Assert(sender is FolderViewModel);
            var vm = sender as FolderViewModel;

            var proceed = App.LastChance("Hide Folder Request", $"Hiding {vm.FolderName}", $"This operation will move {vm.FolderName} from the community directory to the hidden directory. Is this what you want to do?.");
            var su = Locator.SetupViewModel;

            if (proceed)
            {
                if (FileOps.Hide(vm.FolderName, su.CommunityFolder, su.HiddenFilesFolder))
                {
                    vm.IsHidden = true;
                    DbCore.Update(vm);
                    return;
                }

                App.Inform("Hide Folder Request Error", $"{vm.FolderName} could not be hidden. Synchronization may be necessary.");
            }
        }

        /// <summary>
        /// Handles the RestoreFolderRequested event.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void Vm_RestoreFolderRequested(object sender, EventArgs e)
        {
            Debug.Assert(sender is FolderViewModel);
            var vm = sender as FolderViewModel;

            var proceed = App.LastChance("Restore Folder Request", $"Restoring {vm.FolderName}", $"This operation will move {vm.FolderName} from the hidden directory to the community directory. Is this what you want to do?.");
            var su = Locator.SetupViewModel;

            if (proceed)
            {
                vm.IsHidden = false;
                FileOps.Restore(vm.FolderName, su.CommunityFolder, su.HiddenFilesFolder);
                DbCore.Update(vm);
            }
        }

        /// <summary>
        /// Cleans up event handlers when the window closes.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="System.ComponentModel.CancelEventArgs"/>.</param>
        private void Win_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var vm = DataContext as FolderViewModel;

            vm.DeleteFolderRequested -= Vm_DeleteFolderRequested;
            vm.HideFolderRequested -= Vm_HideFolderRequested;
            vm.RestoreFolderRequested -= Vm_RestoreFolderRequested;
            vm.DoneEditingRequested -= Vm_DoneEditingRequested;
        }
    }
}
