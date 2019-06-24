using Moq;
using MyImageLib;
using NUnit.Framework;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MyImageLibTests
{
    [TestFixture]
    public class ImageProcessingTest
    {
        ImageProcessing imgProc;
        byte[] arr;

        int width = 1;
        int height = 1;
        Int32Rect rect;
        double a = 1;
        double b = 1;
        BitmapImage bitmapImage;
        WriteableBitmap newImage;

        ref byte blue => ref arr[0];
        ref byte green => ref arr[1];
        ref byte red => ref arr[2];

        #region Helpers

        private BitmapImage CreateBitmapImage(Color color, ImageFormat format)
        {
            Bitmap bitmap = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(color);

            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, format);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        #endregion

        [SetUp]
        public void SetUp()
        {
            imgProc = new ImageProcessing();
            arr = new byte[4];
        }

        #region ConvertArray Tests

        [Test]
        public async Task ConvertArray_BlueIsTheBiggest_RetrunMaxBlue()

        {
            blue = 2;
            green = 1;
            red = 1;

            await imgProc.ConvertAsync(arr);

            Assert.AreEqual(255, arr[0]);
        }

        [Test]
        public void ConvertArray_GreenIsTheBiggest_RetrunMaxGreen()
        {
            blue = 1;
            green = 2;
            red = 1;

            var result = imgProc.ConvertAsync(arr).Result;

            Assert.AreEqual(255, arr[1]);
        }

        [Test]
        public async Task ConvertArray_RedIsTheBiggest_RetrunMaxRed()
        {
            blue = 1;
            green = 1;
            red = 2;

            await imgProc.ConvertAsync(arr);

            Assert.AreEqual(255, arr[2]);
        }

        [Test]
        public async Task ConvertArray_4thByteChanged()
        {
            arr[3] = 0;

            await imgProc.ConvertAsync(arr);

            Assert.AreEqual(255, arr[3]);
        }

        [Test]
        public async Task ConvertArray_AllColorValuesAreZero_RetrunThreeZeros()
        {
            blue = 0;
            green = 0;
            red = 0;

            await imgProc.ConvertAsync(arr);

            Assert.That(arr, Has.Exactly(3).EqualTo(0));
        }

        [Test]
        public async Task ConvertArray_AllColorValuesAreMax_RetrunAllMax()
        {
            blue = 255;
            green = 255;
            red = 255;

            await imgProc.ConvertAsync(arr);

            Assert.That(arr, Has.Exactly(4).EqualTo(255));
        }

        #endregion

        #region ConvertImageToByteArray Tests

        [Test]
        public void ConvertImageToByteArray_PngFormat()
        {
            bitmapImage = CreateBitmapImage(Color.AliceBlue, ImageFormat.Png);

            arr = imgProc.ConvertImageToByteArray(bitmapImage);

            Assert.That(arr, Has.Some.GreaterThan(0));
        }

        [Test]
        public void ConvertImageToByteArray_JpegFormat()
        {
            bitmapImage = CreateBitmapImage(Color.AliceBlue, ImageFormat.Jpeg);

            arr = imgProc.ConvertImageToByteArray(bitmapImage);

            Assert.That(arr, Has.Some.GreaterThan(0));
        }

        [Test]
        public void ConvertImageToByteArray_BmpFormat()
        {
            bitmapImage = CreateBitmapImage(Color.AliceBlue, ImageFormat.Bmp);

            arr = imgProc.ConvertImageToByteArray(bitmapImage);

            Assert.That(arr, Has.Some.GreaterThan(0));
        }

        [Test]
        public void ConvertImageToByteArray_WhiteColor_AllElementsAreEqualTo255()
        {
            bitmapImage = CreateBitmapImage(Color.White, ImageFormat.Bmp);

            arr = imgProc.ConvertImageToByteArray(bitmapImage);

            Assert.That(arr, Has.Exactly(4).EqualTo(255));
        }

        [Test]
        public void ConvertImageToByteArray_BlackColor_ThreeElementsAreEqualTo0()
        {
            bitmapImage = CreateBitmapImage(Color.Black, ImageFormat.Bmp);

            arr = imgProc.ConvertImageToByteArray(bitmapImage);

            Assert.That(arr, Has.Exactly(3).EqualTo(0));
        }

        #endregion

        #region CreateNewEmptyImage

        [Test]
        public void CreateNewEmptyImage_ProperInput_NewImageIsNotNull()
        {
            newImage = null;

            newImage = imgProc.CreateNewEmptyImage(width, height, out rect, a, b);

            Assert.That(newImage, Is.Not.Null);
        }

        [Test]
        public void CreateNewEmptyImage_ProperInput_RectIsNotEmpty()
        {
            rect = new Int32Rect();

            newImage = imgProc.CreateNewEmptyImage(width, height, out rect, a, b);

            Assert.That(rect.IsEmpty, Is.False);
        }

        #endregion

        #region CreateNewConvertedImage

        [Ignore("Problem with mocking async method")]
        [Test]
        public async Task CreateNewConvertedImage()
        {
            Mock<IProcessing> processingMock = new Mock<IProcessing>();
            //processingMock.Setup(m => m.CreateNewConvertedImage(presenter.OrgImage)).Returns(Task.FromResult(newImage));
            processingMock.Setup(m => m.ConvertAsync(arr)).Returns(Task.FromResult(arr));

            newImage = null;
            bitmapImage = CreateBitmapImage(Color.AliceBlue, ImageFormat.Bmp);

            newImage = await imgProc.CreateNewConvertedImage(bitmapImage,ConvertMode.Normally);

            Assert.That(newImage, Is.Not.Null);
        }

        #endregion
    }
}
