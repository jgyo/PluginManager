namespace PluginManager.Wpf.Utilities
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Defines the <see cref="WpfHelper" />.
    /// </summary>
    public static class WpfHelper
    {
        /// <summary>
        /// Defines the windowHeight.
        /// </summary>
        private static double windowHeight;

        /// <summary>
        /// Defines the windowLeft.
        /// </summary>
        private static double windowLeft;

        /// <summary>
        /// Defines the windowState.
        /// </summary>
        private static WindowState windowState;

        /// <summary>
        /// Defines the windowTop.
        /// </summary>
        private static double windowTop;

        /// <summary>
        /// Defines the windowWidth.
        /// </summary>
        private static double windowWidth;

        /// <summary>
        /// Gets or sets the WindowHeight.
        /// </summary>
        public static double WindowHeight { get => windowHeight; set => windowHeight = value; }

        /// <summary>
        /// Gets or sets the WindowLeft.
        /// </summary>
        public static double WindowLeft { get => windowLeft; set => windowLeft = value; }

        /// <summary>
        /// Gets the WindowState.
        /// </summary>
        public static WindowState WindowState { get => windowState; set => windowState = value; }

        /// <summary>
        /// Gets or sets the WindowTop.
        /// </summary>
        public static double WindowTop { get => windowTop; set => windowTop = value; }

        /// <summary>
        /// Gets or sets the WindowWidth.
        /// </summary>
        public static double WindowWidth { get => windowWidth; set => windowWidth = value; }

        /// <summary>
        /// Finds an ancestor of type T of the specified child.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="child">The child<see cref="DependencyObject"/>.</param>
        /// <returns>The <see cref="T"/>.</returns>
        public static T FindAncestor<T>(DependencyObject child) where T : DependencyObject
        {
            var parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject is T parent)
                return parent;

            return FindAncestor<T>(parentObject);
        }

        /// <summary>
        /// The IntitializeWindowSettings.
        /// </summary>
        public static void IntitializeWindowSettings()
        {
            LoadWindowSettings();
            SizeWindowToFit();
            MoveWindowIntoView();
        }

        /// <summary>
        /// The MoveWindowIntoView.
        /// </summary>
        public static void MoveWindowIntoView()
        {
            if (windowTop + (windowHeight / 2) >
                 SystemParameters.VirtualScreenHeight)
            {
                windowTop =
                  SystemParameters.VirtualScreenHeight -
                  windowHeight;
            }

            if (windowLeft + (windowWidth / 2) >
                     SystemParameters.VirtualScreenWidth)
            {
                windowLeft =
                  SystemParameters.VirtualScreenWidth -
                  windowWidth;
            }

            if (windowTop < 0)
            {
                windowTop = 0;
            }

            if (windowLeft < 0)
            {
                windowLeft = 0;
            }
        }

        /// <summary>
        /// The SaveWindowSettings.
        /// </summary>
        public static void SaveWindowSettings()
        {
            if (WindowState == WindowState.Minimized)
                return;

            AppSettings.Default.WindowTop = windowTop;
            AppSettings.Default.WindowLeft = windowLeft;
            AppSettings.Default.WindowHeight = windowHeight;
            AppSettings.Default.WindowWidth = windowWidth;
            AppSettings.Default.WindowState = WindowState;

            AppSettings.Default.Save();
        }

        /// <summary>
        /// The SizeWindowToFit.
        /// </summary>
        public static void SizeWindowToFit()
        {
            windowHeight = windowHeight < SystemParameters.VirtualScreenHeight
                ? windowHeight
                : SystemParameters.VirtualScreenHeight;

            windowWidth = windowWidth < SystemParameters.VirtualScreenWidth
                ? windowWidth
                : SystemParameters.VirtualScreenWidth;
        }

        /// <summary>
        /// The LoadWindowSettings.
        /// </summary>
        private static void LoadWindowSettings()
        {
            windowTop = AppSettings.Default.WindowTop;
            windowLeft = AppSettings.Default.WindowLeft;
            windowHeight = AppSettings.Default.WindowHeight;
            windowWidth = AppSettings.Default.WindowWidth;
            WindowState = AppSettings.Default.WindowState;
        }
    }
}
