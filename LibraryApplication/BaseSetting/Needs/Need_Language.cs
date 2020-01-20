using BasicFunction.Helper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSetting.Needs
{
    public partial class IndividualNeeds
    {
        private Language languageVariables = new Language();
        /// <summary>
        /// 语言变量
        /// </summary>
        public Language LanguageVariables
        {
            get => languageVariables; set
            {
                Set(() => LanguageVariables, ref languageVariables, value);
            }
        }

        public LanguageType CurrentLanguageType { get; set; }

 
        public void SetLanguageNeeds(LanguageType languageType = LanguageType.Cn)
        {
            var chineseLanguage = XmlHelper.Instance.DeserializeFromXml<Language>("ChineseConfig.xml");
            if (chineseLanguage == null)
            {
                XmlHelper.Instance.SerializeToXml(LanguageVariables, "ChineseConfig.xml");
            }
            var englishLanguage = XmlHelper.Instance.DeserializeFromXml<Language>("EnglishConfig.xml");
            if (englishLanguage == null)
            {
                XmlHelper.Instance.SerializeToXml(LanguageVariables, "EnglishConfig.xml");
            }
            CurrentLanguageType = languageType;
            switch (languageType)
            {
                case LanguageType.En:
                    if (chineseLanguage != null)
                    {
                        LanguageVariables = chineseLanguage;
                    }
                    break;
                case LanguageType.Cn:
                    if (englishLanguage != null)
                    {
                        LanguageVariables = englishLanguage;
                    }
                    break;
            }
        }
    }
}
