namespace PluginManager.Wpf.Windows
{
    using PluginManager.Wpf.Utilities;
    using System.Windows;

    /// <summary>
    /// Interaction logic for AboutWindow.xaml.
    /// </summary>
    public partial class AboutWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutWindow"/> class.
        /// </summary>
        public AboutWindow()
        {
            InitializeComponent();
            WpfHelper.CenterChildWindow(this);
        }
    }
}
