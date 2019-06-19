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

namespace MvvmApp.ViewModels
{

    public class Presenter : ObservableObject
    {
        ImageProcessing imgProc = new ImageProcessing();
        FileDialogService fileDialogService = new FileDialogService();
        byte[] OrgByteArray;

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
            get { return new DelegateCommand(Convert); }
        }

        public ICommand SaveImage
        {
            get { return new DelegateCommand(SaveAs); }
        }
        #endregion

        public void Load()
        {
            ErrorLabel = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            OnPropertyChanged(() => ErrorLabel);
            ClearAll();
            if (fileDialogService.TryOpenDialog(out var filePath))
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
            OrgImage = new BitmapImage(new Uri(filePath, UriKind.RelativeOrAbsolute));
            OrgImageLabel = "Original image:";
        }

        private void ClearAll()
        {
            ClearLabels();
            ClearImages();
        }

        public void ClearLabels()
        {
            ConvertingTimeLabel = null;
            NewImageLabel = null;
            OrgImageLabel = null;
        }

        public void ClearImages()
        {
            OrgImage = null;
            NewImage = null;
        }

        public void Convert()
        {
            if (OrgImage == null)
                return;
            int startTime, endTime, resultTime;
            startTime = endTime = resultTime = 0;

            startTime = CheckTime();

            //OrgByteArray = imgProc.ConvertImageToByteArray(OrgImage);
            //imgProc.ConvertArray(OrgByteArray);
            //NewImage = imgProc.ToMainColors(OrgByteArray, OrgImage.PixelWidth, OrgImage.PixelHeight);

            NewImage = imgProc.CreateNewConvertedImage(OrgImage);

            endTime = CheckTime();
            resultTime = endTime - startTime;
            IsSaveEnabled = true;
            ShowLabels(resultTime);
        }

        public void ShowLabels(int resultTime)
        {
            NewImageLabel = "Converted image:";
            ConvertingTimeLabel = "Converting time: " + resultTime + " ms";
        }

        private int CheckTime()
        {
            return Environment.TickCount & Int32.MaxValue;
        }

        public void SaveAs()
        {
            if (NewImage == null)
                return;
            fileDialogService.SaveDialog(NewImage, OrgFilePath);
        }
    }
}
