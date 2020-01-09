using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CliNamespace;

namespace MyImageLib
{
    public class ImageProcessing : IProcessing
    {
        #region Image preparation

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
                ToMainColors(loadImageByteArr);
            if (convertMode == ConvertMode.Asynchronously)
                await ToMainColorsAsync(loadImageByteArr);
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

        #endregion

        #region Convert methods

        public void ConvertCpp(byte[] OrgArr)
        {
            ManagedClass mc = new CliNamespace.ManagedClass();
            mc.ToMainColorsCPP(OrgArr);
        }

        public void ToMainColors(byte[] OrgArr)
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

        public async Task ToMainColorsAsync(byte[] input)
        {
            var taskCount = Environment.ProcessorCount;

            var lengthInPixels = input.Length / 4;

            var chunkLength = lengthInPixels / taskCount;
            chunkLength *= 4;
            var lastChunkLength = lengthInPixels % taskCount;
            lastChunkLength *= 4;
            List<Task> tasks = new List<Task>();

            for (int i = 0; i <= taskCount; i++)
            {
                var start = chunkLength * i;
                var end = start + chunkLength;
                if (i == taskCount)
                {
                    end = start + lastChunkLength;
                }

                tasks.Add(Task.Run(() =>
                {
                    ConvertShort(input, start, end);
                }));
            }
            await Task.WhenAll(tasks.ToArray());
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

    #endregion
}
