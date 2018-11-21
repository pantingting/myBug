using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHEER.Common
{
    public class Log2
    {
        private Log2() { InitLog4Net(); }
        public static readonly Log2 Instance = new Log2();

        public void Info(Type type, string message, Exception ex = null)
        {
            var logger = LogManager.GetLogger(type);
            if (ex != null)
            {
                logger.Info(message, ex);
            }
            else
            {
                logger.Info(message);
            }
        }

        public void Debug(Type type, string message, Exception ex = null)
        {
            var logger = LogManager.GetLogger(type);
            if (ex != null)
            {
                logger.Debug(message, ex);
            }
            else
            {
                logger.Debug(message);
            }
        }

        public void Error(Type type, string message, Exception ex = null)
        {
            var logger = LogManager.GetLogger(type);
            if (ex != null)
            {
                logger.Error(message, ex);
            }
            else
            {
                logger.Error(message);
            }
        }

        private void InitLog4Net()
        {
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "/Config/log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);
        }
    }
}
