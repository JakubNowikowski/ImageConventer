using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MvvmApp.ViewModels
{
    public class ImageConverter : IValueConverter
	{
        #region Methods

        public object Convert(
			object value, Type targetType, object parameter, CultureInfo culture)
		{
			return new BitmapImage(new Uri(value.ToString()));
		}

		public object ConvertBack(
			object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}

        #endregion
    }
}
