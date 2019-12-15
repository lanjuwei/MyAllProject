using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicFunction.Helper;
using Model.Login;
using Model.Request;
using Newtonsoft.Json;

namespace BasicServices.SocketService
{
    public class SocketService : ISocektInterface
    {
        public  Task<ResponseModel<UserModel>> GetUserInfoAsync(string id, string password)
        {
            return Task.Run(()=> 
            {
                var model = new ResponseModel<UserModel>();
                var str = JsonConvert.SerializeObject(new { Id = id, Password = password });
                var result = SocketHelper.Instance.GeResponseAsync(RequestKey.UserInfo, str);
                if (result.IsSuccess)
                {
                    model = JsonConvert.DeserializeObject<ResponseModel<UserModel>>(result.Data);
                }
                else
                {
                    model.Message = result.Message;
                }
                return model;
            });
        }
    }
}
