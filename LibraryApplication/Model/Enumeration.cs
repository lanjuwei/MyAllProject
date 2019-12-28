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



    public enum BookStatus 
    {
        /// <summary>
        /// 未知
        /// </summary>
        None,
        /// <summary>
        /// 借出
        /// </summary>
        Lended,
        /// <summary>
        /// 在馆
        /// </summary>
        Returned,
        /// <summary>
        /// 已预借
        /// </summary>
        Reserved,
    }
    ///// <summary>
    ///// 人脸识别流程控制
    ///// </summary>
    //public enum FaceRecognitionProcess
    //{
    //    /// <summary>
    //    /// 结束
    //    /// </summary>
    //    Over,
    //    /// <summary>
    //    /// 重新开始
    //    /// </summary>
    //    Continue,
    //}
    public enum ResultType
    {
        RecogineAgian,
        ToLogin,
        Close,
        Success
    }
    //public enum VideoFaceOperation 
    //{
    //    /// <summary>
    //    /// 检测人脸
    //    /// </summary>
    //    DetectFace,
    //    /// <summary>
    //    /// 识别人脸
    //    /// </summary>
    //    RecognitionFace,
    //    /// <summary>
    //    /// 截取人脸
    //    /// </summary>
    //    ShotFace,
    //    /// <summary>
    //    /// 无操作
    //    /// </summary>
    //    None
    //}
}
