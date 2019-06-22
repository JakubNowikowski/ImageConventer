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
        public string convertedImageDescription = "Converted image:";
        public string convertedTimeDescription = "Converting time (min/sec/ms): ";
        public string originalImageDescription = "Original image:";
        IFileService _fileDialogService;
        IProcessing _processing;

        public Presenter(IFileService fileDialogService, IProcessing processing)
        {
            _fileDialogService = fileDialogService;
            _processing = processing;
        }

        #region Properties

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
            if (_fileDialogService.TryOpenDialog(out var filePath))
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

            byte[] loadImageByteArr = _processing.ConvertImageToByteArray(OrgImage);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //loadImageByteArr = _processing.ConvertSync(loadImageByteArr);
            //loadImageByteArr = _processing.ConvertCpp(loadImageByteArr);
            loadImageByteArr = await _processing.ConvertAsync2(loadImageByteArr);
            stopwatch.Stop();
            NewImage = await _processing.CreateNewConvertedImage(OrgImage, loadImageByteArr);
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
            _fileDialogService.SaveDialog(NewImage, OrgFilePath);
        }
    }
}
