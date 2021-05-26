using PluginManager.Core.ViewModels;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for ZipArchiveWindow.xaml
    /// </summary>
    public partial class ZipArchiveWindow : Window
    {
        public ZipArchiveWindow(Core.ViewModels.ZipFileViewModel zfr)
        {
            InitializeComponent();

            var archive = new ZipArchiveViewModel(zfr.Filename, zfr.FilePath);
            view.DataContext = archive;
        }
    }
}
