using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MvvmApp.Services
{
    public class FileDialogService
    {
        public DialogResult OpenDialog(OpenFileDialog opDlg, DialogResult result)
        {
            opDlg.Title = "Open Image";

            opDlg.Filter = "Bmp files (*.bmp)|*.bmp|Png files (*.png)|*.png|Image files (*.jpg)|*.jpg";

            opDlg.RestoreDirectory = true;

            result = opDlg.ShowDialog();

            return result;

        }

        public string GetSelectedFilePath(OpenFileDialog opDlg)
        {
            return opDlg.FileName;
        }


        public void SaveDialog(ImageSource imageWr, string FilePath)
        {
            SaveFileDialog saDlg = new SaveFileDialog();

            saDlg.Title = "Save Image";

            saDlg.RestoreDirectory = true;

            if (FilePath.Substring(Math.Max(0, FilePath.Length - 3)) == "bmp")
            {
                saDlg.Filter = "Bmp files (*.bmp)|*.bmp";

                if (saDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var bmpEncoder = new BmpBitmapEncoder();
                    bmpEncoder.Frames.Add(BitmapFrame.Create((BitmapSource)imageWr));
                    using (FileStream stream = new FileStream(saDlg.FileName, FileMode.Create))
                        bmpEncoder.Save(stream);
                }
            }
            else if (FilePath.Substring(Math.Max(0, FilePath.Length - 3)) == "jpg")
            {
                saDlg.Filter = "Image files (*.jpg)|*.jpg";
                if (saDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var jpgEncoder = new JpegBitmapEncoder();
                    jpgEncoder.Frames.Add(BitmapFrame.Create((BitmapSource)imageWr));
                    using (FileStream stream = new FileStream(saDlg.FileName, FileMode.Create))
                        jpgEncoder.Save(stream);
                }
            }
            else
            {
                saDlg.Filter = "Png files (*.png)|*.png";
                if (saDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create((BitmapSource)imageWr));
                    using (FileStream stream = new FileStream(saDlg.FileName, FileMode.Create))
                        encoder.Save(stream);
                }
            }
        }
    }
}
