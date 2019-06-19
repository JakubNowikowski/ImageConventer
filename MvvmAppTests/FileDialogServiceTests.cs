using Moq;
using MvvmApp.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace MvvmAppTests
{
    [TestFixture]
    public class FileDialogServiceTests
    {
        FileDialogService sut;

        [SetUp]
        public void SetUp()
        {
            sut = new FileDialogService();
        }

        [Test]
        public void EncoderFactory_ProperInput_Bmp()
        {
            //Arrange
            var fileType = ".bmp";

            //Act
            var result = sut.EncoderFactory(fileType);

            //Assert
            //Assert.AreEqual(typeof(BitmapEncoder), result.GetType());
            Assert.IsInstanceOf(typeof(BitmapEncoder), result);
        }

        [Test]
        public void SetFileTypeFilter_ProperInput_Bmp()
        {
            //Arrange
            string imageType = ".bmp";
            string expected = "Bmp files (*.bmp)|*.bmp";

            //Act
            var result = sut.SetFileTypeFilter(imageType);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SetFileTypeFilter_ProperInput_Jpg()
        {
            //Arrange
            string imageType = ".jpg";
            string expected = "Image files (*.jpg)|*.jpg";

            //Act
            var result = sut.SetFileTypeFilter(imageType);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SetFileTypeFilter_ProperInput_Png()
        {
            //Arrange
            string imageType = ".png";
            string expected = "Png files (*.png)|*.png";

            //Act
            var result = sut.SetFileTypeFilter(imageType);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SaveAs_ProperInput()
        {
            //Arrange
            var image = new WriteableBitmap(100, 100, 96, 96, PixelFormats.Bgr32, null);
            var saDlg = new SaveFileDialog();
            var imageType = ".bmp";

            //Act
            sut.SaveAs(image,saDlg,imageType);

            //Assert
        }
    }
}
