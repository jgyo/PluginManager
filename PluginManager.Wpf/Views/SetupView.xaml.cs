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
        }

        // #SavesSetup
        /// <summary>
        /// Handles the AcceptChangesRequested event.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="ViewModelEventArgs"/>.</param>
        private void Setup_AcceptChangesRequested(object sender, ViewModelEventArgs e)
        {
            var setup = e.ViewModel as SetupViewModel;
            Debug.Assert(setup != null);

            AppSettings.Default.CommunityFolder = setup.CommunityFolder;
            AppSettings.Default.HiddenFilesFolder = setup.HiddenFilesFolder;
            AppSettings.Default.ZipFilesFolder = setup.ZipFilesFolder;
            AppSettings.Default.LoggingEnabled = setup.LoggingEnabled;
            AppSettings.Default.LogLevel = (int)setup.LoggingLevel;

            AppSettings.Default.Save();

            LogProvider.Instance.LoggingEnabled = setup.LoggingEnabled;
            LogProvider.Instance.LogLevel = setup.LoggingLevel;

            LogProvider.Instance.GetLogFor<SetupView>().Info("SetupViewModel saved.");

            setup.AcceptChangesRequested -= Setup_AcceptChangesRequested;
            setup.BrowseForFolderRequested -= Setup_BrowseForFolderRequested;

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