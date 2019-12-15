using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BasicFunction.Helper
{
    public class HttpHelper
    {
        private static HttpHelper _httpHelper;
        public static HttpHelper Instance => _httpHelper ?? (_httpHelper = new HttpHelper());

        /// <summary>
        /// 响应状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 获取响应结果
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="param"></param>
        /// <param name="json"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public string GetResponse(string url, HttpType method = HttpType.POST, Dictionary<string, string> param = null, string json = null, Dictionary<string, string> header = null)
        {
            var request = WebRequest.Create(url);
            request.Method = method.ToString();
            //请求头 内部网钥匙
            if (header != null)
            {
                foreach (var item in header)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }
            byte[] byteArray = null;
            //字典为参数
            if (param != null)
            {
                request.ContentType = "application/x-www-form-urlencoded";
                var encodedItems = param.Select(i => WebUtility.UrlEncode(i.Key) + "=" + WebUtility.UrlEncode(i.Value));
                string postData = string.Join("&", encodedItems);
                byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = byteArray.Length;
            }
            //json为参数
            if (json != null)
            {
                request.ContentType = "application/json";
                byteArray = Encoding.UTF8.GetBytes(json);
                request.ContentLength = byteArray.Length;
            }
            // Get the request stream.
            using (var dataStream = request.GetRequestStream())
            {
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
            // Get the original response.
            using (var response = request.GetResponse())
            // Get the stream containing all content returned by the requested server.
            using (var dataStream = response.GetResponseStream())
            // Open the stream using a StreamReader for easy access.
            using (var reader = new StreamReader(dataStream))
            {
                this.Status = ((HttpWebResponse)response).StatusDescription;
                return reader.ReadToEnd();
            }
        }
        public enum HttpType
        {
            POST,
            GET
        }


    }
}
