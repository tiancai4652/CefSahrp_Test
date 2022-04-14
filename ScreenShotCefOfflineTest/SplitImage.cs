using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        public List<BitmapSource> GetBitmapSources(byte[] bytes)
        {
            List<BitmapSource> bitmapSources = new List<BitmapSource>();
            BitmapSource bitmapSource = (BitmapSource)new ImageSourceConverter().ConvertFrom(bytes);
            int x = 0;
            int y = 0;
            for (int i = 0; i < Count; i++)
            {
                BitmapSource newBitmapSource = CutImage(bitmapSource, new Int32Rect(0, y, WidthOnePage, HeightOnePage));
                bitmapSources.Add(newBitmapSource);
                y += WidthOnePage;
            }

            return bitmapSources;
        }

       
        public static System.Drawing.Bitmap ImageSourceToBitmap(ImageSource imageSource)
        {
            BitmapSource m = (BitmapSource)imageSource;

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(m.PixelWidth, m.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            System.Drawing.Imaging.BitmapData data = bmp.LockBits(
            new System.Drawing.Rectangle(System.Drawing.Point.Empty, bmp.Size), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            m.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride); bmp.UnlockBits(data);

            return bmp;
        }

        // Bitmap --> BitmapImage
        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Bmp);

                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
                // Force the bitmap to load right now so we can dispose the stream.
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();

                return result;
            }

        }
        /// <summary>
        /// 切图
        /// </summary>
        /// <param name="bitmapSource">图源</param>
        /// <param name="cut">切割区域</param>
        /// <returns></returns>
        public static BitmapSource CutImage(BitmapSource bitmapSource, Int32Rect cut)
        {
            //计算Stride
            var stride = bitmapSource.Format.BitsPerPixel * cut.Width / 8;
            //声明字节数组
            byte[] data = new byte[cut.Height * stride];
            //调用CopyPixels
            bitmapSource.CopyPixels(cut, data, stride, 0);

            return BitmapSource.Create(cut.Width, cut.Height, 0, 0, PixelFormats.Bgr32, null, data, stride);
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
    }
}
