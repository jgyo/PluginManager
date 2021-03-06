namespace PluginManager.Wpf.Views
{
    using Ookii.Dialogs.Wpf;
    using PluginManager.Core.EventHandlers;
    using PluginManager.Core.Logging;
    using PluginManager.Core.ViewModels;
    using PluginManager.Core.ViewModels.DesignTime;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for SetupView.xaml.
    /// </summary>
    public partial class SetupView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetupView"/> class.
        /// </summary>
        public SetupView()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                this.DataContext = new SetupViewModelDesignTime();
            }
            else
            {
                this.DataContext = Locator.SetupViewModel;
                Initialize(Locator.SetupViewModel);
            }

            this.Loaded += SetupView_Loaded;
        }

        private void SetupView_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // The window does not exist in the design window
                var win = Window.GetWindow(this);
                win.Closing += Win_Closing;
            }
            catch (System.Exception)
            {
                return;
            }
        }

        private void Win_Closing(object sender, CancelEventArgs e)
        {
            var setup = DataContext as SetupViewModel;
            Debug.Assert(setup != null);

            AppSettings.Default.CommunityFolder = setup.CommunityFolder;
            AppSettings.Default.HiddenFilesFolder = setup.HiddenFilesFolder;
            AppSettings.Default.ZipFilesFolder = setup.ZipFilesFolder;
            AppSettings.Default.LoggingEnabled = setup.LoggingEnabled;
            AppSettings.Default.LogLevel = (int)setup.LoggingLevel;
            AppSettings.Default.VersionAutoCheck = setup.CheckForUpdates;
            AppSettings.Default.IncludePrereleaseVersions = setup.IncludePrereleaseVersions;

            AppSettings.Default.Save();

            FileLogProvider.Instance.LoggingEnabled = setup.LoggingEnabled;
            FileLogProvider.Instance.LogLevel = setup.LoggingLevel;

            FileLogProvider.Instance.GetLogFor<SetupView>().Info("SetupViewModel saved.");

            setup.AcceptChangesRequested -= Setup_AcceptChangesRequested;
            setup.BrowseForFolderRequested -= Setup_BrowseForFolderRequested;
        }

        // #SavesSetup
        /// <summary>
        /// Handles the AcceptChangesRequested event.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="ViewModelEventArgs"/>.</param>
        private void Setup_AcceptChangesRequested(object sender, ViewModelEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.Close();
        }

        /// <summary>
        /// Handles the BrowseForFolderRequested event.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="BrowserEventArgs"/>.</param>
        private void Setup_BrowseForFolderRequested(object sender, BrowserEventArgs e)
        {
            var d = new VistaFolderBrowserDialog
            {
                Description = e.Description,
                UseDescriptionForTitle = true,
                ShowNewFolderButton = true,
                SelectedPath = e.Folder
            };

            if (d.ShowDialog() == true)
            {
                e.Folder = d.SelectedPath;
            }
        }

        /// <summary>
        /// Initializes the SetupView class.
        /// </summary>
        /// <param name="vm">The vm<see cref="SetupViewModel"/>.</param>
        private void Initialize(SetupViewModel vm)
        {
            Debug.Assert(vm != null);
            vm.BrowseForFolderRequested += Setup_BrowseForFolderRequested;
            vm.AcceptChangesRequested += Setup_AcceptChangesRequested;
        }
    }
}