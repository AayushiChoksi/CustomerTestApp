﻿using System;
using System.Windows.Input;

namespace CustomersTestApp.Commands
{
    public class AddCustomerCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public AddCustomerCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
