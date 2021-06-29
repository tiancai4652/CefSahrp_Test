using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Text;

namespace CefS_JS_Test.Base
{
    public class JSCallSCObject
    {
        public void Functions(string type, string jsonObject, string param="")
        {
            switch (type.Trim())
            {
                case "FormulaInsert":
                    //string js = @$"window.Formula_show('{jsonObject}')";
                    //JavascriptResponse response = await Browser.GetFocusedFrame().EvaluateScriptAsync(js);
                    //dynamic result = response.Result;
                    break;
                default:
                    break;
            }
        }

        //public void Functions(string type, string jsonObject)
        //{
        //    switch (type.Trim())
        //    {
        //        case "FormulaInsert":
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }
}
