using GalaSoft.MvvmLight;
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
    public class UserModel : ViewModelBase
    {
        private string _id;
        private string _name;
        private string _password;
        private int _sex;
        private double _deposit;
        private double _preDeposit;
        private double _lateFee;
        private int _canBorrowCount;
        private int _lendCount;
        private string _faceImage;
        private byte[] _faceByte;

        /// <summary>
        /// 读者证号 唯一Key
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string Id
        {
            get => _id; set
            {
                Set(() => Id, ref _id, value);
            }
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name
        {
            get => _name; set
            {
                Set(() => Name, ref _name, value);
            }
        }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get => _password; set
            {
                Set(() => Password, ref _password, value);
            }
        }
        /// <summary>
        /// 性别 0男 1女
        /// </summary>
        public int Sex
        {
            get => _sex; set
            {
                Set(() => Sex, ref _sex, value);
            }
        }
        /// <summary>
        /// 押金
        /// </summary>
        public double Deposit
        {
            get => _deposit; set
            {
                Set(() => Deposit, ref _deposit, value);
            }
        }
        /// <summary>
        /// 预存款
        /// </summary>
        public double PreDeposit
        {
            get => _preDeposit; set
            {
                Set(() => PreDeposit, ref _preDeposit, value);
            }
        }
        /// <summary>
        /// 滞纳金
        /// </summary>
        public double LateFee
        {
            get => _lateFee; set
            {
                Set(() => LateFee, ref _lateFee, value);
            }
        }
        /// <summary>
        /// 可借图书数量
        /// </summary>
        public int CanBorrowCount
        {
            get => _canBorrowCount; set
            {
                Set(() => CanBorrowCount, ref _canBorrowCount, value);
            }
        }
        /// <summary>
        /// 已借出的数量
        /// </summary>
        public int LendCount
        {
            get => _lendCount; set
            {
                Set(() => LendCount, ref _lendCount, value);
            }
        }
        /// <summary>
        /// 图片流
        /// </summary>
        public byte[] FaceByte
        {
            get => _faceByte; set
            {
                Set(() => FaceByte, ref _faceByte, value);
            }
        }
        /// <summary>
        /// 人脸图片
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string FaceImage
        {
            get => _faceImage; set
            {

                Set(() => FaceImage, ref _faceImage, value);
            }
        }

    }
}
