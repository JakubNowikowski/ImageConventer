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
            //presenter.NewImage = new WriteableBitmap(1, 1, 96, 96, PixelFormats.Bgr32, null);
        }

        [Test]
        public void ShowLabels_ProperResultTime()
        {
            //Arrange
            presenter.NewImageLabel = null;
            presenter.ConvertingTimeLabel = null;
            int resultTime = 10;
            var result1 = "Converted image:";
            var result2 = "Converting time: " + resultTime + " ms";

            //Act
            presenter.ShowLabels(resultTime);

            //Assert
            Assert.AreEqual(result1, presenter.NewImageLabel);
            Assert.AreEqual(result2, presenter.ConvertingTimeLabel);
        }

        [Ignore("")]
        [Test]
        public void ClearImagesTest()
        {
            //Arrange
            presenter.OrgImage = new BitmapImage();
            presenter.NewImage = new WriteableBitmap(100, 100, 96, 96, PixelFormats.Bgr32, null);

            //Act
            presenter.ClearImages();

            //Assert
            Assert.IsNull(presenter.OrgImage);
            Assert.AreEqual(null, presenter.NewImage);
        }

        [Test]
        public void ClearLabelsTest()
        {
            //Arrange
            presenter.ConvertingTimeLabel = "some text";
            presenter.NewImageLabel = "some text";
            presenter.OrgImageLabel = "some text";

            //Act
            presenter.ClearLabels();

            //Assert
            Assert.IsNull(presenter.ConvertingTimeLabel);
            Assert.IsNull(presenter.NewImageLabel);
            Assert.IsNull(presenter.OrgImageLabel);
        }


    }
}
