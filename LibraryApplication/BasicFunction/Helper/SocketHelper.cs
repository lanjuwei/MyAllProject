using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BasicFunction.Helper
{
    public class SocketHelper
    {
        private static SocketHelper _socketHelper;
        public static SocketHelper Instance => _socketHelper ?? (_socketHelper = new SocketHelper());
        private Socket _client;
        private Encoding _encoder = Encoding.UTF8;
        private List<byte> _dataContainer = new List<byte>();
        private byte[] terminator =new byte[2] { (byte)'#', (byte)'#' };
        private object sendLock = new object();
        /// <summary>
        /// 长连接初始化
        /// </summary>
        public async void InitLongSocket()
        {
            await Connect();
            KeepHeart();
        }
        /// <summary>
        /// 短连接
        /// </summary>
        public async Task<string> ShortSocket(string key, string json) 
        {
            using (_client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                await _client.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2012));//连接
                return await GetRespone(key, json);
            }            
        }

        private async Task Connect() 
        {
            try
            {
                _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                await _client.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2012));//连接    
            }
            catch (Exception ex)
            {
            }
        }
        private void KeepHeart() 
        {
            Task.Run(async ()=> 
            {
                while (true)
                {
                    if (_client.Connected)
                    {
                        await GetRespone("CheckConnect", DateTime.Now.ToString());
                        Thread.Sleep(5000);
                    }
                    else
                    {
                        await Connect();
                        Thread.Sleep(2000);
                    }
                }
            });
        }

        /// <summary>
        /// 获取响应数据
        /// </summary>
        /// <param name="key">方法的key</param>
        /// <param name="json">参数</param>
        public Task<string> GetRespone(string key, string json)
        {
            return Task.Run(() =>
            {
                lock (sendLock)//多个地方会调用这个异步线程 得锁住维护一个线程列表 保证每次只有一个线程能进来
                {
                    if (!_client.Connected)
                    {
                        return null;
                    }
                    var id = Guid.NewGuid().ToString();
                    var requestInfo = $"{key}--{json}--{id}##";
                    _client.Send(_encoder.GetBytes(requestInfo));//send request          
                    while (true)
                    {
                        var buffer = new byte[1024];
                        var count = _client.Receive(buffer);
                        if (count == 0)
                        {
                            continue;
                        }
                        var index = Array.IndexOf(buffer, terminator);
                        if (index < 0)//未找到结束符
                        {
                            _dataContainer.AddRange(buffer);
                            continue;
                        }
                        else
                        {
                            var data1 = new byte[index - 1];
                            var data2 = new byte[buffer.Length - index + 1];
                            Array.Copy(buffer, 0, data1, 0, data1.Length);
                            _dataContainer.AddRange(data1);
                            var str = _encoder.GetString(_dataContainer.ToArray());
                            if (str.Contains(id))
                            {
                                _dataContainer.Clear();
                                return str;
                            }
                            else
                            {
                                _dataContainer.Clear();
                                Array.Copy(buffer, buffer.Length - index + 1, data2, 0, data2.Length);
                                _dataContainer.AddRange(data2);
                            }
                        }
                    }
                }
            });
        }
    }
}
