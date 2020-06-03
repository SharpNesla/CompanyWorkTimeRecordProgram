using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace employees
{
    /// <summary>
    /// Обобщённая команда, принимающая в качестве
    /// действия делегат процедуры без параметра
    /// </summary>
    class RelayCommand : ICommand
    {
        private readonly Action _executeAction;

        public RelayCommand(Action executeAction)
        {
            _executeAction = executeAction;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _executeAction();
        }

        public event EventHandler CanExecuteChanged;
    }
    /// <summary>
    /// Обобщённая команда, принимающая в качестве
    /// действия делегат процедуры с параметром T (класс)
    /// </summary>
    /// <typeparam name="T">Тип параметра процедуры</typeparam>
    public class RelayCommand<T> : ICommand where T : class
    {
        private readonly Action<T> _executeAction;

        public RelayCommand(Action<T> executeAction)
        {
            _executeAction = executeAction;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _executeAction(parameter as T);
        }

        public event EventHandler CanExecuteChanged;
    }
}
