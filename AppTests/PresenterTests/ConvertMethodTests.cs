using MvvmApp.ViewModels;
using NUnit.Framework;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Moq;
using MyImageLib;
using System.Threading.Tasks;
using MvvmApp.Services;

namespace AppTests
{
    [TestFixture]
    public class ConvertMethodTests
    {
        Presenter presenter;
        WriteableBitmap newImage;
        Mock<IFileService> fileServiceMock;
        Mock<IProcessing> processingMock;

        [SetUp]
        public void SetUp()
        {
            fileServiceMock = new Mock<IFileService>();
            processingMock = new Mock<IProcessing>();
            presenter = new Presenter(fileServiceMock.Object, processingMock.Object);
            newImage = new WriteableBitmap(1, 1, 96, 96, PixelFormats.Bgr32, null);
            presenter.OrgImage =  new BitmapImage();
            processingMock.Setup(m => m.CreateNewConvertedImage(presenter.OrgImage)).Returns(Task.FromResult(newImage));
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
