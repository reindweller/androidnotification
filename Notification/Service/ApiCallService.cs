using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace Notification.Service
{
    public static class ApiCallService
    {

        public static HttpWebResponse CreateRequest<TModel>(TModel model, string url)
        {
            var data = JsonConvert.SerializeObject(model);
            var address = new Uri(url);
            var byteData = UTF8Encoding.UTF8.GetBytes(data);
            var request = WebRequest.Create(address) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json; encoding='utf-8'";
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            return response;
        }

        public static HttpWebResponse CreateRequest1<TModel>(TModel model, string url)
        {
            System.Net.HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            using (var sw = new StreamWriter(request.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(model);
                sw.Write(json);
                sw.Flush();
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            return response;
        }

        public static async Task<HttpWebResponse> CreateRequestAsync<TModel>(TModel model, string url)
        {
            System.Net.HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            using (var sw = new StreamWriter(request.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(model);
                sw.Write(json);
                sw.Flush();
            }
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            return response;
        }

        public static async Task<HttpWebResponse> UpdateRequestAsync<TModel>(TModel model, string url)
        {
            System.Net.HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "PUT";
            request.ContentType = "application/json";
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            using (var sw = new StreamWriter(request.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(model);
                sw.Write(json);
                sw.Flush();
            }
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            return response;
        }


        public static HttpWebResponse GetAllRequest(string url)
        {
            var address = new Uri(url);
            var request = WebRequest.Create(address) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/json; encoding='utf-8'";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            return response;
        }
        
    }
}