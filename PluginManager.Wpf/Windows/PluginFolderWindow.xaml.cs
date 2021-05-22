namespace PluginManager.Wpf.Windows
{
    using PluginManager.Core;
    using PluginManager.Core.ViewModels;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;

    /// <summary>
    /// Interaction logic for FolderWindow.xaml.
    /// </summary>
    public partial class FolderWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FolderWindow"/> class.
        /// </summary>
        /// <param name="vm">The vm<see cref="FolderViewModel"/>.</param>
        public FolderWindow(FolderViewModel vm = null)
        {
            InitializeComponent();

            if (vm == null)
            {
                vm = Locator.GetFolderViewModel();
                vm.FolderId = 1001;
                vm.FolderName = "Arphaxhad";
                vm.InstallDate = DateTime.Now;
            }
            this.DataContext = vm;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            var vm = this.DataContext as FolderViewModel;
            Debug.Assert(vm != null);

            if(!vm.DeleteScheduled)
            {
                DbCore.Update(vm);
            }

            base.OnClosing(e);
        }
    }
}