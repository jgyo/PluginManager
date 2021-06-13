namespace PluginManager.Wpf.Windows
{
    using PluginManager.Core.ViewModels;
    using PluginManager.Wpf.Utilities;
    using System.Windows;

    /// <summary>
    /// Interaction logic for ZipArchiveWindow.xaml.
    /// </summary>
    public partial class ZipArchiveWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZipArchiveWindow"/> class.
        /// </summary>
        /// <param name="zfr">The zfr<see cref="Core.ViewModels.ZipFileViewModel"/>.</param>
        public ZipArchiveWindow(Core.ViewModels.ZipFileViewModel zfr)
        {
            InitializeComponent();
            WpfHelper.PositionChildWindow(this);

            IArchiveViewModel archive;
            if (zfr.Filename.ToLower().EndsWith(".7z"))
            {
                archive = new SevenZipArchiveViewModel(zfr.Filename, zfr.FilePath, zfr.PackageId);
            }
            else
            {
                archive = new ZipArchiveViewModel(zfr.Filename, zfr.FilePath, zfr.PackageId);
            }

            view.DataContext = archive;
        }
    }
}
