using System;
using System.Windows.Input;

namespace Standard
{
    public class RelayCommand : ICommand
    {
        private readonly CanExecuteHandler<object> canExecute;
        private readonly ExecuteHandler<object> execute;

        /// <summary>Событие извещающее об изменении состояния команды.</summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>Конструктор команды.</summary>
        /// <param name="execute">Выполняемый метод команды.</param>
        /// <param name="canExecute">Метод, возвращающий состояние команды.</param>
        public RelayCommand(ExecuteHandler<object> execute, CanExecuteHandler<object> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        /// <inheritdoc cref="RelayCommand(ExecuteHandler, CanExecuteHandler)"/>
        public RelayCommand(ExecuteHandler<object> execute)
            : this(execute, AlwaysTrue) { }

        /// <inheritdoc cref="RelayCommand(ExecuteHandler, CanExecuteHandler)"/>
        public RelayCommand(ExecuteHandler execute, CanExecuteHandler canExecute)
                : this
                (
                      Create(execute),
                      Create(canExecute)
                )
        { }

        /// <inheritdoc cref="RelayCommand(ExecuteHandler, CanExecuteHandler)"/>
        public RelayCommand(ExecuteHandler execute)
                : this
                (
                      Create(execute),
                      AlwaysTrue
                )
        { }

        private static CanExecuteHandler<object> Create(CanExecuteHandler canExecute)
        {
            if (canExecute is null) throw new ArgumentNullException(nameof(execute));
            return p => canExecute();
        }

        private static ExecuteHandler<object> Create(ExecuteHandler execute)
        {
            if (execute is null) throw new ArgumentNullException(nameof(execute));
            return p => execute();
        }

        public static bool AlwaysTrue(object _) => true;


        /// <summary>Метод, подымающий событие <see cref="CanExecuteChanged"/>.</summary>
        public virtual void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        /// <summary>Вызов метода, возвращающего состояние команды.</summary>
        /// <param name="parameter">Параметр команды.</param>
        /// <returns><see langword="true"/> - если выполнение команды разрешено.</returns>
        public bool CanExecute(object parameter)
        {
            return canExecute(parameter);
        }

        /// <summary>Вызов выполняющего метода команды.</summary>
        /// <param name="parameter">Параметр команды.</param>
        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }

}
