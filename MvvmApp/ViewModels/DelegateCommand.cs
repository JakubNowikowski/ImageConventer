using System;
using System.Windows.Input;

namespace MvvmApp.ViewModels
{
	public class DelegateCommand : ICommand
	{
		private readonly Action _action;
		private object v;

		public DelegateCommand(Action action)
		{
			_action = action;
		}

		public DelegateCommand(object v)
		{
			this.v = v;
		}

		public void Execute(object parameter)
		{
			_action();
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

#pragma warning disable 67
		public event EventHandler CanExecuteChanged { add { } remove { } }
#pragma warning restore 67
	}
}
