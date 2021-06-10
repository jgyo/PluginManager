using PluginManager.Wpf.Utilities;
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
using VersionManagement;

namespace PluginManager.Wpf.Windows
{
    /// <summary>
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        private VersionCheck verCheck;

        public UpdateWindow()
        {
            InitializeComponent();
        }

        public UpdateWindow(VersionCheck verCheck)
        {
            InitializeComponent();
            this.verCheck = verCheck;
            this.DataContext = verCheck;
            WpfHelper.CenterChildWindow(this);
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            verCheck.OpenDownloadSite();
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(verCheck.LatestVersionDownloadUrl);
            System.Windows.Forms.MessageBox.Show("The download site URL has been copied to the clipboard.", "Clipboard");
        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
