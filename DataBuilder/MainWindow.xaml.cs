using Ookii.Dialogs.Wpf;
using PluginManager.Data.Models;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace DataBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Scan_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Folder.Text))
                return;

            var folder = Folder.Text;
            var zipFiles = new List<ZipFile>();

            foreach (var ext in new[] { "*.zip", "*.7z" })
            {
                var files = Directory.GetFiles(folder, ext);
                foreach (var filename in files)
                {
                    var fileInfo = new FileInfo(filename);
                    var zipfile = new ZipFile();
                    zipfile.GetFileInfo(fileInfo);
                    zipFiles.Add(zipfile);
                }
            }

            collection.ItemsSource = zipFiles;
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            var win = new VistaFolderBrowserDialog();
            var result = win.ShowDialog();

            if (result == true)
            {
                Folder.Text = win.SelectedPath;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var collection = this.collection.ItemsSource;
            var pmdb = new PmDb();

            foreach (var item in collection)
            {
                pmdb.ZipFiles.Add(item as ZipFile);
            }
            var number = pmdb.SaveChanges();

            System.Windows.Forms.MessageBox.Show($"Number saved: {number}");
        }
    }
}