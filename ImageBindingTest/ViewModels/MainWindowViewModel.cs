using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageBindingTest.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }

        private ImageSource _Thumbnail;
        public ImageSource Thumbnail
        {
            get { return _Thumbnail; }
            set { SetProperty(ref _Thumbnail, value); }
        }

        private DelegateCommand _PaintingCommand;
        public DelegateCommand PaintingCommand =>
            _PaintingCommand ?? (_PaintingCommand = new DelegateCommand(ExecutePaintingCommand));

        void ExecutePaintingCommand()
        {
            SaveImage();
        }

        void SaveImage()
        {
            var tempPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Tal\\WhiteBoard3.0";
            var dirX = tempPath + "\\output";
            var path = dirX + "\\" + Guid.NewGuid().ToString("N") + ".jpg";
            FrameworkElement source = Application.Current.MainWindow;
            var memoryStream = new MemoryStream();
            Application.Current.Dispatcher.Invoke(() =>
            {
                //创建承载DrawingVisual的Bitmap，将白板画面绘制进去
                RenderTargetBitmap renderTarget = new RenderTargetBitmap((int)source.ActualWidth, (int)source.ActualHeight, 96, 96, PixelFormats.Pbgra32);
                renderTarget.Render(source);


                //准备输出图片文件
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderTarget));
                encoder.Save(memoryStream);

                memoryStream.Seek(0, SeekOrigin.Begin);
                System.Drawing.Image img = System.Drawing.Image.FromStream(memoryStream);


                var randomStr = Guid.NewGuid().ToString("N");
                var dir = tempPath + "\\output";
                var pathX = dir + "\\" + randomStr + ".jpg";
                img.Save(pathX);
                Thumbnail = new BitmapImage(new Uri(pathX, UriKind.Absolute));


                memoryStream.Close();
                img.Dispose();

            });



        }
    }
}
