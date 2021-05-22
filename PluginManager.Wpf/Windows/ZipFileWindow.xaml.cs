using PluginManager.Core;
using PluginManager.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace PluginManager.Wpf.Windows
{
    /// <summary>
    /// Interaction logic for ZipFileWindow.xaml
    /// </summary>
    public partial class ZipFileWindow : Window
    {
        public ZipFileWindow(ZipFileViewModel vm)
        {
            InitializeComponent();
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
