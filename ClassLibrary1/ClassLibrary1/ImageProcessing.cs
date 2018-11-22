using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Forms;

namespace MyImageLib
{
	public class ImageProcessing
	{

		//public string OpenDialog()
		//{
		//	OpenFileDialog opDlg = new OpenFileDialog();

		//	opDlg.Title = "Open Image";

		//	opDlg.Filter = "Bmp files (*.bmp)|*.bmp|Png files (*.png)|*.png|Image files (*.jpg)|*.jpg";

		//	opDlg.RestoreDirectory = true;

		//	if (opDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
		//	{
		//		return opDlg.FileName;
		//	}
		//	return "error";
		//}

		public virtual bool OpenDialog()
		{
			string path;

			OpenFileDialog opDlg = new OpenFileDialog();

			opDlg.Title = "Open Image";

			opDlg.Filter = "Bmp files (*.bmp)|*.bmp|Png files (*.png)|*.png|Image files (*.jpg)|*.jpg";

			opDlg.RestoreDirectory = true;

			if (opDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				path = opDlg.FileName;
			}

			return true;
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

		public WriteableBitmap ToMainColors(byte[] pixelOrgArr, int imageWidth, int imageHeight)
		{
			

			int nStride = imageWidth * 4;
			pixelOrgArr = ConvertArray(pixelOrgArr);

			WriteableBitmap writeImg = new WriteableBitmap(imageWidth, imageHeight, 96, 96, PixelFormats.Bgr32, null);
			
			Int32Rect rect = new Int32Rect(0, 0, imageWidth, imageHeight);
			writeImg.WritePixels(rect, pixelOrgArr, nStride, 0);

			return writeImg;
		}

		public byte[] ImageToByteArray(BitmapImage loadImg)
		{
			int height = loadImg.PixelHeight;
			int width = loadImg.PixelWidth;

			int nStride = (loadImg.PixelWidth * 4);

			byte[] pixelOrgArr = new byte[loadImg.PixelHeight * nStride];
			loadImg.CopyPixels(pixelOrgArr, nStride, 0);

			return pixelOrgArr;
		}

		public byte[] ConvertArray(byte[] OrgArr)
		{
			int i = 0;

			while (i < OrgArr.Length)
			{
				int B = OrgArr[i];
				int G = OrgArr[i + 1];
				int R = OrgArr[i + 2];

				if (B > G & B > R)
				{
					OrgArr[i] = 255;
					OrgArr[i + 1] = 0;
					OrgArr[i + 2] = 0;
					OrgArr[i + 3] = 255;
				}
				else if (G > B & G > R)
				{
					OrgArr[i] = 0;
					OrgArr[i + 1] = 255;
					OrgArr[i + 2] = 0;
					OrgArr[i + 3] = 255;
				}
				else if (R > B & R > G)
				{
					OrgArr[i] = 0;
					OrgArr[i + 1] = 0;
					OrgArr[i + 2] = 255;
					OrgArr[i + 3] = 255;
				}

				i += 4;
			}

			return OrgArr;
		}

	}
}
