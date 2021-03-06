namespace PluginManager.Wpf.Windows
{
    using PluginManager.Wpf.Utilities;
    using System.Windows;

    /// <summary>
    /// Interaction logic for SetupWindow.xaml.
    /// </summary>
    public partial class SetupWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetupWindow"/> class.
        /// </summary>
        public SetupWindow()
        {
            InitializeComponent();
            WpfHelper.PositionChildWindow(this);
        }
    }
}
