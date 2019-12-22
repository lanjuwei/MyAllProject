
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BasicFunction.Log
{
    public  class Logger 
    {
        private static log4net.ILog _log4NetDebugLogger = log4net.LogManager.GetLogger("debugLogger");
        private static log4net.ILog _log4NetInfoLogger = log4net.LogManager.GetLogger("infoLogger");
        private static log4net.ILog _log4NetWarnLogger = log4net.LogManager.GetLogger("warnLogger");
        private static log4net.ILog _log4NetErrorLogger = log4net.LogManager.GetLogger("errorLogger");
        private static log4net.ILog _log4NetfatalLogger = log4net.LogManager.GetLogger("fatalLogger");

        /// <summary>
        /// 构造函数
        /// </summary>
        static Logger()
        {
            try
            {
                Assembly log4NetAssembly = Assembly.GetCallingAssembly();//读取嵌入式资源
                using (Stream configStream = log4NetAssembly.GetManifestResourceStream("BasicFunction.Log.log4netConfig.xml"))
                {
                    if (configStream != null)
                    {
                        XmlConfigurator.Configure(configStream);
                    }
                }
            }
            catch(Exception ex)
            {
                
            }
        }

        public static void Debug(string message)
        {
            _log4NetDebugLogger.Debug(message);
        }

       
        public static void Error(Exception ex)
        {
            _log4NetErrorLogger.Error(ex.Message, ex);
        }
        public static void Fatal(string message)
        {
            _log4NetfatalLogger.Fatal(message);
        }

        public static void Info(string message)
        {
            _log4NetInfoLogger.Info(message);
        }


        public static void Warn(string message)
        {
            _log4NetWarnLogger.Warn(message);
        }

    }
}
