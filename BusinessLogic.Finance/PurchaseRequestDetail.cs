using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Linq;
using BusinessLogic.Services.General;
using BusinessLogic.Services.Finance;
using NLog;
using DataAccess.PhoenixERP.Finance;
using DataAccess.PhoenixERP;
using NPoco;
using System.Transactions;
using DataAccess.Dto.Custom.Finance;
using DataAccess.PhoenixERP.General;
using BusinessLogic.Core;
using CommonTool.JEasyUI.DataGrid;
using Omu.ValueInjecter;
using CommonTool.KendoUI.Grid;

namespace BusinessLogic.Services
{
    public partial class PurchaseRequestDetail : PurchaseRequestDetailService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public PurchaseRequestDetail(DataContext AppUserData) : base(AppUserData) { }

        public DataResponse GetListKendoUIGrid(DataRequest dtRequest, String purchaseRequestId,bool isWindow)
        {
            DataResponse result = null;
            try
            {
                String OptionalFilters = string.Empty;
                String OptionalParameters = string.Empty;
                String OptionalQueryBuilder = string.Empty;
                if (isWindow)
                {
                    OptionalQueryBuilder = String.Format("  r.purchase_request_id='{1}' AND r.rfq_id IS NULL ", Context.OrganizationId, purchaseRequestId);
                }
                else
                {
                    OptionalQueryBuilder = String.Format("  r.purchase_request_id='{1}' ", Context.OrganizationId, purchaseRequestId);
                }
                
                //OptionalFilters = " r.purchase_request__id=@1";
                //OptionalParameters = Context.OrganizationId;
                //OptionalParameters += "," + purchaseRequestId;

                result = GetListKendoGrid(dtRequest, OptionalFilters, OptionalParameters, OptionalQueryBuilder);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public override bool SaveEntity(purchase_request_detailDTO RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;

            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    RecordDto.amount = (RecordDto.exchange_rate * RecordDto.unit_price) * RecordDto.quantity;

                    result = Svc.SaveEntity(RecordDto, ref isNew);
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                    throw;
                }
            }

            return result;
        }


        public List<purchase_request_detailDTO> getListDetails(String purchaseRequestId)
        {
            List<purchase_request_detailDTO> response = null;
            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    var sql = Sql.Builder.Append(purchase_request_detail.DefaultView);
                    sql.Append(" WHERE r.purchase_request_id = @0 AND r.is_active=1 AND COALESCE(r.is_deleted,0)=0  ", purchaseRequestId);
                    var details = db.Fetch<purchase_request_detail>(sql);
                    appLogger.Debug(db.LastCommand);
                    response = details.Select(x => new purchase_request_detailDTO().InjectFrom(x)).Cast<purchase_request_detailDTO>().ToList();


                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                    throw;
                }
            }
            return response;
        }


    }
}
