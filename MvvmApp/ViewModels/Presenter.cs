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
        public string sizeIsTooBigDescriprion = "Image size is too big to convert";
        IFileService _fileDialogService;
        IProcessing _processing;

        public Presenter(IFileService fileDialogService, IProcessing processing)
        {
            _fileDialogService = fileDialogService;
            _processing = processing;
        }

        #region Properties

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

        private string _sizeIsTooBigLabel;
        public string SizeIsTooBigLabel
        {
            get { return _sizeIsTooBigLabel; }
            set
            {
                _sizeIsTooBigLabel = value;
                RaisePropertyChangedEvent("SizeIsTooBigLabel");
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

        private bool _convertNormally;
        public bool ConvertNormally
        {
            get { return _convertNormally; }
            set
            {
                _convertNormally = value;
                RaisePropertyChangedEvent("ConvertNormally");
            }
        }

        private bool _convertAsynchronously;
        public bool ConvertAsynchronously
        {
            get { return _convertAsynchronously; }
            set
            {
                _convertAsynchronously = value;
                RaisePropertyChangedEvent("ConvertAsynchronously");
            }
        }

        private bool _convertUsingCpp;
        public bool ConvertUsingCpp
        {
            get { return _convertUsingCpp; }
            set
            {
                _convertUsingCpp = value;
                RaisePropertyChangedEvent("ConvertUsingCpp");
            }
        }

        private List<ConvertMode> _convertOpitons;
        public List<ConvertMode> ConvertOpitons
        {
            get => new List<ConvertMode>()
            {
            ConvertMode.Normally,
            ConvertMode.Asynchronously,
            ConvertMode.UsingCpp,
            };
            set => _convertOpitons = value;
        }

        private ConvertMode _selectedConverOption = ConvertMode.Normally;
        public ConvertMode SelectedConvertOption
        {
            get => _selectedConverOption;
            set
            {
                _selectedConverOption = value;
                RaisePropertyChangedEvent("SelectedConvertOption");
            }
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

        private bool IsImageSizeValid(BitmapImage image)
        {
            if (image == null)
                return false;

            return false;
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
            SizeIsTooBigLabel = null;
        }

        private void ClearImages()
        {
            OrgImage = null;
            NewImage = null;
        }

        public async Task Convert()
        {
            if (OrgImage == null || !IsSizeValid())
            {
                SizeIsTooBigLabel = sizeIsTooBigDescriprion;
                return;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            NewImage = await _processing.CreateNewConvertedImage(OrgImage, SelectedConvertOption);

            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;

            ShowLabels(ts);
        }

        private bool IsSizeValid()
        {
            var size = OrgImage.PixelHeight * OrgImage.PixelWidth;
            if (size < 80000000)
                return true;
            return false;
        }

        public void ShowLabels(TimeSpan ts)
        {
            NewImageLabel = convertedImageDescription;
            ConvertingTimeLabel = convertedTimeDescription + String.Format("{0:00}:{1:00}:{2:00}",
            ts.Minutes, ts.Seconds, ts.Milliseconds);
            IsSaveEnabled = true;
        }

        public void SaveAs()
        {
            if (NewImage == null)
                return;
            _fileDialogService.SaveDialog(NewImage, OrgFilePath);
        }
    }
}
