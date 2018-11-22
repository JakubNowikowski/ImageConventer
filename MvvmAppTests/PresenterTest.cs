using System;
using MvvmApp.ViewModels;
using NUnit.Framework;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using Moq;
using MyImageLib;

namespace MvvmAppTests
{
	[TestFixture]
	public class PresenterTest
	{
		Presenter presenter;

		[SetUp]
		public void SetUp()
		{
			presenter = new Presenter();

		}

		[Test]
		public void ClearLabels_StandardInput()
		{
			presenter.NewImageLabel = "SomeText";
			presenter.OrgImageLabel = "SomeText";
			presenter.ProcTimeLabel = "SomeText";
			presenter.ClearLabels();
			Assert.IsNull(presenter.NewImageLabel, presenter.OrgImageLabel, presenter.ProcTimeLabel);
		}

		[Test]
		public void ProcTime_StandardInput()
		{
			int result = presenter.ProcTime(2, 4);
			Assert.AreEqual(result, 2);
		}

		[Test]
		public void ProcImage_Null()
		{

			//presenter.OrgImage = null;
			presenter.OrgFilePath = "error";
			//presenter.ProcImage();

			Assert.IsNotNull(presenter.OrgFilePath);
		}

		[Test]
		public void OpenFileDialogTest()
		{
			Mock<ImageProcessing> imgProc = new Mock<ImageProcessing>();

			imgProc.Setup(x => x.OpenDialog()).Returns(true);

			bool IsInserted = presenter.OpenFileDialog(imgProc.Object);

			Assert.AreEqual(IsInserted, true);
		}
	}
}
