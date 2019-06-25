using MvvmApp.ViewModels;
using NUnit.Framework;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Moq;
using MyImageLib;
using System.Threading.Tasks;
using MvvmApp.Services;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;



namespace AppTests
{
    [TestFixture]
    public class ConvertMethodTests
    {
        Presenter presenter;
        BitmapImage loadImage;
        WriteableBitmap newImage;
        Mock<IFileService> fileServiceMock;
        Mock<IProcessing> processingMock;

        #region Helpers

        private BitmapImage CreateBitmapImage()
        {
            Bitmap bitmap = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(System.Drawing.Color.Black);

            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
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
            fileServiceMock = new Mock<IFileService>();
            processingMock = new Mock<IProcessing>();
            presenter = new Presenter(fileServiceMock.Object, processingMock.Object);
            newImage = new WriteableBitmap(1, 1, 96, 96, PixelFormats.Bgr32, null);
            loadImage = CreateBitmapImage();
            processingMock.Setup(m => m.CreateNewConvertedImage(presenter.OrgImage, presenter.SelectedConvertOption)).Returns(Task.FromResult(newImage));
        }

        [Test]
        public async Task Convert_ShowLabels_NewImageLabelChanged()
        {
            presenter.NewImageLabel = null;

            await presenter.Convert();
            //presenter.Convert().GetAwaiter().GetResult();

            Assert.AreEqual(presenter.convertedImageDescription, presenter.NewImageLabel);
        }

        [Test]
        public async Task Convert_ShowLabels_ConvertingTimeLabelChanged()
        {
            presenter.ConvertingTimeLabel = null;

            await presenter.Convert();

            Assert.That(presenter.ConvertingTimeLabel.Contains(presenter.convertedTimeDescription));
        }

        [Test]
        public async Task Convert_IsSaveEnabledChanged()
        {
            presenter.IsSaveEnabled = false;

            await presenter.Convert();

            Assert.That(presenter.IsSaveEnabled);
        }
    }
}
