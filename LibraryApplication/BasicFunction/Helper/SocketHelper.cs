using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BasicFunction.Helper
{
    public class SocketHelper
    {
        private static SocketHelper _socketHelper;
        public static SocketHelper Instance => _socketHelper ?? (_socketHelper = new SocketHelper());
        private Socket _client;
        private Encoding _encoder = Encoding.UTF8;
        /// <summary>
        /// 初始化
        /// </summary>
        public  async void InitSocket()
        {
            _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await _client.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2012));//连接       
            KeepHeart();
        }
        private void KeepHeart() 
        {
            Task.Run(()=> 
            {
                while (true)
                {
                    if (_client.Connected)
                    {

                    }
                }
            });
        }

        /// <summary>
        /// 获取响应数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="json"></param>
        public void GetRespone(string key,string json) 
        {
            var data = new List<byte>();
            var terminator =new byte[2] { (byte)'#', (byte)'#' };//可为字节数组
            while (true)
            {
                var buffer = new byte[1024];
                var count = _client.Receive(buffer);
                if (count==0)
                {
                    break;
                }
                var index = Array.IndexOf(buffer, terminator);
                if (index<0)
                {
                    data.AddRange(buffer);
                    continue;
                }
                else
                {
                    var data1= new byte[index-1];
                    var data2 = new byte[buffer.Length - index + 1];
                    Array.Copy(buffer,0, data1,0, data1.Length);
                    data.AddRange(data1);
                    var str = _encoder.GetString(data.ToArray());
                    if (str.Contains("key"))
                    {

                    }
                }
                
            }

        }
    }
}
