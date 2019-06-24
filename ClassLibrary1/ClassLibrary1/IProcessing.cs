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
        Task<WriteableBitmap> CreateNewConvertedImage(BitmapImage loadImage, ConvertMode convertMode);
        Task<byte[]> ConvertAsync(byte[] orgArr);
        byte[] ConvertImageToByteArray(BitmapImage loadImg);
        byte[] ConvertCpp(byte[] OrgArr);
        byte[] ToMainColors(byte[] OrgArr);
        Task<byte[]> ToMainColorsAsync(byte[] orgArr);
    }
}
