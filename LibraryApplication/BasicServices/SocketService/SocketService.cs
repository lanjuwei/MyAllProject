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
        /// <param name="id"></param>
        /// <param name="password"></param>
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
                        if (bool.TryParse(j["IsSuccess"].ToString(), out isOk))
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
                        model.Message = "GetUserInfoAsync方法返回空字符串";
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
                }
                return model;
            });
        }
    }
}
