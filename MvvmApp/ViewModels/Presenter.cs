using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MyImageLib;
using System.Drawing;
using System.Windows.Media;
using System.IO;
using System.Windows.Data;
using MvvmApp.Services;
using System.Windows.Forms;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MvvmApp.ViewModels
{

	public class Presenter : ObservableObject
	{
		string convertedImageDescription = "Converted image:";
		string convertedTimeDescription = "Converting time (min/sec/ms): ";
		public string originalImageDescription = "Original image:";
		IFileService _fileDialogService;

		public Presenter()
		{
			_fileDialogService = new FileDialogService();
		}

		#region Properties

		public IFileService FileServicePropMyProperty
		{
			get { return _fileDialogService; }
			set { _fileDialogService = value; }
		}

		private BitmapImage _orgImage;
		public BitmapImage OrgImage
		{
			get { return _orgImage; }
			set
			{
				_orgImage = value;
				RaisePropertyChangedEvent("OrgImage");
			}
		}

		private ImageSource _newImage;
		public ImageSource NewImage
		{
			get { return _newImage; }
			set
			{
				_newImage = value;
				RaisePropertyChangedEvent("NewImage");
			}
		}

		private string _orgImageLabel;
		public string OrgImageLabel
		{
			get { return _orgImageLabel; }
			set
			{
				_orgImageLabel = value;
				RaisePropertyChangedEvent("OrgImageLabel");
			}
		}

		private string _newImageLabel;
		public string NewImageLabel
		{
			get { return _newImageLabel; }
			set
			{
				_newImageLabel = value;
				RaisePropertyChangedEvent("NewImageLabel");
			}
		}

		private string _orgFilePath;
		public string OrgFilePath
		{
			get { return _orgFilePath; }
			set
			{
				_orgFilePath = value;
				RaisePropertyChangedEvent("OrgFilePath");
			}
		}

		private string _convertingTimeLabel;
		public string ConvertingTimeLabel
		{
			get { return _convertingTimeLabel; }
			set
			{
				_convertingTimeLabel = value;
				RaisePropertyChangedEvent("ConvertingTimeLabel");
			}
		}

		private string _errorLabel;

		public string ErrorLabel
		{
			get { return _errorLabel; }
			set
			{
				_errorLabel = value;
				//RaisePropertyChangedEvent("ErrorLabel");
			}
		}

		private bool _isConvertEnabled;
		public bool IsConvertEnabled
		{
			get { return _isConvertEnabled; }
			set
			{
				_isConvertEnabled = value;
				RaisePropertyChangedEvent("IsConvertEnabled");
			}
		}

		private bool _isSaveEnabled;

		public bool IsSaveEnabled
		{
			get { return _isSaveEnabled; }
			set
			{
				_isSaveEnabled = value;
				RaisePropertyChangedEvent("IsSaveEnabled");
			}
		}

		private void OnPropertyChanged<T>(Expression<Func<T>> expression)
		{
			var name = (expression.Body as MemberExpression).Member.Name;
			RaisePropertyChangedEvent(name);
		}

		public ICommand LoadImage
		{
			get { return new DelegateCommand(Load); }
		}

		public ICommand ConvertImage
		{
			get { return new DelegateCommand(async () => await Convert()); }
		}

		public ICommand SaveImage
		{
			get { return new DelegateCommand(SaveAs); }
		}
		#endregion

		public void Load()
		{
			//ErrorLabel = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
			//OnPropertyChanged(() => ErrorLabel);

			ClearAll();
			if (FileServicePropMyProperty.TryOpenDialog(out var filePath))
			{
				OrgFilePath = filePath;
				OpenImage(OrgFilePath);
				IsConvertEnabled = true;
			}
			else
			{
				IsConvertEnabled = false;
				IsSaveEnabled = false;
			}
		}

		private void OpenImage(string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
				throw new ArgumentNullException();
			OrgImage = new BitmapImage(new Uri(filePath, UriKind.RelativeOrAbsolute));
			OrgImageLabel = originalImageDescription;
		}

		private void ClearAll()
		{
			ClearLabels();
			ClearImages();
		}

		private void ClearLabels()
		{
			ConvertingTimeLabel = null;
			NewImageLabel = null;
			OrgImageLabel = null;
		}

		private void ClearImages()
		{
			OrgImage = null;
			NewImage = null;
		}

		public async Task Convert()
		{
			if (OrgImage == null)
				return;

			ImageProcessing imgProc = new ImageProcessing();
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();

			NewImage = await imgProc.CreateNewConvertedImage(OrgImage);

			stopwatch.Stop();
			TimeSpan ts = stopwatch.Elapsed;
			ShowLabels(ts);

			IsSaveEnabled = true;
		}

		private void ShowLabels(TimeSpan ts)
		{
			NewImageLabel = convertedImageDescription;
			ConvertingTimeLabel = convertedTimeDescription + String.Format("{0:00}:{1:00}:{2:00}",
			ts.Minutes, ts.Seconds, ts.Milliseconds);
		}

		public void SaveAs()
		{
			if (NewImage == null)
				return;
			FileServicePropMyProperty.SaveDialog(NewImage, OrgFilePath);
		}
	}
}
