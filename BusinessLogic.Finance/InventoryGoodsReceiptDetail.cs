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
    public partial class InventoryGoodsReceiptDetail : InventoryGoodsReceiptDetailService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public InventoryGoodsReceiptDetail(DataContext AppUserData) : base(AppUserData) { }

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
                    OptionalQueryBuilder = String.Format("r.inventory_goods_receipt_id='{0}' AND r.rfq_id IS NULL ", purchaseRequestId);
                }
                else
                {
                    OptionalQueryBuilder = String.Format(" r.inventory_goods_receipt_id='{0}' ", purchaseRequestId);
                }
                
                result = GetListKendoGrid(dtRequest, OptionalFilters, OptionalParameters, OptionalQueryBuilder);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public override bool SaveEntity(inventory_goods_receipt_detailDTO RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;

            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    //RecordDto.amount = (RecordDto.exchange_rate * RecordDto.unit_price) * RecordDto.quantity;

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

        public List<inventory_goods_receipt_detailDTO> getListDetails(String purchaseRequestId)
        {
            List<inventory_goods_receipt_detailDTO> response = null;
            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    var sql = Sql.Builder.Append(purchase_request_detail.DefaultView);
                    sql.Append(" WHERE r.inventory_goods_receipt_id = @0 AND r.is_active=1 AND COALESCE(r.is_deleted,0)=0  ", purchaseRequestId);
                    var details = db.Fetch<purchase_request_detail>(sql);
                    appLogger.Debug(db.LastCommand);
                    response = details.Select(x => new inventory_goods_receipt_detailDTO().InjectFrom(x)).Cast<inventory_goods_receipt_detailDTO>().ToList();


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
