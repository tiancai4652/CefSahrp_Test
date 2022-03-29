using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI_Test
{
    class Class1
    {
        public string Course3SDKSetRequestAddress { get { return "https://jyyapi-fz.jiaoyanyun.com/v1/sdkversion/fastsave"; } }
        public string Course3SDKGetRequestAddress { get { return "https://jyyapi-fz.jiaoyanyun.com/v1/sdkversion/getlatestsdkversion"; } }

        string source = "100001";
        string appid = "101";
        string appkey = "da907a1b8f74e6922d93b025eecfb852";
        public string HeaderSource { get { return "AUTH-SOURCE"; } }

        public string HeaderTime { get { return "AUTH-TIME"; } }

        public string HeaderSign { get { return "AUTH-SIGN"; } }
        public bool Set(string versionNum,string remark,string jssdk, int status)
        {
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)WebRequest.Create(Course3SDKSetRequestAddress);
            request.Method = "POST";
            request.Headers.Add(HeaderSource, GetHeaderSourceContent());
            request.Headers.Add(HeaderTime, GetHeaderTimeContent());
            request.Headers.Add(HeaderSign, GetHeaderSignContent());
            request.ContentType = "application/json";

            #region 添加Post 参数

            var jsondata = new { id = 0, versionNum = versionNum, remark= remark,jssdk= jssdk , status = status };
            var json = JsonConvert.SerializeObject(jsondata);

            byte[] data = Encoding.UTF8.GetBytes(json);
            request.ContentLength = data.Length;
            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion

            string result = "";
            HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }

            JObject jobject = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(result);
            result = jobject["msg"].ToString();
            return result.Equals("success");
        }

        public bool Get(out string x)
        {
            x = "";
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)WebRequest.Create(Course3SDKGetRequestAddress);
            request.Method = "GET";
            request.Headers.Add(HeaderSource, GetHeaderSourceContent());
            request.Headers.Add(HeaderTime, GetHeaderTimeContent());
            request.Headers.Add(HeaderSign, GetHeaderSignContent());
            request.ContentType = "application/json";

           
            string result = "";
            HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }

            try
            {
                JObject jobject = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                result = jobject["data"]["jssdk"].ToString();
                x = result;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetHeaderSourceContent()
        {
            return source;
        }

        public string GetHeaderTimeContent()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        public string GetHeaderSignContent()
        {
            string md5 = appid + appkey + '&' + GetHeaderTimeContent();
            return GetMD5(md5);
        }

      
        public static string GetMD5(string str)
        {
            try
            {
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] bytValue, bytHash;
                bytValue = System.Text.Encoding.UTF8.GetBytes(str);
                bytHash = md5.ComputeHash(bytValue);
                md5.Clear();
                string sTemp = "";
                for (int i = 0; i < bytHash.Length; i++)
                {
                    sTemp += bytHash[i].ToString("X").PadLeft(2, '0');
                }
                str = sTemp.ToLower();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return str;
        }
    }
}
