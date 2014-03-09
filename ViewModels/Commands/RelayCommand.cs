using System;
using System.Windows.Input;
using Models;

namespace ViewModels.Commands
{
    internal class RelayCommand : ICommand
    {
        #region Instance fields

        private readonly Action<object> _action;

        #endregion

        #region Constructors

        public RelayCommand(Action<object> action)
        {
            _action = action;
        }

        #endregion

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            var returnValue = false;

            var advertisement = parameter as Advertisement;
            var newspaper = parameter as Newspaper;

            if(advertisement != null)
                if (!advertisement.HasErrors && (!advertisement.IsUnchanged))
                    returnValue = true;

            if(newspaper != null)
                if (newspaper.HasErrors && (!newspaper.IsUnchanged))
                    returnValue = true;

            return returnValue;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (parameter != null)
                _action(parameter);
        }

        #endregion
    }
}