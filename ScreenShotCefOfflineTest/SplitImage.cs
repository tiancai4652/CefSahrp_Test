using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenShotCefOfflineTest
{
    public class SplitImage
    {
        public int Count { get; set; }
        public int WidthOnePage { get; set; }
        public int HeightOnePage { get; set; }

        public ImageSource ImageSource { get; set; }

        public SplitImage(int count, int widthOnePage, int heightOnePage)
        {
            Count = count;
            WidthOnePage = widthOnePage;
            HeightOnePage = heightOnePage;
        }

        /// <summary>
        /// 裁剪图片
        /// </summary>
        /// <param name="originImage">原图片</param>
        /// <param name="region">裁剪的方形区域</param>
        /// <returns>裁剪后图片</returns>
        public static Image CropImage(Image originImage, Rectangle region)
        {
            Bitmap result = new Bitmap(region.Width, region.Height);
            Graphics graphics = Graphics.FromImage(result);
            graphics.DrawImage(originImage, new Rectangle(0, 0, region.Width, region.Height), region, GraphicsUnit.Pixel);
            return result;

        }

        /// <summary>
        /// Convert Byte[] to Image
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static Image BytesToImage(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            Image image = System.Drawing.Image.FromStream(ms);


          
            return image;
        }

        public List<Image> GetImages(byte[] bytes)
        {
            var image = BytesToImage(bytes);
            List<Image> images = new List<Image>();
            int x = 0;
            int y = 0;
            for (int i = 0; i < Count; i++)
            {
                Image newBitmapSource = CropImage(image,new Rectangle(0, y, WidthOnePage, HeightOnePage));
                images.Add(newBitmapSource);
                y += HeightOnePage;
            }

            return images;
        }

        public List<ImageSource> GetImageSources(byte[] bytes)
        {
            var image = BytesToImage(bytes);
            List<ImageSource> images = new List<ImageSource>();
            int x = 0;
            int y = 0;
            for (int i = 0; i < Count; i++)
            {
                Image newBitmapSource = CropImage(image, new Rectangle(0, y, WidthOnePage, HeightOnePage));
                MemoryStream memoryStream = null;
                newBitmapSource.Save(memoryStream, ImageFormat.Bmp);
                images.Add(ToImageSource(memoryStream));
                memoryStream.Close();
                y += HeightOnePage;
            }

            return images;
        }

        public static ImageSource ToImageSource(MemoryStream stream)
        {
            var bitmap = new Bitmap(stream);
            return Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
