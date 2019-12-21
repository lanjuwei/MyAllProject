﻿using Model.Login;
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
    }
}