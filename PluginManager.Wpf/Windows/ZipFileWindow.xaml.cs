namespace PluginManager.Wpf.Windows
{
    using PluginManager.Core;
    using PluginManager.Core.ViewModels;
    using PluginManager.Wpf.Utilities;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;

    /// <summary>
    /// Interaction logic for ZipFileWindow.xaml.
    /// </summary>
    public partial class ZipFileWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZipFileWindow"/> class.
        /// </summary>
        /// <param name="vm">The vm<see cref="ZipFileViewModel"/>.</param>
        public ZipFileWindow(ZipFileViewModel vm)
        {
            InitializeComponent();
            WpfHelper.PositionChildWindow(this);
            if (vm == null)
            {
                vm = Locator.GetZipFileViewModel();
                vm.PackageId = 11;
                vm.AddonName = "Arphaxhad";
                vm.AddedDate = DateTime.Now;
                vm.Filename = "Arphaxhad.zip";
                vm.FilePath = "d:\\Downloads\\";
                vm.FileSize = 100000;

            }
            this.DataContext = vm;
        }

        /// <summary>
        /// The OnClosing.
        /// </summary>
        /// <param name="e">The e<see cref="CancelEventArgs"/>.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            var vm = this.DataContext as ZipFileViewModel;
            Debug.Assert(vm != null);

            if (!vm.DeleteScheduled)
            {
                DbCore.Update(vm);
            }

            base.OnClosing(e);
        }
    }
}
