using CefSharp;
using CefSharp.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CefS_JS_Test.Base
{
    public class JSCallSCObject
    {
        public FormulaData FormulaData { get; set; }
        public void Functions(string type, string jsonObject, string param = "")
        {
            switch (type.Trim())
            {
                case "FormulaInsert":
                    //string js = @$"window.Formula_show('{jsonObject}')";
                    //JavascriptResponse response = await Browser.GetFocusedFrame().EvaluateScriptAsync(js);
                    //dynamic result = response.Result;

                    FormulaData = JsonConvert.DeserializeObject<FormulaData>(jsonObject);
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

    public class FormulaData
    {
        public string width { get; set; }

        [JsonIgnore]
        public double Width 
        {
            get
            {
                if (double.TryParse(width.Replace("px", ""),out double w))
                {
                    return w;
                }
                else
                {
                    throw new Exception("width parse wrong.");
                }
            }
        }
        public string height { get; set; }
        [JsonIgnore]
        public double Height
        {
            get
            {
                if (double.TryParse(height.Replace("px", ""), out double w))
                {
                    return w;
                }
                else
                {
                    throw new Exception("height parse wrong.");
                }
            }
        }

        public string html { get; set; }

        public string latex { get; set; }

        public string type { get; set; }
    }
}
