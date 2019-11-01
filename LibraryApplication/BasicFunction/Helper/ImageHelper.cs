using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace BasicFunction.Helper
{
    public class ImageHelper
    {
        private static ImageHelper _imageHelper;
        public static ImageHelper Instance => _imageHelper ?? (_imageHelper = new ImageHelper());
        public  BitmapImage ToImage(byte[] byteArray)
        {
            BitmapImage bmp = null;
            try
            {
                bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = new MemoryStream(byteArray); bmp.EndInit();
            }
            catch
            {
                bmp = null;
            }
            return bmp;
        }

        /// <summary>
        /// bitmap转化为BitmapImage
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
        {
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }

        public  Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new System.IO.MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                Bitmap bitmap = new Bitmap(outStream);
                return bitmap;
            }
        }

    }
}