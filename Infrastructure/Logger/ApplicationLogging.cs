using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace CodeMarvel.Infrastructure.Logger
{
    //Simplify Call Nlog.config
    //Logger appLogger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
    public class ApplicationLogging
    {
        private static ILoggerFactory _Factory = null;

        public static void ConfigureLogger(ILoggerFactory factory)
        {
            try
            {
                factory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
                NLog.LogManager.LoadConfiguration("nlog.config");
            }
            catch (Exception)
            {
                return;   
            }
        }

        public static ILoggerFactory LoggerFactory
        {
            get
            {
                if (_Factory == null)
                {
                    _Factory = new LoggerFactory();
                    ConfigureLogger(_Factory);
                }
                return _Factory;
            }
            set { _Factory = value; }
        }
        public static ILogger GetCurrentClassLogger<T>() => LoggerFactory.CreateLogger<T>();

    }
}
