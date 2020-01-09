using System;
using MvvmApp.ViewModels;
using NUnit.Framework;
using System.Windows.Media.Imaging;
using Moq;
using MvvmApp.Services;
using MyImageLib;

namespace AppTests
{
    [TestFixture]
    public class LoadMethodTests
    {
        #region Fields

        MainWindowViewModel mainWindowViewModel;
        string filePath;
        Mock<IFileService> fileServiceMock;
        Mock<IProcessing> processingMock;

        #endregion

        #region SetUp

        [SetUp]
        public void SetUp()
        {
            fileServiceMock = new Mock<IFileService>();
            processingMock = new Mock<IProcessing>();
            mainWindowViewModel = new MainWindowViewModel(fileServiceMock.Object, processingMock.Object);
            filePath = "some Path";
            fileServiceMock.Setup(m => m.TryOpenDialog(out filePath)).Returns(true);
        }

        #endregion

        #region Tests

        [Test]
        public void Load_ConvertingTimeLabelChanged()
        {
            mainWindowViewModel.ConvertingTimeLabel = "some text";

            mainWindowViewModel.Load();

            Assert.IsNull(mainWindowViewModel.ConvertingTimeLabel);
        }

        [Test]
        public void Load_NewImageLabelChanged()
        {
            mainWindowViewModel.NewImageLabel = "some text";

            mainWindowViewModel.Load();

            Assert.IsNull(mainWindowViewModel.NewImageLabel);
        }

        [Test]
        public void Load_OrgFilePathLabelChanged()
        {
            mainWindowViewModel.OrgFilePath = null;

            mainWindowViewModel.Load();

            Assert.AreEqual(filePath, mainWindowViewModel.OrgFilePath);
        }

        [Test]
        public void Load_OpenImage_OrgImageCreated()
        {
            mainWindowViewModel.OrgImage = new BitmapImage();

            mainWindowViewModel.Load();

            Assert.IsNotNull(mainWindowViewModel.OrgImage);
        }

        [Test]
        public void Load_OpenImage_OrgImageLabelChanged()
        {
            mainWindowViewModel.OrgImageLabel = "some text";

            mainWindowViewModel.Load();

            Assert.AreEqual(mainWindowViewModel.originalImageDescription, mainWindowViewModel.OrgImageLabel);
        }

        [Test]
        public void Load_OpenImage_FilePathIsNull_ThrowsNullException()
        {
            filePath = null;
            fileServiceMock.Setup(m => m.TryOpenDialog(out filePath)).Returns(true);

            Assert.Throws<ArgumentNullException>(() => mainWindowViewModel.Load());
        }

        [Test]
        public void Load_OpenImage_FilePathIsEmpty_ThrowsNullException()
        {
            filePath = "";
            fileServiceMock.Setup(m => m.TryOpenDialog(out filePath)).Returns(true);

            Assert.Throws<ArgumentNullException>(() => mainWindowViewModel.Load());
        }

        [Test]
        public void Load_IsConvertedEnabledChangedToFalse()
        {
            mainWindowViewModel.IsConvertEnabled = true;
            fileServiceMock.Setup(m => m.TryOpenDialog(out filePath)).Returns(false);

            mainWindowViewModel.Load();

            Assert.AreEqual(false, mainWindowViewModel.IsConvertEnabled);
        }

        [Test]
        public void Load_IsSaveEnabledChangedToFalse()
        {
            mainWindowViewModel.IsSaveEnabled = true;
            fileServiceMock.Setup(m => m.TryOpenDialog(out filePath)).Returns(false);

            mainWindowViewModel.Load();

            Assert.That(mainWindowViewModel.IsSaveEnabled, Is.False);
        }

        #endregion
    }
}
