using CefSharp;
using CefsharpScreenshot.Views;
using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Threading;
using System.Windows;

namespace CefsharpScreenshot
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

    
    }
}
