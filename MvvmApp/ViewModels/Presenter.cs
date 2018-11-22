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

namespace MvvmApp.ViewModels
{

	public class Presenter : ObservableObject
	{
		ImageProcessing imgProc = new ImageProcessing();
		int start, stop;
		byte[] OrgByteArray;

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

		private string _procTimeLabel;
		public string ProcTimeLabel
		{
			get { return _procTimeLabel; }
			set
			{
				_procTimeLabel = value;
				RaisePropertyChangedEvent("ProcTimeLabel");
			}
		}

		private string _errorLabel;
		public string ErrorLabel
		{
			get { return _errorLabel; }
			set
			{
				_errorLabel = value;
				RaisePropertyChangedEvent("ErrorLabel");
			}
		}

		public ICommand LoadImage
		{
			get { return new DelegateCommand(LoadOrgImage); }
		}

		public ICommand ProcessImage
		{
			get { return new DelegateCommand(ProcImage); }
		}

		public ICommand SaveImage
		{
			get { return new DelegateCommand(SaveNewImage); }
		}

		public void LoadOrgImage()
		{

			//////////////////////////////Test
			ImageSource img;
			img = null;
			byte[] Arr = new byte[4];
			Arr[0] = 255;
			Arr[1] = 255;
			Arr[2] = 255;
			Arr[3] = 255;

			img= imgProc.ToMainColors(Arr, 1, 1);

			///////////////////////////////

			ClearLabels();
			ClearImages();
			OpenFileDialog(imgProc);
			OrgImage = new BitmapImage(new Uri(OrgFilePath, UriKind.RelativeOrAbsolute));
			if (OrgFilePath != "error")
				OrgImageLabel = "Oryginał:";
		}

		//public void OpenFileDialog()
		//{
		//	OrgFilePath = imgProc.OpenDialog();
		//}

		public bool OpenFileDialog(ImageProcessing obj)
		{
			imgProc.OpenDialog();

			return true;
		}


		public void ClearLabels()
		{
			ProcTimeLabel = null;
			NewImageLabel = null;
			OrgImageLabel = null;
			ErrorLabel = null;
		}

		public void ClearImages()
		{
			OrgImage = null;
			NewImage = null;
		}

		public int ProcTime(int start, int stop)
		{
			return (stop - start);
		}

		public void ProcImage()
		{
			
			if (OrgImage != null && OrgFilePath != "error")
			{
				start = Environment.TickCount & Int32.MaxValue;

				OrgByteArray = imgProc.ImageToByteArray(OrgImage);

				imgProc.ConvertArray(OrgByteArray);

				NewImage = imgProc.ToMainColors(OrgByteArray, OrgImage.PixelWidth, OrgImage.PixelHeight);

				stop = Environment.TickCount & Int32.MaxValue;
				NewImageLabel = "Przetworzony obraz:";
				ProcTimeLabel = "Czas przetwarzania: " + ProcTime(start, stop) + " ms";
			}
			else
				ErrorLabel = "Uwaga: Nie można przetworzyć (wczytaj obraz)";

		}

		public void SaveNewImage()
		{
			if (NewImage != null)
				imgProc.SaveDialog(NewImage, OrgFilePath);
		}
	}
}
