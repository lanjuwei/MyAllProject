using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Login
{
    [SugarTable("User")]
    public class UserModel
    {
        /// <summary>
        /// 序列号
        /// </summary>
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Index { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 身份证 唯一Key
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Id { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
    }
}
