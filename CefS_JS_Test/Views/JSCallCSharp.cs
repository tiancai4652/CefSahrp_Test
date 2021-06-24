using CefS_JS_Test.Base;
using CefSharp;
using CefSharp.JavascriptBinding;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;


namespace CefS_JS_Test.Views
{
    public partial class MainWindow : Window
    {
        internal void InitJSCallCSharp()
        {
			//InAdvance立即注册
			//For async object registration (equivalent to the old RegisterAsyncJsObject)
			Browser.JavascriptObjectRepository.Register("boundAsync", new BoundObject(), BindingOptions.DefaultBinder);

			//WhenRequired在使用的时候才注册
			Browser.JavascriptObjectRepository.ResolveObject += (sender, e) =>
			{
				var repo = e.ObjectRepository;
				if (e.ObjectName == "boundAsyncWhenRequired")
				{
					//Binding options is an optional param, defaults to null
					BindingOptions bindingOptions = null;
					//Use the default binder to serialize values into complex objects
					bindingOptions = BindingOptions.DefaultBinder;
					//bindingOptions = new BindingOptions { Binder = new MyCustomBinder() }); //Specify a custom binder
					repo.NameConverter = null; //No CamelCase of Javascript Names
											   //For backwards compatability reasons the default NameConverter doesn't change the case of the objects returned from methods calls.
											   //https://github.com/cefsharp/CefSharp/issues/2442
											   //Use the new name converter to bound object method names and property names of returned objects converted to camelCase
					//repo.NameConverter = new CamelCaseJavascriptNameConverter();
					repo.Register("boundAsyncWhenRequired", new BoundObject(), options: bindingOptions);
				}
			};

			//注册成功通知
			Browser.JavascriptObjectRepository.ObjectBoundInJavascript += (sender, e) =>
			{
				var name = e.ObjectName;

				Debug.WriteLine($"Object {e.ObjectName} was bound successfully.");
			};
		}


        private async void Button_Click_13(object sender, RoutedEventArgs e)
        {
            const string script = @"(async function()
{
			await CefSharp.BindObjectAsync('boundAsync');

	//The default is to camel case method names (the first letter of the method name is changed to lowercase)
			boundAsync.add(16, 2).then(function(actualResult)
	{
				const expectedResult = 18;
				alert(actualResult);
			});
		})();";
            var javascriptResponse = await Browser.EvaluateScriptAsync(script);
            dynamic result = javascriptResponse.Result;
		}

        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
			InitJSCallCSharp();

		}
    }
}
