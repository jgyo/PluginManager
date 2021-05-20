namespace PluginManager.Core.ViewModels
{
    using global::System;

    /// <summary>
    /// Defines the <see cref="BrowserEventArgs" />.
    /// </summary>
    public class BrowserEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Folder.
        /// </summary>
        public string Folder { get; set; }
    }
}
