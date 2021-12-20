using Mutex_Test.Views;
using Prism.Ioc;
using System;
using System.Threading;
using System.Windows;

namespace Mutex_Test
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            bool createdNew = false
                ;
            using (Mutex mutex = new Mutex(true, "TalWhiteBoard", out createdNew))
            {
                if (createdNew)
                {
                    return Container.Resolve<MainWindow>();
                }
                else
                {
                    MessageBox.Show("当前只允许运行一个白板程序!");
                    Environment.Exit(-2);
                    return null;
                }
            }
          
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
