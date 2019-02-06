using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MobMentality.Command
{
    public class RelayCommand<T> : ICommand
    {
        #region private
        private Action<T> _execute;

        #endregion

        public RelayCommand(Action<T> execute)
        {
            _execute = execute;
        }


        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }
    }
}
