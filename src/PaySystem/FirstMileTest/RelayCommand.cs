﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FirstMileTest
{
    class RelayCommand : ICommand
    {

        private Action<object> _action;

        public RelayCommand(Action<object> action)
        {
            _action = action;

        }


        #region "Icommand Members"

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter != null)
            {
                _action(parameter);
            }
            else
            {
                _action("");
            }
        }

        #endregion
    }
}
