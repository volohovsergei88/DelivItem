using Standard;
using System;
using System.Windows.Input;
using static System.Windows.Application;

namespace Wpf
{
    public class RelayCommand : Standard.RelayCommand
    {
        /// <summary>Конструктор команды.</summary>
        /// <param name="execute">Выполняемый метод команды.</param>
        /// <param name="canExecute">Метод, возвращающий состояние команды.</param>
        public RelayCommand(ExecuteHandler<object> execute, CanExecuteHandler<object> canExecute = null)
            : base(execute, canExecute)
        {
            requerySuggested = (s, e) => base.RaiseCanExecuteChanged();
            CommandManager.RequerySuggested += requerySuggested;
        }

        /// <inheritdoc cref="RelayCommand(ExecuteHandler, CanExecuteHandler)"/>
        public RelayCommand(ExecuteHandler execute, CanExecuteHandler canExecute = null)
            : base(execute, canExecute)
        {
            requerySuggested = (s, e) => base.RaiseCanExecuteChanged();
            CommandManager.RequerySuggested += requerySuggested;
        }

        /// <summary>Метод, подымающий событие <see cref="CanExecuteChanged"/>.</summary>
        public override void RaiseCanExecuteChanged()
        {
            if (Current.Dispatcher.CheckAccess())
                base.RaiseCanExecuteChanged();
            else
                Current.Dispatcher.Invoke(base.RaiseCanExecuteChanged);
        }

        private readonly EventHandler requerySuggested;
    }
}
