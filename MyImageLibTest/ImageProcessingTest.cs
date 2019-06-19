//using System;
//using MyImageLib;
//using NUnit.Framework;
////using System.Collections.Generic;
////using System.Windows.Media;
////using System.Windows.Media.Imaging;
//using System.Windows.Input;
//using System.Drawing;

//namespace MyImageLibTests
//{
//	[TestFixture]
//	public class ImageProcessingTest
//	{
//		ImageProcessing imgProc;
//		byte[] Arr = new byte[4];


//		[SetUp]
//		public void SetUp()
//		{
//			imgProc = new ImageProcessing();
//		}

//		[Test]
//		public void ConvertArray_StandardInput()
//		{
//			Arr[0] = 2;
//			Arr[1] = 0;
//			Arr[2] = 0;
//			Arr[3] = 0;

//			imgProc.ToMainColors(Arr);

//			Assert.AreEqual(Arr[0], 255);
//		}

//		[Test]
//		public void ConvertArray_Zero()
//		{
//			Arr[0] = 0;
//			Arr[1] = 0;
//			Arr[2] = 0;
//			Arr[3] = 0;

//			imgProc.ToMainColors(Arr);

//			Assert.That(Arr, Has.Exactly(4).EqualTo(0));
//		}

//		[Test]
//		public void ConvertArray_MaxValues_255()
//		{
//			Arr[0] = 255;
//			Arr[1] = 255;
//			Arr[2] = 255;
//			Arr[3] = 255;

//			imgProc.ToMainColors(Arr);

//			Assert.That(Arr, Has.Exactly(4).EqualTo(255));
//		}

//		//[Test]
//		//public void ToMainColors_StandardInput()
//		//{

//		//	ImageSource img;
//		//	Arr[0] = 2;
//		//	Arr[1] = 0;
//		//	Arr[2] = 0;
//		//	Arr[3] = 0;

//		//	img = null;
//		//	img = imgProc.ToMainColors(Arr, 1, 1);
//		//	Assert.Null(img);
//		//}

//	}
//}
