namespace PluginManager.Wpf
{
    using Ookii.Dialogs.Wpf;
    using PluginManager.Core;
    using PluginManager.Core.Logging;
    using PluginManager.Core.ViewModels;
    using PluginManager.Wpf.ViewModels;
    using PluginManager.Wpf.Windows;
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;

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

            FileLogProvider.Instance.LogLevel = vm.LoggingLevel;
            FileLogProvider.Instance.LoggingEnabled = vm.LoggingEnabled;

            var log = FileLogProvider.Instance.GetLogFor<App>();
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
        /// The WaitDialog.
        /// </summary>
        /// <param name="description">The description<see cref="string"/>.</param>
        /// <returns>The <see cref="WaitWindow"/>.</returns>
        public static WaitWindow WaitDialog(string description)
        {
            return new WaitWindow()
            {
                DataContext = new WaitWindowViewModel()
                {
                    WindowTitle = "Working",
                    Text = "Please wait.",
                    Description = description,
                    IsIndeterminate = true
                }
            };
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

        /// <summary>
        /// The OnStartup.
        /// </summary>
        /// <param name="e">The e<see cref="StartupEventArgs"/>.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SetupExceptionAhandling();
        }

        /// <summary>
        /// The App_DispatcherUnhandledException.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="DispatcherUnhandledExceptionEventArgs"/>.</param>
        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            LogUnhandledException(e.Exception, "App.DispatcherUnhandledException");
        }

        /// <summary>
        /// The CurrentDomain_UnhandledException.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="UnhandledExceptionEventArgs"/>.</param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogUnhandledException(e.ExceptionObject as Exception, "AppDomain.CurrentDomain.UnhandledException");
        }

        /// <summary>
        /// The LogUnhandledException.
        /// </summary>
        /// <param name="e">The e<see cref="Exception"/>.</param>
        /// <param name="source">The source<see cref="string"/>.</param>
        private void LogUnhandledException(Exception e, string source)
        {
            var message = $"Unhandled exception ({source})";
            var log = FileLogProvider.Instance.GetLogFor<App>();

            try
            {
                AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
                var assembly = Application.Current.MainWindow.GetType().Assembly;
                var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
                message = $"Unhandled exception in {assemblyName.Name} {version}";
            }
            catch (Exception ex)
            {
                log.Error(ex, "Exception in LogUnhandledException");
            }
            finally
            {
                log.Error(e, message);
                if (e.InnerException != null)
                    log.Error(e.InnerException, "Inner Exception");
            }
        }

        /// <summary>
        /// The SetupExceptionAhandling.
        /// </summary>
        private void SetupExceptionAhandling()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        /// <summary>
        /// The TaskScheduler_UnobservedTaskException.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="UnobservedTaskExceptionEventArgs"/>.</param>
        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            LogUnhandledException(e.Exception, "TaskScheduler.UnobservedTaskException");
        }
    }
}
