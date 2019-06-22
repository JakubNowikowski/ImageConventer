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
        Task<WriteableBitmap> CreateNewConvertedImage(BitmapImage loadImage, byte[] array);
        Task<byte[]> ConvertAsync(byte[] orgArr);
        byte[] ConvertImageToByteArray(BitmapImage loadImg);
        byte[] ConvertCpp(byte[] OrgArr);
        byte[] ConvertSync(byte[] OrgArr);
        Task<byte[]> ConvertAsync2(byte[] orgArr);
    }
}
