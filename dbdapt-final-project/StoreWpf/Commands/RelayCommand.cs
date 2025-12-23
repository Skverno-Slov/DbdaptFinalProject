using System.Windows.Input;

namespace StoreWpf.Commands
{
    // моя крутая команда
    public class RelayCommand : ICommand
    {
        private Action<object> execute; //выполнение команды: делегат без возвращаемого значения
        private Func<object, bool> canExecute; // Может ли команда быть выполнена: делегат с возвращаемым булевым значением

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
            => canExecute == null || canExecute(parameter);

        public void Execute(object parameter)
            => execute(parameter);
    }
}
