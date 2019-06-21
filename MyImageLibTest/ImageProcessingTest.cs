using MyImageLib;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MyImageLibTests
{
    [TestFixture]
    public class ImageProcessingTest
    {
        ImageProcessing imgProc;
        byte[] arr;

        ref byte blue => ref arr[0];
        ref byte green => ref arr[1];
        ref byte red => ref arr[2];

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

        [Ignore("")]
        [Test]
        public void ConvertImageToByteArray_ProperInput()
        {
            BitmapImage image = new BitmapImage();

            image.BeginInit();
            image.DecodePixelHeight = 5;
            image.DecodePixelWidth = 5;
            image.EndInit();

            imgProc.ConvertImageToByteArray(image);

            Assert.AreEqual(4, image.Height);
        }

        #endregion
    }
}
