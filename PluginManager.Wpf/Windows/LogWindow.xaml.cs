namespace PluginManager.Wpf.Windows
{
    using PluginManager.Wpf.ViewModels;
    using System.ComponentModel;
    using System.Windows;

    /// <summary>
    /// Interaction logic for LogWindow.xaml.
    /// </summary>
    public partial class LogWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogWindow"/> class.
        /// </summary>
        public LogWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The OnClosing.
        /// </summary>
        /// <param name="e">The e<see cref="CancelEventArgs"/>.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            var vm = DataContext as LogViewModel;
            if (vm.IsModified)
            {
                var result = MessageBox.Show("Save changes?", "Text Modified", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                    vm.SaveLogCommand.Execute(null);
                if (result == MessageBoxResult.Cancel)
                    e.Cancel = true;
            }
            base.OnClosing(e);
        }

        /// <summary>
        /// The DoneButtonClick.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/>.</param>
        private void DoneButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
