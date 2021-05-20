namespace PluginManager.Core.EventHandlers
{
    using global::System;
    using PluginManager.Core.ViewModels;

    /// <summary>
    /// Defines the <see cref="ViewModelEventArgs" />.
    /// </summary>
    public class ViewModelEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelEventArgs"/> class.
        /// </summary>
        /// <param name="vm">The vm<see cref="ViewModel"/>.</param>
        public ViewModelEventArgs(ViewModel vm)
        {
            ViewModel = vm;
        }

        /// <summary>
        /// Gets the ViewModel.
        /// </summary>
        public ViewModel ViewModel { get; private set; }
    }
}
