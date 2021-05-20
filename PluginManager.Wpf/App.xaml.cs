namespace PluginManager.Wpf
{
    using Ookii.Dialogs.Wpf;
    using PluginManager.Core;
    using PluginManager.Core.Logging;
    using PluginManager.Core.ViewModels;
    using System.Windows;

    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            var vm = Locator.SetupViewModel;
            vm.CommunityFolder = AppSettings.Default.CommunityFolder;
            vm.ZipFilesFolder = AppSettings.Default.ZipFilesFolder;
            vm.HiddenFilesFolder = AppSettings.Default.HiddenFilesFolder;
            vm.LoggingLevel = (LogLevel)AppSettings.Default.LogLevel;
            vm.LoggingEnabled = AppSettings.Default.LoggingEnabled;

            LogProvider.Instance.LogLevel = vm.LoggingLevel;
            LogProvider.Instance.LoggingEnabled = vm.LoggingEnabled;

            var log = LogProvider.Instance.GetLogFor<App>();
            log.Info("Wpf.App logging started.");
            log.Info("SetupViewModel initialized.");

            DbCore.Initialize();
        }

        /// <summary>
        /// The LastChance.
        /// </summary>
        /// <param name="title">The title<see cref="string"/>.</param>
        /// <param name="heading">The heading<see cref="string"/>.</param>
        /// <param name="content">The content<see cref="string"/>.</param>
        /// <param name="warning">The warning<see cref="bool"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool LastChance(string title, string heading, string content, bool warning = false)
        {
            TaskDialog td;
            td = new();
            td.WindowTitle = title;
            td.MainInstruction = heading;
            td.Content = content;
            td.MainIcon = warning ? TaskDialogIcon.Warning : TaskDialogIcon.Information;
            td.Buttons.Add(new TaskDialogButton("Yes") { ButtonType = ButtonType.Yes });
            td.Buttons.Add(new TaskDialogButton("No") { ButtonType = ButtonType.No });
            return td.ShowDialog().ButtonType == ButtonType.Yes;
        }

        /// <summary>
        /// The Inform.
        /// </summary>
        /// <param name="title">The title<see cref="string"/>.</param>
        /// <param name="content">The content<see cref="string"/>.</param>
        internal static void Inform(string title, string content)
        {
            TaskDialog td;
            td = new();
            td.WindowTitle = title;
            td.Content = content;
            td.MainIcon = TaskDialogIcon.Information;
            td.Buttons.Add(new TaskDialogButton("Okay") { ButtonType = ButtonType.Ok });
            td.Show();
        }
    }
}
