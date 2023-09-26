using System;
using DataAccess;
using DataAccess.PhoenixERP;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Omu.ValueInjecter;
using BusinessLogic;
using System.Transactions;
using Newtonsoft.Json;

namespace BusinessLogic.Services
{
    public partial class PrivateService
    {
        protected DataContext context;

        protected NLog.Logger appLogger = NLog.LogManager.GetCurrentClassLogger();

        public PrivateService(DataContext AppUserData)
        {
            context = AppUserData;
        }

    }
}
