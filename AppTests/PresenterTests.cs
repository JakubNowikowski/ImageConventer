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
	public class PresenterTest
	{
		Presenter presenter;
		string filePath;
		Mock<IFileService> fileServiceMock;

	[SetUp]
		public void SetUp()
		{
			presenter = new Presenter();
			//presenter.NewImage = new WriteableBitmap(1, 1, 96, 96, PixelFormats.Bgr32, null);
			fileServiceMock = new Mock<IFileService>();
			presenter.FileServicePropMyProperty = fileServiceMock.Object;
			filePath = "some Path";
			fileServiceMock.Setup(m => m.TryOpenDialog(out filePath)).Returns(true);
		}

		[Test]
		public void Load_LabelsChanged()
		{
			presenter.ConvertingTimeLabel = "some text";
			presenter.NewImageLabel = "some text";
			presenter.OrgImageLabel = "some text";
			
			presenter.Load();

			Assert.IsNull(presenter.ConvertingTimeLabel);
			Assert.IsNull(presenter.NewImageLabel);
		}

		[Test]
		public void Load_OrgFilePathLabelChanged()
		{
			presenter.OrgFilePath = null;
			
			presenter.Load();

			Assert.AreEqual(filePath, presenter.OrgFilePath);
		}

		[Test]
		public void Load_OpenImage_OrgImageCreated()
		{
			presenter.OrgImage = new BitmapImage();
			
			presenter.Load();

			Assert.IsNotNull(presenter.OrgImage);
		}

		[Test]
		public void Load_OpenImage_OrgImageLabelChanged()
		{
			presenter.OrgImageLabel = "some text";
			
			presenter.Load();

			Assert.AreEqual(presenter.originalImageDescription, presenter.OrgImageLabel);
		}

		[Test]
		public void Load_OpenImage_ThrowsNullException()
		{
			filePath = null;
			fileServiceMock.Setup(m => m.TryOpenDialog(out filePath)).Returns(true);

			Assert.Throws<ArgumentNullException>(() => presenter.Load());
		}
	}
}
