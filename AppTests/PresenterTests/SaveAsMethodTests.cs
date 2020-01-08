using MvvmApp.ViewModels;
using NUnit.Framework;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Moq;
using MvvmApp.Services;
using MyImageLib;

namespace AppTests
{
    [TestFixture]
    public class SaveAsMethodTests
    {
        MainWindowViewModel mainWindowViewModel;
        string filePath;
        ImageSource image;
        Mock<IFileService> fileServiceMock;
        Mock<IProcessing> processingMock;

        [SetUp]
        public void SetUp()
        {
            fileServiceMock = new Mock<IFileService>();
            processingMock = new Mock<IProcessing>();
            mainWindowViewModel = new MainWindowViewModel(fileServiceMock.Object, processingMock.Object);
            filePath = "some Path";
            image = new WriteableBitmap(1, 1, 96, 96, PixelFormats.Bgr32, null);
            fileServiceMock.Setup(m => m.SaveDialog(image, filePath));
        }

        [Test]
        public void SaveAs_NewImageIsNull_SaveDialogIsNotCalled()
        {
            mainWindowViewModel.NewImage = null;

            mainWindowViewModel.SaveAs();

            fileServiceMock.Verify(m => m.SaveDialog(mainWindowViewModel.NewImage, filePath), Times.Never());
        }

        [Test]
        public void SaveAs_NewImageIsValid_SaveDialogIsCalled()
        {
            mainWindowViewModel.NewImage = image;
            mainWindowViewModel.OrgFilePath = filePath;

            mainWindowViewModel.SaveAs();

            fileServiceMock.Verify(m => m.SaveDialog(mainWindowViewModel.NewImage, mainWindowViewModel.OrgFilePath), Times.Once());
        }
    }
}
