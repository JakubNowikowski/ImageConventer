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
using System;

namespace AppTests
{
    [TestFixture]
    public class ShowLabelsMethodTests
    {
        MainWindowViewModel mainWindowViewModel;
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
            mainWindowViewModel = new MainWindowViewModel(fileServiceMock.Object, processingMock.Object);
            newImage = new WriteableBitmap(1, 1, 96, 96, PixelFormats.Bgr32, null);
            loadImage = CreateBitmapImage();
            processingMock.Setup(m => m.CreateNewConvertedImage(mainWindowViewModel.OrgImage, mainWindowViewModel.SelectedConvertOption)).Returns(Task.FromResult(newImage));
        }

        [Test]
        public void ShowLabels_NewImageLabelChanged()
        {
            mainWindowViewModel.NewImageLabel = null;

            mainWindowViewModel.ShowLabels(TimeSpan.Zero);

            Assert.AreEqual(mainWindowViewModel.convertedImageDescription, mainWindowViewModel.NewImageLabel);
        }

        [Test]
        public void ShowLabels_ConvertingTimeLabelChanged()
        {
            mainWindowViewModel.ConvertingTimeLabel = null;

            mainWindowViewModel.ShowLabels(TimeSpan.Zero);

            Assert.That(mainWindowViewModel.ConvertingTimeLabel.Contains(mainWindowViewModel.convertedTimeDescription));
        }

        [Test]
        public void ShowLabels_IsSaveEnabledChanged()
        {
            mainWindowViewModel.IsSaveEnabled = false;

            mainWindowViewModel.ShowLabels(TimeSpan.Zero);

            Assert.That(mainWindowViewModel.IsSaveEnabled);
        }
    }
}
