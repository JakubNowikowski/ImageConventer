using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace MyImageLib
{
    public class ImageProcessing
    {
        public async Task<WriteableBitmap> CreateNewConvertedImage(BitmapImage loadImage)
        {
            int height = loadImage.PixelHeight;
            int width = loadImage.PixelWidth;
            int nStride = loadImage.PixelWidth * 4;
            byte[] loadImageByteArr;
            byte[] newImageByteArr;
            WriteableBitmap writeImg;
            Int32Rect rect;
			var dpix = loadImage.DpiX;
			var dpiy = loadImage.DpiY;
            loadImageByteArr = ConvertImageToByteArray(loadImage);
            writeImg = CreateNewEmptyImage(width, height, out rect, dpix, dpiy);
            writeImg.WritePixels(rect, await ConvertAsync(loadImageByteArr), nStride, 0);
            return writeImg;
        }

        public WriteableBitmap CreateNewEmptyImage(int width, int height, out Int32Rect rect, double a, double b)
        {
            WriteableBitmap emptyImg = new WriteableBitmap(width, height, a, b, PixelFormats.Pbgra32, null);
            rect = new Int32Rect(0, 0, width, height);
            return emptyImg;
        }

        //public WriteableBitmap ToMainColorsOldVer(byte[] pixelOrgArr, int imageWidth, int imageHeight)
        //{
        //    int nStride = imageWidth * 4;
        //    pixelOrgArr = ConvertAsync(pixelOrgArr);
        //    WriteableBitmap writeImg = new WriteableBitmap(imageWidth, imageHeight, 96, 96, PixelFormats.Bgr32, null);
        //    Int32Rect rect = new Int32Rect(0, 0, imageWidth, imageHeight);
        //    writeImg.WritePixels(rect, pixelOrgArr, nStride, 0);
        //    return writeImg;
        //}

        public byte[] ConvertImageToByteArray(BitmapImage loadImg)
        {
            int height = loadImg.PixelHeight;
            int width = loadImg.PixelWidth;
            int nStride = (loadImg.PixelWidth * 4);

            byte[] pixelOrgArr = new byte[loadImg.PixelHeight * nStride];
            loadImg.CopyPixels(pixelOrgArr, nStride, 0);
            return pixelOrgArr;
        }

		//public byte[] ToMainColors(byte[] OrgArr)
		//{
		//    int i = 0;

		//    while (i < OrgArr.Length)
		//    {
		//        int B = OrgArr[i];
		//        int G = OrgArr[i + 1];
		//        int R = OrgArr[i + 2];

		//        if (B > G & B > R)
		//        {
		//            OrgArr[i] = 255;
		//            OrgArr[i + 1] = 0;
		//            OrgArr[i + 2] = 0;
		//            OrgArr[i + 3] = 255;
		//        }
		//        else if (G > B & G > R)
		//        {
		//            OrgArr[i] = 0;
		//            OrgArr[i + 1] = 255;
		//            OrgArr[i + 2] = 0;
		//            OrgArr[i + 3] = 255;
		//        }
		//        else if (R > B & R > G)
		//        {
		//            OrgArr[i] = 0;
		//            OrgArr[i + 1] = 0;
		//            OrgArr[i + 2] = 255;
		//            OrgArr[i + 3] = 255;
		//        }
		//        i += 4;
		//    }
		//    return OrgArr;
		//}

		public async Task<byte[]> ConvertAsync(byte[] OrgArr) // assume we return an int from this long running operation 
		{
			return await Task.Run(() =>
			{
				int i = 0;
				int lemgth = OrgArr.Length;

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
			});
		}
	}
}
