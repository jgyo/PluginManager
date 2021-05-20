namespace PluginManager.Core.EventHandlers
{
    using global::System.ComponentModel;

    /// <summary>
    /// Defines the <see cref="PropertyChangedInfoEventArgs" />.
    /// </summary>
    internal class PropertyChangedInfoEventArgs : PropertyChangedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyChangedInfoEventArgs"/> class.
        /// </summary>
        /// <param name="oldValue">The oldValue<see cref="object"/>.</param>
        /// <param name="newValue">The newValue<see cref="object"/>.</param>
        /// <param name="callerMemberName">The callerMemberName<see cref="string"/>.</param>
        public PropertyChangedInfoEventArgs(object oldValue, object newValue, string callerMemberName) : base(callerMemberName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>
        /// Gets the NewValue.
        /// </summary>
        public object NewValue { get; }

        /// <summary>
        /// Gets the OldValue.
        /// </summary>
        public object OldValue { get; }
    }
}
