namespace PluginManager.Wpf
{
    using PluginManager.Wpf.Utilities;
    using PluginManager.Wpf.Windows;
    using System;
    using System.ComponentModel;
    using System.Reflection;
    using System.Windows;
    using VersionManagement;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            WpfHelper.IntitializeWindowSettings();
            this.Height = WpfHelper.WindowHeight;
            this.Width = WpfHelper.WindowWidth;
            this.Top = WpfHelper.WindowTop;
            this.Left = WpfHelper.WindowLeft;
            this.WindowState = WpfHelper.WindowState;
        }

        /// <summary>
        /// The OnClosing.
        /// </summary>
        /// <param name="e">The e<see cref="CancelEventArgs"/>.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            WpfHelper.SetWindowSettings(this);
            WpfHelper.SaveWindowSettings();

            base.OnClosing(e);
        }

        /// <summary>
        /// The OnContentRendered.
        /// </summary>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            if (AppSettings.Default.VersionAutoCheck)
            {
                var version = Application.Current.MainWindow.GetType()
                    .Assembly
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                    .InformationalVersion;
                var verCheck = new VersionCheck(version, AppSettings.Default.PackageUrl);
                if (verCheck.DoesUpdateExist && (AppSettings.Default.IncludePrereleaseVersions || verCheck.LastestVersionIsPrerelease == false))
                {
                    var win = new UpdateWindow(verCheck);
                    WpfHelper.SetWindowSettings(this);
                    win.ShowDialog();
                }
            }
        }
    }
}
