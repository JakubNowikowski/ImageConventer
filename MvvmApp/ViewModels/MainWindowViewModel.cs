using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MyImageLib;
using System.Windows.Media;
using MvvmApp.Services;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MvvmApp.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        #region Fields

        public string convertedImageDescription = "Converted image:";
        public string convertedTimeDescription = "Converting time (min/sec/ms): ";
        public string originalImageDescription = "Original image:";
        public string sizeIsTooBigDescriprion = "Image size is too big to convert";
        IFileService _fileDialogService;
        IProcessing _processing;
        private BitmapImage _orgImage;
        private ImageSource _newImage;
        private string _orgImageLabel;
        private string _newImageLabel;
        private string _orgFilePath;
        private string _convertingTimeLabel;
        private string _sizeIsTooBigLabel;
        private string _errorLabel;
        private bool _isConvertEnabled;
        private bool _isSaveEnabled;
        private bool _convertNormally;
        private bool _convertAsynchronously;
        private bool _convertUsingCpp;
        private List<ConvertMode> _convertOpitons;
        private ConvertMode _selectedConverOption = ConvertMode.Normally;

        #endregion

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

        public BitmapImage OrgImage
        {
            get { return _orgImage; }
            set
            {
                _orgImage = value;
                RaisePropertyChangedEvent("OrgImage");
            }
        }

        public ImageSource NewImage
        {
            get { return _newImage; }
            set
            {
                _newImage = value;
                RaisePropertyChangedEvent("NewImage");
            }
        }

        public string OrgImageLabel
        {
            get { return _orgImageLabel; }
            set
            {
                _orgImageLabel = value;
                RaisePropertyChangedEvent("OrgImageLabel");
            }
        }

        public string NewImageLabel
        {
            get { return _newImageLabel; }
            set
            {
                _newImageLabel = value;
                RaisePropertyChangedEvent("NewImageLabel");
            }
        }

        public string OrgFilePath
        {
            get { return _orgFilePath; }
            set
            {
                _orgFilePath = value;
                RaisePropertyChangedEvent("OrgFilePath");
            }
        }

        public string ConvertingTimeLabel
        {
            get { return _convertingTimeLabel; }
            set
            {
                _convertingTimeLabel = value;
                RaisePropertyChangedEvent("ConvertingTimeLabel");
            }
        }

        public string SizeIsTooBigLabel
        {
            get { return _sizeIsTooBigLabel; }
            set
            {
                _sizeIsTooBigLabel = value;
                RaisePropertyChangedEvent("SizeIsTooBigLabel");
            }
        }
        
        public string ErrorLabel
        {
            get { return _errorLabel; }
            set
            {
                _errorLabel = value;
                //RaisePropertyChangedEvent("ErrorLabel");
            }
        }

        public bool IsConvertEnabled
        {
            get { return _isConvertEnabled; }
            set
            {
                _isConvertEnabled = value;
                RaisePropertyChangedEvent("IsConvertEnabled");
            }
        }

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

        public bool ConvertNormally
        {
            get { return _convertNormally; }
            set
            {
                _convertNormally = value;
                RaisePropertyChangedEvent("ConvertNormally");
            }
        }

        public bool ConvertAsynchronously
        {
            get { return _convertAsynchronously; }
            set
            {
                _convertAsynchronously = value;
                RaisePropertyChangedEvent("ConvertAsynchronously");
            }
        }

        public bool ConvertUsingCpp
        {
            get { return _convertUsingCpp; }
            set
            {
                _convertUsingCpp = value;
                RaisePropertyChangedEvent("ConvertUsingCpp");
            }
        }

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

        #region Constructor

        public MainWindowViewModel(IFileService fileDialogService, IProcessing processing)
        {
            _fileDialogService = fileDialogService;
            _processing = processing;
        }

        #endregion

        #region Methods

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

        #endregion
    }
}
