using BasicFunction.Log;
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
        private byte terminator =(byte)'#';
        private object sendLock = new object();
        private bool isRuning = true;
        private Task backgroundTask;
        /// <summary>
        /// socket断开回调
        /// </summary>
        public  event Action<bool> SocketDisconnectCallBack;
        private int disconnectCount;
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
        /// 短连接获取响应 不是这么写的
        /// </summary>
        public  string GeResponseAsyncByShortConnect(RequestKey key, string json) 
        {
            using (_client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                _client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2012));//连接socket
                _client.ReceiveTimeout = 5000;
                return  GetRespone(key.ToString(), json);
            }
        }
        /// <summary>
        /// 析构函数 程序进程关闭时必须释放全部的_client资源并关闭连接 
        /// 光是dispose是不够的 只是释放了内存 垃圾回收器还没有回收 连接如果还在的话 系统进程依旧有 会不停的给服务器发送消息
        /// 直到关闭
        /// </summary>
        ~SocketHelper()
        {
            isRuning = false;
            if (_client?.Connected==true)
            {
                _client.Shutdown( SocketShutdown.Both);//关闭收发
            }
            _client?.Dispose();//释放内存
        }

        /// <summary>
        /// 长连接获取响应
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public  string GeResponseAsync(RequestKey key, string json) 
        {
            return  GetRespone(key.ToString(), json);
        }

        private  void Connect() 
        {
            try
            {
                if (_client==null)
                {
                    _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//只实例化一个socket
                    _client.ReceiveTimeout = 30000;//30秒超时便结束         
                    _client.SendTimeout = 30000;
                }               
                _client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2012));//连接    
            }
            catch (Exception ex)
            {
                //_client.Close();close() 与Dispose一样
                //_client?.Dispose();//及时释放 不占用内存
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
                        if (disconnectCount==3)//代表窗口已经打开
                        {
                            SocketDisconnectCallBack?.Invoke(false);
                            disconnectCount = 0;
                        } 
                        GetRespone("CheckConnect");
                        Thread.Sleep(5000);
                    }
                    else
                    {
                        disconnectCount++;
                        if (disconnectCount==3)
                        {
                            SocketDisconnectCallBack?.Invoke(true);                           
                        }
                        Thread.Sleep(1000);
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
        private string GetRespone(string key, string json=" ")
        {
            lock (sendLock)//多个地方会调用这个异步线程 得锁住维护一个线程列表 保证每次只有一个线程能进来
            {
                var responseData = string.Empty;
                try
                {
                    if (!_client.Connected)
                    {
                        Logger.Info("没有连接到Socket服务器");
                        return responseData;
                    }
                    var requestInfo = $"{key}--{json}##";
                    _client.Send(_encoder.GetBytes(requestInfo));//send request
                    while (true)
                    {
                        var buffer = new byte[10];
                        var count = _client.Receive(buffer);
                        if (count == 0)
                        {
                            continue;
                        }
                        _dataContainer.AddRange(buffer);
                        var index = _dataContainer.IndexOf(terminator);
                        if (index < 0)//未找到结束符
                        {
                            continue;
                        }
                        else
                        {
                            var data1 = _dataContainer.GetRange(0, index);
                            responseData = _encoder.GetString(data1.ToArray());//数据源
                            if (responseData.Contains(key))
                            {
                                _dataContainer.Clear();
                                return responseData;
                            }
                            else
                            {
                                if (_dataContainer.Count > index + 1)
                                {
                                    var data2 = _dataContainer.GetRange(index + 1, _dataContainer.Count - 1 - index);//截取后半部分
                                    _dataContainer.Clear();//清空
                                    _dataContainer.AddRange(data2);//再度添加
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Info(ex.Message);
                    _dataContainer.Clear();
                }
                return responseData;
            }
        }
    }
    /// <summary>
    /// 方法key
    /// </summary>
    public enum RequestKey
    {
        UserInfo,
        GetUserBookList,
        LoginOut,
        ChangePassword,
        UploadImage,
        DeleteFace,
        ReturnBooks,
        GetAllbooks,
        BorrowBooks,
        RenewBooks
    }
}
