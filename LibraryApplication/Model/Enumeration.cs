using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum LoginWay
    {
        SlotPatronCard = 1,//刷读者证
        SlotIdCard = 2,//刷身份证
        ScanQrCode = 3,//扫二维码
        Veriface = 4,//人脸识别
        Fingerprint = 5,//指纹识别
        Vein = 6,//静脉识别
        Handword = 7,//手工输入
        Scanner = 8,//扫码枪扫码
        OneDimensionalCode = 9,//一维码登录
        VirtualReaderCard = 10,//虚拟读者证登录
        CreditLogin = 11,//信用登录
        SocialSecurityCardCheck = 12//社保卡登录
    }

    public  enum ButtonType
    {
        BorrowBook,
        ReturnBook,
        PersonalCenter,
        RenewBook
    }
}
