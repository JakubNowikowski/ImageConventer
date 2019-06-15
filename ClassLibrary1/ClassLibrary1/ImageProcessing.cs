using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace MyImageLib
{
	public class ImageProcessing
	{
        //public bool OpenDialog()
        //{
        //	string path;

        //	OpenFileDialog opDlg = new OpenFileDialog();

        //	opDlg.Title = "Open Image";

        //	opDlg.Filter = "Bmp files (*.bmp)|*.bmp|Png files (*.png)|*.png|Image files (*.jpg)|*.jpg";

        //	opDlg.RestoreDirectory = true;

        //	if (opDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //	{
        //		path = opDlg.FileName;
        //	}

        //	return true;
        //}

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
