using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MyImageLib
{
    public interface IProcessing
    {
        #region Methods

        Task<WriteableBitmap> CreateNewConvertedImage(BitmapImage loadImage, ConvertMode convertMode);
        Task<byte[]> ConvertAsync(byte[] orgArr);
        byte[] ConvertImageToByteArray(BitmapImage loadImg);
        void ConvertCpp(byte[] OrgArr);
        void ToMainColors(byte[] OrgArr);
        Task ToMainColorsAsync(byte[] orgArr);

        #endregion
    }
}
