using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Neurio.Client;
using Neurio.ViewModels;

namespace Neurio.Commands
{
    public abstract class BaseCommand : ICommand
    {
        protected readonly NeurioClient _neurioClient;
        protected readonly BaseViewModel _viewModel;

        public BaseCommand(NeurioClient neurioClient, BaseViewModel viewModel)
        {
            _neurioClient = neurioClient;
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public abstract void Execute(object parameter);

        public event EventHandler CanExecuteChanged;

    }
}
