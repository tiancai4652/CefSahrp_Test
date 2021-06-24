using CefSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace CefS_JS_Test.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Browser.RenderProcessMessageHandler = new RenderProcessMessageHandler();
            //Wait for the page to finish loading (all resources will have been loaded, rendering is likely still happening)
            Browser.LoadingStateChanged += (sender, args) =>
            {
                //Wait for the Page to finish loading
                if (args.IsLoading == false)
                {
                    Browser.ExecuteScriptAsync("alert('All Resources Have Loaded');");
                }
            };
            //Wait for the MainFrame to finish loading
            Browser.FrameLoadEnd += (sender, args) =>
            {
                //Wait for the MainFrame to finish loading
                if (args.Frame.IsMain)
                {
                    args.Frame.ExecuteJavaScriptAsync("alert('MainFrame finished loading');");
                }
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Browser.ExecuteScriptAsync("document.body.style.background = 'red';");
        }

        void Show(string msg)
        {
            Browser.ExecuteScriptAsync($"alert('{msg}');");
            Browser.ExecuteScriptAsync($" console.info('{msg}');");
           
            //Browser.ExecuteScriptAsync("alert('" + msg + "');");
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            JavascriptResponse response = await Browser.EvaluateScriptAsync("1 + 1");
            dynamic result = response.Result;
            Show(result.ToString());
        }
        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            JavascriptResponse response = await Browser.GetMainFrame().EvaluateScriptAsync("2 + 2");
            dynamic result = response.Result;
            Show(result.ToString());
        }
        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            JavascriptResponse response = await Browser.GetFocusedFrame().EvaluateScriptAsync("3 + 3");
            dynamic result = response.Result;
            Show(result.ToString());
        }

        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var script = @"(function() { let val = 1 + 1; return val; })();";
            JavascriptResponse response = await Browser.EvaluateScriptAsync(script);
            Show(response.Result.ToString());
        }

        private async void Button_Click_5(object sender, RoutedEventArgs e)
        {
            //If your script uses a Promise then you must use the EvaluateScriptAsPromiseAsync method, it differs slightly
            //in that you must return the value.
            //The following will return a Promise that after one second resolves with a simple objec
            var script = "return new Promise(function(resolve, reject) { setTimeout(resolve.bind(null, { a: 'CefSharp', b: 42, }), 1000); });";
            JavascriptResponse javascriptResponse = await Browser.EvaluateScriptAsPromiseAsync(script);
            //You can access the object using the dynamic keyword for convenience.
            dynamic result = javascriptResponse.Result;
            var a = result.a;
            var b = result.b;
            Show($"a:{a},b:{b}");
        }

        private async void Button_Click_6(object sender, RoutedEventArgs e)
        {
            //EvaluateScriptAsPromiseAsync calls Promise.resolve internally so even if your code doesn't
            //return a Promise it will still execute successfully.
            var script = @"return (function() { return 1 + 1; })();";
            JavascriptResponse response = await Browser.EvaluateScriptAsPromiseAsync(script);
            Show(response.Result.ToString());
        }

        private async void Button_Click_7(object sender, RoutedEventArgs e)
        {
            // An example that gets the Document Height
            var task = await Browser.EvaluateScriptAsync("(function() { var body = document.body, html = document.documentElement; return  Math.max( body.scrollHeight, body.offsetHeight, html.clientHeight, html.scrollHeight, html.offsetHeight ); })();");
            var task2 = await Browser.EvaluateScriptAsync("(function() { var body = document.body, html = document.documentElement; return  Math.max( body.scrollWidth, body.offsetWidth, html.clientWidth, html.scrollWidth, html.offsetWidth ); })();");
            Show($"Height:{task.Result.ToString()},Width:{task2.Result.ToString()}");
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            // An example that gets the Document Height
            var task = Browser.EvaluateScriptAsync("(function() { var body = document.body, html = document.documentElement; return  Math.max( body.scrollHeight, body.offsetHeight, html.clientHeight, html.scrollHeight, html.offsetHeight ); })();");

            //Continue execution on the UI Thread
            task.ContinueWith(t =>
            {
                if (!t.IsFaulted)
                {
                    var response = t.Result;
                    var EvaluateJavaScriptResult = response.Success ? (response.Result ?? "null") : response.Message;
                    Show($"Height:{EvaluateJavaScriptResult.ToString()}");
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async void Button_Click_9(object sender, RoutedEventArgs e)
        {
            //HTMLElement/HTMLCollection Examples
            //As stated above, you cannot return a HTMLElement/HTMLCollection directly.
            //It's best to return only the data you require, here are some examples of using Array.from to convert a HTMLCollection  into an array of objects
            //which can be returned to your .Net application.

            //Get all the span elements and create an array that contains their innerText
            var script = @"Array.from(document.getElementsByTagName('span')).map(x => ( x.innerText));";
            JavascriptResponse response = await Browser.EvaluateScriptAsync(script);
            var result = response.Result as List<object>;
            string msg = string.Empty;
            result.ForEach(t => msg += t.ToString() + Environment.NewLine);
            Show(msg);
        }

        private async void Button_Click_10(object sender, RoutedEventArgs e)
        {
            //Get all the a tags and create an array that contains a list of objects 
            //Second param is the mapping function
            var script = @"Array.from(document.getElementsByTagName('a'), x => ({ innerText : x.innerText, href : x.href }));";
            JavascriptResponse response = await Browser.GetFocusedFrame().EvaluateScriptAsync(script);
            string msg = string.Empty;

            dynamic result = response.Result;
            for (int i = 0; i < result.Count; i++)
            {
                msg += $"innerText:{result[i].innerText},href:{result[i].href}"+ " \\r\\n";
            }


            //var result = response.Result as List<object>;
            //result.ForEach(t => msg += t.ToString() + Environment.NewLine);

            Show(msg);
        }

        private async void Button_Click_11(object sender, RoutedEventArgs e)
        {
            //Browser.Address = "https://cefsharp.github.io/api/91.1.x/html/T_CefSharp_IJavascriptCallback.htm";
            //List of Links, click represents a function pointer which can be used to execute the link click)
            //In .Net the https://cefsharp.github.io/api/91.1.x/html/T_CefSharp_IJavascriptCallback.htm is used
            //to represent the function.
            var script = @"Array.from(document.getElementsByTagName('a')).map(x => ({ innerText: x.innerText, click: x.click}));";
            JavascriptResponse response = await Browser.EvaluateScriptAsync(script);
            string msg = string.Empty;
            dynamic result = response.Result;
            for (int i = 0; i < result.Count; i++)
            {
                msg += $"innerText:{result[i].innerText},click:{result[i].click}" + Environment.NewLine;
            }
            Show(msg);
        }

        private async void Button_Click_12(object sender, RoutedEventArgs e)
        {
            try
            {
                //Browser.Address = "google.com";
                //Execute the following against google.com to get the `I'm Feeling Lucky` button then click the button in .Net
                //NOTE: This is a simple example, you could return an aggregate object consisting of data from multiple html elements.
                const string script = @"(function()
{
  let element = document.getElementsByName('btnI')[0];
  let obj = {};
  obj.id = element.id;
  obj.nodeValue = element.nodeValue;
  obj.localName = element.localName;
  obj.tagName = element.tagName;
  obj.innerText = element.innerText;
  obj.click = element.click;
  obj.attributes = Array.from(element.attributes).map(x => ({name: x.name, value: x.value}));

  return obj;
})();";
                var javascriptResponse = await Browser.EvaluateScriptAsync(script);
                dynamic result = javascriptResponse.Result;
                var clickJavascriptCallback = (IJavascriptCallback)result.click;
                await clickJavascriptCallback.ExecuteAsync();
                //Dispose of the click callback when done
                clickJavascriptCallback.Dispose();
            }
            catch
            {
                MessageBox.Show("should go to google,now go");
                Browser.Address = "google.com";
            }
        }

        private void window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key.Equals(System.Windows.Input.Key.F12))
            {
                Browser.ShowDevTools();
            }
        }
    }

    public class RenderProcessMessageHandler : IRenderProcessMessageHandler
    {
        public void OnContextReleased(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
        {
            //throw new System.NotImplementedException();
        }

        public void OnFocusedNodeChanged(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IDomNode node)
        {
            //throw new System.NotImplementedException();
        }

        public void OnUncaughtException(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, JavascriptException exception)
        {
            throw new System.NotImplementedException();
        }

        // Wait for the underlying JavaScript Context to be created. This is only called for the main frame.
        // If the page has no JavaScript, no context will be created.
        void IRenderProcessMessageHandler.OnContextCreated(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {
            const string script = "document.addEventListener('DOMContentLoaded', function(){ alert('DomLoaded'); });";

            frame.ExecuteJavaScriptAsync(script);
        }
    }
}
