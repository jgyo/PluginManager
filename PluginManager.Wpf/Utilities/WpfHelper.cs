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
        /// Finds the parent window of the specified child.
        /// </summary>
        /// <param name="child">The child<see cref="DependencyObject"/>.</param>
        /// <returns>The <see cref="Window"/>.</returns>
        internal static Window FindWidow(DependencyObject child)
        {
            return FindAncestor<Window>(child);
        }
    }
}