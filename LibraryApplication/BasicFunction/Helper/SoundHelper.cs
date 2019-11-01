

using SpeechLib;
using System.Text;
using System.Threading.Tasks;

namespace BasicFunction.Helper
{
    public class SoundHelper
    {
        private static SoundHelper _soundHelper;
        public static SoundHelper Instance => _soundHelper ?? (_soundHelper = new SoundHelper());
        private const string DefaultEnglishLangID = "409";// 默认的英文语音ID
        private const string DefaultChineseLangID = "804";//默认的中文语音ID
        private const string DefaultJapaneseLangID = "411";//默认的日文语音ID
        public Task TransformTextToVideo(string text)
        {
            return Task.Run(()=>
            {
                var src = AddXmlLangTag(text);
                SpVoice Voice = new SpVoice();
                Voice.Voice = Voice.GetVoices(string.Empty, string.Empty).Item(0);
                Voice.Speak(src, SpeechVoiceSpeakFlags.SVSFIsXML | SpeechVoiceSpeakFlags.SVSFlagsAsync);
            });
        }
        private  string AddXmlLangTag(string src)
        {
            return AddXmlLangTag(src, DefaultEnglishLangID, DefaultChineseLangID);
        }
        private  string AddXmlLangTag(string src, string englishLangID, string chineseLangID)
        {
            if (src.Length < 1)
            {
                return "";
            }
            StringBuilder dest = new StringBuilder();
            int startPos = 0, endPos = 0;
            bool isAscii = !(src[0] > 128);
            for (int i = 0; i < src.Length; i++)
            {
                /* 判断每个字符是否为ASCII，如果是则加上<voice required="Language=englishLangID"></voice>
                   如果不是就加上<voice required="Language=chineseLangID></voice>*/
                if (src[i] > 128)
                {
                    if (isAscii)
                    {
                        string sub = src.Substring(startPos, endPos - startPos);
                        dest.Append("<voice required=\"Language=" + englishLangID + "\">" + sub + "</voice>");
                        startPos = endPos;
                    }
                    isAscii = false;
                    endPos++;
                }
                else
                {
                    if (!isAscii)
                    {
                        string sub = src.Substring(startPos, endPos - startPos);
                        dest.Append("<voice required=\"Language=" + chineseLangID + "\">" + sub + "</voice>");
                        startPos = endPos;
                    }
                    isAscii = true;
                    endPos++;
                }
            }
            string r = src.Substring(startPos, endPos - startPos);
            string langID = isAscii == true ? englishLangID : chineseLangID;
            dest.Append("<voice required=\"Language=" + langID + "\">" + r + "</voice>");
            return dest.ToString();
        }
    }
}