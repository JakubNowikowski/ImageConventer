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

namespace MvvmApp.ViewModels
{

    public class Presenter : ObservableObject
    {
        ImageProcessing imgProc = new ImageProcessing();
        FileDialogService fileDialogService = new FileDialogService();
        OpenFileDialog opDlg = new OpenFileDialog();
        DialogResult dialogResult;

        int start, stop;
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
                RaisePropertyChangedEvent("ErrorLabel");
            }
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
            ClearLabels();
            ClearImages();
            //OpenFileDialog();
            dialogResult = fileDialogService.OpenDialog(opDlg, dialogResult);

            if (dialogResult == DialogResult.OK)
            {
                OrgFilePath = fileDialogService.GetSelectedFilePath(opDlg);
                OrgImage = new BitmapImage(new Uri(OrgFilePath, UriKind.RelativeOrAbsolute));
                OrgImageLabel = "Original image:";
            }
        }

        public void OpenFileDialog()
        {
            //OrgFilePath = fileDialogService.OpenDialog(opDlg);
        }

        public void ClearLabels()
        {
            ConvertingTimeLabel = null;
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

        public void Convert()
        {

            if (OrgImage != null && OrgFilePath != "error")
            {
                start = Environment.TickCount & Int32.MaxValue;

                OrgByteArray = imgProc.ImageToByteArray(OrgImage);

                imgProc.ConvertArray(OrgByteArray);

                NewImage = imgProc.ToMainColors(OrgByteArray, OrgImage.PixelWidth, OrgImage.PixelHeight);

                stop = Environment.TickCount & Int32.MaxValue;
                NewImageLabel = "Converted image:";
                ConvertingTimeLabel = "Converting time: " + ProcTime(start, stop) + " ms";
            }
            else
                ErrorLabel = "Error: You must (wczytaj obraz)"; // todo enable convert button if image is't loaded

        }

        public void SaveAs()
        {
            if (NewImage != null)
                fileDialogService.SaveDialog(NewImage, OrgFilePath);
        }
    }
}
