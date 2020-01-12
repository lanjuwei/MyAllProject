using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicFunction.Helper;
using BasicFunction.Log;
using Model.Login;
using Model.Request;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BasicServices.SocketService
{
    /// <summary>
    /// 总的外部数据接口
    /// </summary>
    public class SocketService : ISocektInterface
    {
        /*最上层的接口设计结构模型：IsSuccess Data Message +try catch +输出日志*/

        /// <summary>
        /// 获取读者信息
        /// </summary>
        /// <param name="id">传入读者id</param>
        /// <param name="password">读者密码</param>
        /// <returns></returns>
        public Task<ResponseModel<UserModel>> GetUserInfoAsync(string id, string password)
        {
            return Task.Run(()=> 
            {
                var model = new ResponseModel<UserModel>();
                try
                {
                    var str = JsonConvert.SerializeObject(new { Id = id, Password = password });
                    var result = SocketHelper.Instance.GeResponseAsync(RequestKey.UserInfo, str);
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        var j = (JObject)JsonConvert.DeserializeObject(result);
                        var isOk = false;
                        if (bool.TryParse(j["IsSuccess"].ToString(), out isOk)&& isOk)
                        {
                            model.IsSuccess = true;
                            model.Data = JsonConvert.DeserializeObject<UserModel>((j["Data"].ToString()));
                            model.Message = j["Message"].ToString();
                        }
                        else
                        {
                            model.Message = j["Message"].ToString();
                        }
                    }
                    else
                    {
                        model.Message = "服务器无响应";
                    }
                }
                catch (Exception ex)
                {
                    model.Message = ex.Message;
                    Logger.Error(ex);
                }
                finally 
                {
                    if (!model.IsSuccess)
                    {
                        Logger.Info(model.Message);
                    }
                    TipService.TipService.Instance.ShowTip(TipService.TipService.ToolTip, 1000, model.Message);
                }
                return model;
            });
        }

        /// <summary>
        /// 获取读者的在借列表
        /// </summary>
        /// <param name="id">传入读者的id</param>
        /// <returns></returns>
        public Task<ResponseModel<List<BookModel>>> GetUserBookListAsync(string id)
        {
            return Task.Run(() =>
            {
                var model = new ResponseModel<List<BookModel>>();
                try
                {
                    var str = JsonConvert.SerializeObject(new { ReaderId = id });
                    var result = SocketHelper.Instance.GeResponseAsync(RequestKey.GetUserBookList, str);//方法key GetUserBookList
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        var j = (JObject)JsonConvert.DeserializeObject(result);
                        var isOk = false;
                        if (bool.TryParse(j["IsSuccess"].ToString(), out isOk) && isOk)
                        {
                            model.IsSuccess = true;
                            model.Data = JsonConvert.DeserializeObject<List<BookModel>>((j["Data"].ToString()));
                            model.Message = j["Message"].ToString();
                        }
                        else
                        {
                            model.Message = j["Message"].ToString();
                        }
                    }
                    else
                    {
                        model.Message = "服务器无响应";
                    }
                }
                catch (Exception ex)
                {
                    model.Message = ex.Message;
                    Logger.Error(ex);
                }
                finally
                {
                    if (!model.IsSuccess)
                    {
                        Logger.Info(model.Message);
                        TipService.TipService.Instance.ShowTip(TipService.TipService.ToolTip, 1000, model.Message);
                    }
                }
                return model;
            });
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ResponseModel<string>> LoginOut()
        {
            return Task.Run(() =>
            {
                var model = new ResponseModel<string>();
                try
                {
                    var result = SocketHelper.Instance.GeResponseAsync(RequestKey.LoginOut, "");//方法key GetUserBookList
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        var j = (JObject)JsonConvert.DeserializeObject(result);
                        var isOk = false;
                        if (bool.TryParse(j["IsSuccess"].ToString(), out isOk) && isOk)
                        {
                            model.IsSuccess = true;
                            model.Message = j["Message"].ToString();
                        }
                        else
                        {
                            model.Message = j["Message"].ToString();
                        }
                    }
                    else
                    {
                        model.Message = "服务器无响应";
                    }
                }
                catch (Exception ex)
                {
                    model.Message = ex.Message;
                    Logger.Error(ex);
                }
                finally
                {
                    if (!model.IsSuccess)
                    {
                        Logger.Info(model.Message);
                    }
                    else
                    {
                        TipService.TipService.Instance.ShowTip(TipService.TipService.ToolTip, 1000, model.Message);
                    }
                }
                return model;
            });
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ResponseModel<string>> ChangePassword(string password)
        {
            return Task.Run(() =>
            {
                var model = new ResponseModel<string>();
                try
                {
                    var result = SocketHelper.Instance.GeResponseAsync(RequestKey.ChangePassword, password);//方法key GetUserBookList
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        var j = (JObject)JsonConvert.DeserializeObject(result);
                        var isOk = false;
                        if (bool.TryParse(j["IsSuccess"].ToString(), out isOk) && isOk)
                        {
                            model.IsSuccess = true;
                            model.Message = j["Message"].ToString();
                        }
                        else
                        {
                            model.Message = j["Message"].ToString();
                        }
                    }
                    else
                    {
                        model.Message = "服务器无响应";
                    }
                }
                catch (Exception ex)
                {
                    model.Message = ex.Message;
                    Logger.Error(ex);
                }
                finally
                {
                    if (!model.IsSuccess)
                    {
                        Logger.Info(model.Message);
                    }
                    else
                    {
                        TipService.TipService.Instance.ShowTip(TipService.TipService.ToolTip, 1000, model.Message);
                    }
                }
                return model;
            });
        }


    }
}
