namespace PluginManager.Wpf
{
    using PluginManager.Wpf.Utilities;
    using System.ComponentModel;
    using System.Windows;

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

        protected override void OnClosing(CancelEventArgs e)
        {
            WpfHelper.WindowHeight = this.Height;
            WpfHelper.WindowWidth = this.Width;
            WpfHelper.WindowTop = this.Top;
            WpfHelper.WindowLeft = this.Left;
            WpfHelper.WindowState = this.WindowState;

            WpfHelper.SaveWindowSettings();

            base.OnClosing(e);
        }
    }
}
