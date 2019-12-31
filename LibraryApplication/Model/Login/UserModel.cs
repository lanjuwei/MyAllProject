using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

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
        [SugarColumn(IsNullable = true)]
        public string Password { get; set; }
        /// <summary>
        /// 性别 0男 1女
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 押金
        /// </summary>
        public double Deposit { get; set; }
        /// <summary>
        /// 预存款
        /// </summary>
        public double PreDeposit { get; set; }
        /// <summary>
        /// 滞纳金
        /// </summary>
        public double LateFee { get; set; }
        /// <summary>
        /// 人脸图片
        /// </summary>
        public ImageSource FaceImage { get; set; }

    }
}
