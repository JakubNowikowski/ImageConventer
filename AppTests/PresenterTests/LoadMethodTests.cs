﻿using System;
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
        Presenter presenter;
        string filePath;
        Mock<IFileService> fileServiceMock;
        Mock<IProcessing> processingMock;

        [SetUp]
        public void SetUp()
        {
            fileServiceMock = new Mock<IFileService>();
            processingMock = new Mock<IProcessing>();
            presenter = new Presenter(fileServiceMock.Object,processingMock.Object);
            filePath = "some Path";
            fileServiceMock.Setup(m => m.TryOpenDialog(out filePath)).Returns(true);
        }

        [Test]
        public void Load_ConvertingTimeLabelChanged()
        {
            presenter.ConvertingTimeLabel = "some text";

            presenter.Load();

            Assert.IsNull(presenter.ConvertingTimeLabel);
        }

        [Test]
        public void Load_NewImageLabelChanged()
        {
            presenter.NewImageLabel = "some text";

            presenter.Load();

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
        public void Load_OpenImage_FilePathIsNull_ThrowsNullException()
        {
            filePath = null;
            fileServiceMock.Setup(m => m.TryOpenDialog(out filePath)).Returns(true);

            Assert.Throws<ArgumentNullException>(() => presenter.Load());
        }

        [Test]////
        public void Load_OpenImage_FilePathIsEmpty_ThrowsNullException()
        {
            filePath = "";
            fileServiceMock.Setup(m => m.TryOpenDialog(out filePath)).Returns(true);

            Assert.Throws<ArgumentNullException>(() => presenter.Load());
        }

        [Test]////
        public void Load_IsConvertedEnabledChangedToFalse()
        {
            presenter.IsConvertEnabled = true;
            fileServiceMock.Setup(m => m.TryOpenDialog(out filePath)).Returns(false);

            presenter.Load();

            Assert.AreEqual(false, presenter.IsConvertEnabled);
        }

        [Test]////
        public void Load_IsSaveEnabledChangedToFalse()
        {
            presenter.IsSaveEnabled = true;
            fileServiceMock.Setup(m => m.TryOpenDialog(out filePath)).Returns(false);

            presenter.Load();

            Assert.AreEqual(false, presenter.IsSaveEnabled);///////
            Assert.That(presenter.IsSaveEnabled, Is.False);
        }
    }
}
