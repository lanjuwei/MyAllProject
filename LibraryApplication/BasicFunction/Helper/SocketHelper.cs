using Model.Request;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private bool isRuning = true;
        private Task backgroundTask;
        /// <summary>
        /// 长连接初始化
        /// </summary>
        public  void InitLongSocketAsync()
        {
            Task.Run(()=> 
            {
                Connect();
                KeepHeart();
            });
        }
        /// <summary>
        /// 短连接获取响应
        /// </summary>
        public  ResponseModel<string> GeResponseAsyncByShortConnect(RequestKey key, string json) 
        {           
            using (_client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                _client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2012));//连接socket
                _client.ReceiveTimeout = 5;
                return  GetRespone(key.ToString(), json);
            }
        }
        /// <summary>
        /// 析构函数 程序进程关闭时必须释放全部的_client资源 不然会产生内存泄漏 如果socket是长连接的话
        /// </summary>
        ~SocketHelper()
        {
            isRuning = false;
            _client?.Dispose();
        }

        /// <summary>
        /// 长连接获取响应
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public  ResponseModel<string> GeResponseAsync(RequestKey key, string json) 
        {
            return  GetRespone(key.ToString(), json);
        }

        private  void Connect() 
        {
            try
            {
                _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _client.ReceiveTimeout = 5;
                _client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2012));//连接    
            }
            catch (Exception ex)
            {
                //_client.Close();不用close() 这种方法在垃圾回收的时候才释放内存 不能及时的释放
                _client?.Dispose();//及时释放 不占用内存
            }
            finally
            {
                
            }
        }
        private void KeepHeart() 
        {
            backgroundTask=Task.Run( ()=> 
            {
                while (isRuning)
                {
                    if (_client.Connected)
                    {
                        var data=GetRespone("CheckConnect");
                        Thread.Sleep(5000);
                    }
                    else
                    {
                        Thread.Sleep(3000);
                        Connect();
                    }
                }
            });
        }
        

        /// <summary>
        /// 获取响应数据
        /// </summary>
        /// <param name="key">方法的key</param>
        /// <param name="json">参数</param>
        private ResponseModel<string> GetRespone(string key, string json=" ")
        {

            lock (sendLock)//多个地方会调用这个异步线程 得锁住维护一个线程列表 保证每次只有一个线程能进来
            {
                var model = new ResponseModel<string>();
                var isComplete = false;
                try
                {
                    if (!_client.Connected)
                    {
                        model.Message = "没有连接到Socket服务器";
                        return model;
                    }                    
                    var requestInfo = $"{key}--{json}##";
                    _client.Send(_encoder.GetBytes(requestInfo));//send request
                    while (!isComplete)
                    {
                        var buffer = new byte[1024];
                        var count = _client.Receive(buffer);
                        if (count == 0)
                        {
                            continue;
                        }
                        int index = buffer.Select((x, i) => new { i, x = buffer.Skip(i).Take(2) }).FirstOrDefault(x => x.x.SequenceEqual(terminator)).i;
                        if (index < 0)//未找到结束符
                        {
                            _dataContainer.AddRange(buffer);
                            continue;
                        }
                        else
                        {
                            var data1 = new byte[index];
                            Array.Copy(buffer, 0, data1, 0, data1.Length);
                            _dataContainer.AddRange(data1);
                            var str = _encoder.GetString(_dataContainer.ToArray());
                            if (str.Contains(key))
                            {
                                _dataContainer.Clear();
                                model.IsSuccess = true;
                                model.Data = str;
                                return model;
                            }
                            else
                            {
                                //_dataContainer.Clear();
                                //Array.Copy(buffer, buffer.Length - index + 1, data2, 0, data2.Length);
                                //_dataContainer.AddRange(data2);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    model.Message = ex.Message;
                    _dataContainer.Clear();
                }
                finally
                {
                    isComplete = true;
                }
                return model;
            }
        }
    }
    public enum RequestKey
    {
        UserInfo,
    }
}
