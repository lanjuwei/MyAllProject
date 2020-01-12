using Model.Login;
using Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicServices.SocketService
{
    /// <summary>
    /// 基类接口
    /// </summary>
    public interface ISocektInterface
    {
        /// <summary>
        /// 获取读者信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<ResponseModel<UserModel>> GetUserInfoAsync(string id, string password);
        /// <summary>
        /// 获取读者的在借列表
        /// </summary>
        /// <param name="id">传入读者的id</param>
        /// <returns></returns>
        Task<ResponseModel<List<BookModel>>> GetUserBookListAsync(string id);
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> LoginOut();
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> ChangePassword(string password);
    }
}
