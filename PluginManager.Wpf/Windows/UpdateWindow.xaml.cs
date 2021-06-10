namespace PluginManager.Wpf.Windows
{
    using PluginManager.Wpf.Utilities;
    using System.Windows;
    using VersionManagement;

    /// <summary>
    /// Interaction logic for UpdateWindow.xaml.
    /// </summary>
    public partial class UpdateWindow : Window
    {
        /// <summary>
        /// Defines the verCheck.
        /// </summary>
        private VersionCheck verCheck;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateWindow"/> class.
        /// </summary>
        public UpdateWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateWindow"/> class.
        /// </summary>
        /// <param name="verCheck">The verCheck<see cref="VersionCheck"/>.</param>
        public UpdateWindow(VersionCheck verCheck)
        {
            InitializeComponent();
            this.verCheck = verCheck;
            this.DataContext = verCheck;
            WpfHelper.CenterChildWindow(this);
        }

        /// <summary>
        /// The Copy_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/>.</param>
        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(verCheck.LatestVersionDownloadUrl);
            System.Windows.Forms.MessageBox.Show("The download site URL has been copied to the clipboard.", "Clipboard");
        }

        /// <summary>
        /// The Done_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/>.</param>
        private void Done_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// The Open_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/>.</param>
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            verCheck.OpenDownloadSite();
        }
    }
}
