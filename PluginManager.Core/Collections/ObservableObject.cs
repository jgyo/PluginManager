namespace PluginManager.Core.Collections
{
    using global::System;
    using global::System.ComponentModel;
    using global::System.Runtime.CompilerServices;
    using PluginManager.Core.EventHandlers;

    /// <summary>
    /// Defines the <see cref="ObservableObject" />.
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Defines the PropertyChanged.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The NotifyPropertyChanged.
        /// </summary>
        /// <param name="callerMemberName">The callerMemberName<see cref="string"/>.</param>
        protected void NotifyPropertyChanged([CallerMemberName] string callerMemberName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerMemberName));
        }

        /// <summary>
        /// The NotifyPropertyChanged.
        /// </summary>
        /// <param name="oldValue">The oldValue<see cref="object"/>.</param>
        /// <param name="newValue">The newValue<see cref="object"/>.</param>
        /// <param name="callerMemberName">The callerMemberName<see cref="string"/>.</param>
        protected void NotifyPropertyChanged(object oldValue, object newValue, [CallerMemberName] string callerMemberName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedInfoEventArgs(oldValue, newValue, callerMemberName));
        }

        /// <summary>
        /// The SetProperty.
        /// </summary>
        /// <param name="field">The field<see cref="IntPtr"/>.</param>
        /// <param name="newValue">The newValue<see cref="IntPtr"/>.</param>
        /// <param name="propertyName">The propertyName<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        protected bool SetProperty(ref IntPtr field, IntPtr newValue, [CallerMemberName] string propertyName = null)
        {
            if (field == newValue)
                return false;

            object oldValue = field;
            field = newValue;
            NotifyPropertyChanged(oldValue, newValue, propertyName);

            return true;
        }

        /// <summary>
        /// The SetProperty.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="field">The field<see cref="T"/>.</param>
        /// <param name="newValue">The newValue<see cref="T"/>.</param>
        /// <param name="propertyName">The propertyName<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if ((field == null && newValue == null) || (field != null && field.Equals(newValue)))
                return false;

            T oldValue = field;
            field = newValue;
            NotifyPropertyChanged(oldValue, newValue, propertyName);

            return true;
        }

        /// <summary>
        /// The SetProperty.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="field">The field<see cref="T"/>.</param>
        /// <param name="newValue">The newValue<see cref="T"/>.</param>
        /// <param name="beforeNotifyAction">The beforeNotifyAction<see cref="Action"/>.</param>
        /// <param name="propertyName">The propertyName<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        protected bool SetProperty<T>(ref T field, T newValue, Action beforeNotifyAction, [CallerMemberName] string propertyName = null)
        {
            if ((field == null && newValue == null) || (field != null && field.Equals(newValue)))
                return false;

            T oldValue = field;
            field = newValue;
            beforeNotifyAction();
            NotifyPropertyChanged(oldValue, newValue, propertyName);

            return true;
        }

        /// <summary>
        /// The SetProperty.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="field">The field<see cref="T"/>.</param>
        /// <param name="newValue">The newValue<see cref="T"/>.</param>
        /// <param name="beforeNotifyAction">The beforeNotifyAction<see cref="Action{T, T}"/>.</param>
        /// <param name="propertyName">The propertyName<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        protected bool SetProperty<T>(ref T field, T newValue, Action<T, T> beforeNotifyAction, [CallerMemberName] string propertyName = null)
        {
            if ((field == null && newValue == null) || (field != null && field.Equals(newValue)))
                return false;

            T oldValue = field;
            field = newValue;
            beforeNotifyAction(oldValue, newValue);
            NotifyPropertyChanged(oldValue, newValue, propertyName);

            return true;
        }

        /// <summary>
        /// The SetProperty.
        /// </summary>
        /// <param name="field">The field<see cref="UIntPtr"/>.</param>
        /// <param name="newValue">The newValue<see cref="UIntPtr"/>.</param>
        /// <param name="propertyName">The propertyName<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        protected bool SetProperty(ref UIntPtr field, UIntPtr newValue, [CallerMemberName] string propertyName = null)
        {
            if (field == newValue)
                return false;

            object oldValue = field;
            field = newValue;
            NotifyPropertyChanged(oldValue, newValue, propertyName);

            return true;
        }
    }
}
