using BusinessLogic.Services.Finance;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace BusinessLogic.Finance
{
    public partial class PurchaseBiddingDetail : PurchaseBiddingDetailService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public PurchaseBiddingDetail(DataContext AppUserData) : base(AppUserData)
        {

        }
        
    }
}
