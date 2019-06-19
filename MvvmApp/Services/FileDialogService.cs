using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MvvmApp.Services
{
    public class FileDialogService
    {
        public bool TryOpenDialog(out string filePath)
        {
            filePath = null;
            OpenFileDialog opDlg = new OpenFileDialog();
            opDlg.Title = "Open Image";
            opDlg.Filter = "Bmp files (*.bmp)|*.bmp|Png files (*.png)|*.png|Image files (*.jpg)|*.jpg";
            opDlg.RestoreDirectory = true;
            bool success = opDlg.ShowDialog() == DialogResult.OK;
            if (success)
                filePath = opDlg.FileName;
            return success;
        }

        public void SaveDialog(ImageSource image, string filePath)
        {
            SaveFileDialog saDlg = new SaveFileDialog();
            string imageType = filePath.Substring(filePath.LastIndexOf('.'));
            saDlg.Title = "Save Image";
            saDlg.RestoreDirectory = true;
            SaveAs(image, saDlg, imageType);
        }

        public void SaveAs(ImageSource image, SaveFileDialog saDlg, string imageType)
        {
            saDlg.Filter = SetFileTypeFilter(imageType);
            if (saDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            var encoder = EncoderFactory(imageType);
            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)image));
            using (FileStream stream = new FileStream(saDlg.FileName, FileMode.Create))
                encoder.Save(stream);
        }

        public string SetFileTypeFilter(string imageType)
        {
            if (imageType == ".bmp")
                return "Bmp files (*.bmp)|*.bmp";
            else if (imageType == ".jpg")
                return "Image files (*.jpg)|*.jpg";
            else if (imageType == ".png")
                return "Png files (*.png)|*.png";
            else return null;
        }

        public BitmapEncoder EncoderFactory(string fileType)
        {
            if (fileType == ".bmp")
                return new BmpBitmapEncoder();
            else if (fileType == ".jpg")
                return new JpegBitmapEncoder();
            else if (fileType == ".png")
                return new PngBitmapEncoder();
            else return null;
        }
    }
}
