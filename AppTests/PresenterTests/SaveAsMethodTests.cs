using System;
using MvvmApp.ViewModels;
using NUnit.Framework;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using Moq;
using MyImageLib;
using MvvmApp.Services;

namespace AppTests
{
	[TestFixture]
	public class SaveAsMethodTests
	{
		Presenter presenter;
		string filePath;
        ImageSource image;
		Mock<IFileService> fileServiceMock;

	[SetUp]
		public void SetUp()
		{
			presenter = new Presenter();
			fileServiceMock = new Mock<IFileService>();
			presenter.FileServicePropMyProperty = fileServiceMock.Object;
            filePath = "some Path";
            image = new WriteableBitmap(1, 1, 96, 96, PixelFormats.Bgr32, null);
            fileServiceMock.Setup(m => m.SaveDialog(image, filePath));
        }

        [Test]
        public void SaveAs_NewImageIsNull_SaveDialogIsNotCalled()
        {
            presenter.NewImage = null;

            presenter.SaveAs();

            fileServiceMock.Verify(m => m.SaveDialog(presenter.NewImage, filePath),Times.Never());
        }

        [Test]
        public void SaveAs_NewImageIsValid_SaveDialogIsCalled()
        {
            presenter.NewImage = new WriteableBitmap(1, 1, 96, 96, PixelFormats.Bgr32, null);

            presenter.SaveAs();

            fileServiceMock.Verify(m => m.SaveDialog(presenter.NewImage, filePath), Times.Once());
        }
    }
}
