using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model
{
    [XmlRoot(ElementName = "config")]
    public class Language
    {
        public string LanguageName = "中文";
        public string PublicNumberDescription { get; set; } = "扫码即刻关注图书馆微信公众号";
        public string ProductName { get; set; } = "自助借还书机";
        public string TerminalNumber { get; set; } = "终端：数字图书馆    版本：2020-01-19";
        public string BorrowBook { get; set; } = "借书";
        public string ReturnBook { get; set; } = "还书";
        public string RenewBook { get; set; } = "续借";
        public string PersonalCenter { get; set; } = "个人中心";
    }
}
