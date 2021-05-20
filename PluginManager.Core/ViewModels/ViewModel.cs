namespace PluginManager.Core.ViewModels
{
    using global::System.Runtime.CompilerServices;
    using PluginManager.Core.Collections;

    /// <summary>
    /// Defines the <see cref="ViewModel" />.
    /// </summary>
    public class ViewModel : ObservableObject
    {
        /// <summary>
        /// The RaisePropertyChanged.
        /// </summary>
        /// <param name="memberName">The memberName<see cref="string"/>.</param>
        public void RaisePropertyChanged([CallerMemberName] string memberName = null)
        {
            NotifyPropertyChanged(memberName);
        }
    }
}
