using System.ComponentModel;

namespace MvvmApp.ViewModels
{
	public abstract class ObservableObject : INotifyPropertyChanged
	{
        #region Methods

        protected void RaisePropertyChangedEvent(string propertyName)
		{
			var handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}

        #endregion

        #region INotifyPropertyChanged interface

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
	}
}
