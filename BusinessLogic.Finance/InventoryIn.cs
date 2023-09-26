using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Linq;
using BusinessLogic.Services.Finance;
using NLog;
using DataAccess.PhoenixERP.Finance;
using DataAccess.PhoenixERP;
using NPoco;
using System.Transactions;
using Omu.ValueInjecter;

namespace BusinessLogic.Services
{
    public partial class InventoryIn : InventoryInService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public InventoryIn(DataContext AppUserData) : base(AppUserData)
        {

        }

        public override bool SaveEntity(inventory_inDTO RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;
            RecordDto.organization_id = Context.OrganizationId;
            try
            {
                result = Svc.SaveEntity(RecordDto, ref isNew);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

    }
}
