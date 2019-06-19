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
	public class ConvertMethodTests
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

		
    }
}
