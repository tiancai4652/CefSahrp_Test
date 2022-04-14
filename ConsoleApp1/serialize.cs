using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class serialize<T>
    {
        public T getJsonContract(string obj)
        {
            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(obj));
            DataContractJsonSerializer deseralizer = new DataContractJsonSerializer(typeof(T));
            T data = (T)deseralizer.ReadObject(ms);// //反序列化ReadObject
            return data;
        }

        public T getJsonContract(object obj)
        {
            string objStr = obj.ToString();
            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(objStr));
            DataContractJsonSerializer deseralizer = new DataContractJsonSerializer(typeof(T));
            T data = (T)deseralizer.ReadObject(ms);// //反序列化ReadObject
            return data;
        }

        public T getJsonContract(byte[] byteArray)
        {
            MemoryStream ms = new MemoryStream(byteArray);
            DataContractJsonSerializer deseralizer = new DataContractJsonSerializer(typeof(T));
            T data = (T)deseralizer.ReadObject(ms);// //反序列化ReadObject
            return data;
        }

        public string getContractJson(T obj)
        {
            MemoryStream ms = new MemoryStream();
            DataContractJsonSerializer deseralizer = new DataContractJsonSerializer(typeof(T));
            deseralizer.WriteObject(ms, obj);
            byte[] dataBytes = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(dataBytes, 0, (int)ms.Length);
            return Encoding.UTF8.GetString(dataBytes);
        }
    }
}
