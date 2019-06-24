using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CliNamespace;

namespace MyImageLib
{
    public class ImageProcessing : IProcessing
    {

        public async Task<WriteableBitmap> CreateNewConvertedImage(BitmapImage loadImage, ConvertMode convertMode)
        {

            int height = loadImage.PixelHeight;
            int width = loadImage.PixelWidth;
            int nStride = loadImage.PixelWidth * 4;
            byte[] loadImageByteArr;
            WriteableBitmap writeImg;
            Int32Rect rect;
            var dpix = loadImage.DpiX;
            var dpiy = loadImage.DpiY;

            loadImageByteArr = ConvertImageToByteArray(loadImage);
            loadImageByteArr = await ConvertWithSelectedMode(convertMode, loadImageByteArr);
            writeImg = CreateNewEmptyImage(width, height, out rect, dpix, dpiy);
            writeImg.WritePixels(rect, loadImageByteArr, nStride, 0);
            return writeImg;
        }

        public byte[] ConvertImageToByteArray(BitmapImage loadImg)
        {
            int height = loadImg.PixelHeight;
            int width = loadImg.PixelWidth;
            int nStride = (loadImg.PixelWidth * 4);

            byte[] pixelOrgArr = new byte[loadImg.PixelHeight * nStride];
            loadImg.CopyPixels(pixelOrgArr, nStride, 0);
            return pixelOrgArr;
        }

        private async Task<byte[]> ConvertWithSelectedMode(ConvertMode convertMode, byte[] loadImageByteArr)
        {
            ManagedClass mc = new ManagedClass();
            if (convertMode == ConvertMode.Normally)
                loadImageByteArr = ToMainColors(loadImageByteArr);
            if (convertMode == ConvertMode.Asynchronously)
                loadImageByteArr = await ToMainColorsAsync(loadImageByteArr);
            if (convertMode == ConvertMode.UsingCpp)
                mc.ToMainColorsCPP(loadImageByteArr);
            return loadImageByteArr;
        }

        public WriteableBitmap CreateNewEmptyImage(int width, int height, out Int32Rect rect, double a, double b)
        {
            WriteableBitmap emptyImg = new WriteableBitmap(width, height, a, b, PixelFormats.Pbgra32, null);
            rect = new Int32Rect(0, 0, width, height);
            return emptyImg;
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

        public byte[] ConvertCpp(byte[] OrgArr)
        {
            ManagedClass mc = new CliNamespace.ManagedClass();
            mc.ToMainColorsCPP(OrgArr);
            return OrgArr;
        }

        public byte[] ToMainColors(byte[] OrgArr)
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

        public async Task<byte[]> ConvertAsync(byte[] orgArr)
        {
            return await Task.Run(() =>
            {
                int i = 0;
                while (i < orgArr.Length)
                {
                    int B = orgArr[i];
                    int G = orgArr[i + 1];
                    int R = orgArr[i + 2];
                    orgArr[i + 3] = 255; // 4th byte is always 255

                    if (B > G & B > R)
                    {
                        orgArr[i] = 255;
                        orgArr[i + 1] = 0;
                        orgArr[i + 2] = 0;
                    }
                    else if (G > B & G > R)
                    {
                        orgArr[i] = 0;
                        orgArr[i + 1] = 255;
                        orgArr[i + 2] = 0;
                    }
                    else if (R > B & R > G)
                    {
                        orgArr[i] = 0;
                        orgArr[i + 1] = 0;
                        orgArr[i + 2] = 255;
                    }
                    i += 4;
                }
                return orgArr;
            });
        }

        public async Task<byte[]> ToMainColorsAsync(byte[] orgArr)
        {
            int taskCount = 4;
            int orgLength = orgArr.Length;

            int lenght1 = orgLength / 4;
            int lenght2 = lenght1 * 2;
            int lenght3 = lenght1 * 3;
            int lenght4 = lenght1 * 4;

            //List<Task<byte[]>> tasks = new List<Task<byte[]>>();

            //tasks.Add(Task.Run(() => ConvertShort(orgArr, 0, lenght1)));
            //tasks.Add(Task.Run(() => ConvertShort(orgArr, lenght1, lenght2)));
            //tasks.Add(Task.Run(() => ConvertShort(orgArr, lenght2, lenght3)));
            //tasks.Add(Task.Run(() => ConvertShort(orgArr, lenght3, lenght4)));
            //var results = await Task.WhenAll(tasks);

            //byte[] arr1 = results[0];
            //byte[] arr2 = results[1];
            //byte[] arr3 = results[2];
            //byte[] arr4 = results[3];

            int truncLength = orgLength / 4;

            Task<byte[]>[] tasks = new Task<byte[]>[taskCount];

            for (int i = 0; i < taskCount; i++)
            {
                int startIndex = i * truncLength;
                int length = (i + 1) * truncLength;

                tasks[i] = Task.Run(() => ConvertShort(orgArr, startIndex, length));
            }

            var results = await Task.WhenAll(tasks);


            byte[] arr1 = results[0];
            byte[] arr2 = results[1];
            byte[] arr3 = results[2];
            byte[] arr4 = results[3];

            var z = new byte[arr1.Length + arr2.Length + arr3.Length + arr4.Length];
            arr1.CopyTo(z, 0);
            arr2.CopyTo(z, arr1.Length);
            arr3.CopyTo(z, arr2.Length);
            arr4.CopyTo(z, arr3.Length);
            return z;



            //arr1 = ConvertShort1(orgArr);
            // arr2 = ConvertShort2(orgArr);


            //byte[] arr1 = await ConvertShort(orgArr.Take(length / 3).ToArray());
            //byte[] arr2 = await ConvertShort(orgArr.Skip(length / 3).ToArray());
            //byte[] arr3 = await ConvertShort(orgArr.Skip(skip3).ToArray());
            //return arr1.Concat(arr2).Concat(arr3).ToArray();
            //return arr1.Concat(arr2).ToArray();


        }

        public byte[] ConvertShort(byte[] orgArr, int start, int stop)
        {
            int i = start;
            while (i < stop)
            {
                int B = orgArr[i];
                int G = orgArr[i + 1];
                int R = orgArr[i + 2];
                orgArr[i + 3] = 255; // 4th byte is always 255

                if (B > G & B > R)
                {
                    orgArr[i] = 255;
                    orgArr[i + 1] = 0;
                    orgArr[i + 2] = 0;
                }
                else if (G > B & G > R)
                {
                    orgArr[i] = 0;
                    orgArr[i + 1] = 255;
                    orgArr[i + 2] = 0;
                }
                else if (R > B & R > G)
                {
                    orgArr[i] = 0;
                    orgArr[i + 1] = 0;
                    orgArr[i + 2] = 255;
                }
                i += 4;
            }
            return orgArr;
        }
    }
}
