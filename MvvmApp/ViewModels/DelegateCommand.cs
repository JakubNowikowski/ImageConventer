using System;
using System.Windows.Input;

namespace MvvmApp.ViewModels
{
	public class DelegateCommand : ICommand
	{
        #region Fields

        private readonly Action _action;
		private object v;

        #endregion

        #region Methods

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

        #endregion

        #region ICommand Interface

#pragma warning disable 67
        public event EventHandler CanExecuteChanged { add { } remove { } }
#pragma warning restore 67

        #endregion
    }
}
