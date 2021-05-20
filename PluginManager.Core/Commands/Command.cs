namespace PluginManager.Core.Commands
{
    using global::System;
    using global::System.Diagnostics;
    using global::System.Windows.Input;

    /// <summary>
    /// Defines the <see cref="Command" />.
    /// </summary>
    public class Command : ICommand
    {
        /// <summary>
        /// Defines the canExecute.
        /// </summary>
        private readonly Func<object, bool> canExecute;

        /// <summary>
        /// Defines the execute.
        /// </summary>
        private readonly Action<object> execute;

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="execute">The execute<see cref="Action"/>.</param>
        public Command(Action execute)
        {
            Debug.Assert(execute != null);
            this.execute = (m) => execute();
            this.canExecute = (m) => true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="execute">The execute<see cref="Action"/>.</param>
        /// <param name="canExecute">The canExecute<see cref="Func{bool}"/>.</param>
        public Command(Action execute, Func<bool> canExecute)
        {
            Debug.Assert(execute != null);
            Debug.Assert(canExecute != null);
            this.execute = (m) => execute();
            this.canExecute = (m) => canExecute();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="execute">The execute<see cref="Action"/>.</param>
        /// <param name="canExecute">The canExecute<see cref="Func{object, bool}"/>.</param>
        public Command(Action execute, Func<object, bool> canExecute)
        {
            Debug.Assert(execute != null);
            Debug.Assert(canExecute != null);
            this.execute = (m) => execute();
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="execute">The execute<see cref="Action{object}"/>.</param>
        public Command(Action<object> execute)
        {
            Debug.Assert(execute != null);
            this.execute = execute;
            this.canExecute = (m) => true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="execute">The execute<see cref="Action{object}"/>.</param>
        /// <param name="canExecute">The canExecute<see cref="Func{bool}"/>.</param>
        public Command(Action<object> execute, Func<bool> canExecute)
        {
            Debug.Assert(execute != null);
            Debug.Assert(canExecute != null);
            this.execute = execute;
            this.canExecute = (m) => canExecute();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="execute">Action to execute.</param>
        /// <param name="canExecute">Function to test if the object can execute.</param>
        public Command(Action<object> execute, Func<object, bool> canExecute)
        {
            Debug.Assert(execute != null);
            Debug.Assert(canExecute != null);
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Defines the CanExecuteChanged.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// The CanExecute.
        /// </summary>
        /// <param name="parameter">The parameter<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool CanExecute(object parameter = null)
        {
            if (canExecute == null)
                return true;
            return canExecute(parameter);
        }

        /// <summary>
        /// The Execute.
        /// </summary>
        /// <param name="parameter">The parameter<see cref="object"/>.</param>
        public void Execute(object parameter = null)
        {
            execute(parameter);
        }

        /// <summary>
        /// The RaiseCanExecuteChanged.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
