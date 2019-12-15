using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Request
{
    /// <summary>
    /// 请求结构体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseModel<T> where T :class
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 返回的信息 用于提示用
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 返回的数据参数T
        /// </summary>
        public T Data { get; set; }
    }
}
