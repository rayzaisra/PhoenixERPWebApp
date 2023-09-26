using System;
using System.Collections.Generic;
using System.Text;
using CommonTool;
using DataAccess;
using BusinessLogic;
using Newtonsoft.Json;
using NLog;
using CodeMarvel.Infrastructure.ModelShared;

namespace BusinessLogic.Test.Core
{
    public partial class Authentication
    {
        private readonly Logger appLogger;
        
        public Authentication(Logger logger)
        {
            appLogger = logger;
        }

        public void Run()
        {
            string log = "Run Authentication test ...";
            Console.WriteLine(log);
            appLogger.Debug(log);

            var auth = new BusinessLogic.Core.Authentication(appLogger);
            var cred = new UserCredential()
            {
                Username = "admin2",
                Password = "admin2"
            };

            log = string.Format("Run Authentication test on {0}", JsonConvert.SerializeObject(cred));            
            Console.WriteLine(log);
            appLogger.Debug(log);

            var token = "";
            var mstoken = "";
            var result = auth.Authenticate(cred, out token, out mstoken);
            log = string.Format("Result = {0}, token = {1}, mstoken = {1}", result, token, mstoken);
            Console.WriteLine(log);
            appLogger.Debug(log);
        }
    }
}
