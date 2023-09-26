using BusinessLogic.Finance;
using CodeMarvel.Infrastructure.ModelShared;
using CommonTool.KendoUI.Grid;
using Microsoft.AspNetCore.Mvc;
using PhoenixWebApi.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixWebApi.Areas.Finance.Controllers
{
    [Route("api/finance/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Finance")]
    [ApiController]
    public class PurchaseBiddingController : ApiBaseController
    {

        [HttpGet]
        [Route("{Id}/details")]
        public async Task<IActionResult> Details(string Id)
        {
            var service = new PurchaseBidding(AppUserData);
            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                var record = service.Details(Id);
                try
                {
                    if (record != null)
                    {
                        result.Status.Success = true;
                        result.Data = record;
                    }
                    else
                    {
                        result.Status.Message = "Record not found";
                    }
                }
                catch (Exception ex)
                {
                    result.Status.Message = ex.Message;
                }
                return result;
            });
            if (detailResult == null) return new EmptyResult();
            else return Ok(detailResult);
        }


        [HttpPost]
        [Route("kendoGrid")]
        public async Task<IActionResult> GetDataKendoGridBinder(DataRequest dtRequest)
        {
            var Service = new PurchaseBidding(AppUserData);
            var listResult = await Task.Run<DataResponse>(() =>
            {
                return Service.GetListKendoGrid(dtRequest);
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }

        [HttpPost]
        [Route("createbidding")]
        public async Task<IActionResult> CreateVendorQuotation([FromQuery(Name = "rfqid")] String rfqId)
        {
            var svc = new PurchaseBidding(AppUserData);

            var saveResult = await Task.Run<ApiResponse>(() =>
            {
                bool isNew = false;
                var result = new ApiResponse();

                try
                {
                    var RecordId = string.Empty;
                    result.Status.Success = svc.createBidding(rfqId, ref isNew, ref RecordId);

                    if (result.Status.Success)
                    {
                        if (isNew)
                        {
                            result.Status.Message = "The data has been added.";
                        }
                        else
                        {
                            result.Status.Message = "The data has been updated.";
                        }
                        result.Data = new
                        {
                            recordID = RecordId
                        };
                    }
                    else
                    {
                        result.Status.Message = "Record is not saved !";
                    }
                }
                catch (Exception ex)
                {
                    result.Status.Message = ex.Message;
                }

                return result;
            });

            if (saveResult == null) return new EmptyResult();
            else return Ok(saveResult);
        }

        [HttpPost]
        [Route("createbiddingfromrfq")]
        public async Task<IActionResult> CreateVendorQuotationFromRFQ([FromQuery(Name = "rfqid")] String rfqId, [FromBody] string[] listVendorQuotationId)
        {
            var svc = new PurchaseBidding(AppUserData);

            var saveResult = await Task.Run<ApiResponse>(() =>
            {
                bool isNew = false;
                var result = new ApiResponse();

                try
                {
                    var RecordId = string.Empty;
                    result.Status.Success = svc.createBiddingFromRFQ(rfqId, listVendorQuotationId, ref isNew, ref RecordId);

                    if (result.Status.Success)
                    {
                        if (isNew)
                        {
                            result.Status.Message = "The data has been added.";
                        }
                        else
                        {
                            result.Status.Message = "The data has been updated.";
                        }
                        result.Data = new
                        {
                            recordID = RecordId
                        };
                    }
                    else
                    {
                        result.Status.Message = "Record is not saved !";
                    }
                }
                catch (Exception ex)
                {
                    result.Status.Message = ex.Message;
                }

                return result;
            });

            if (saveResult == null) return new EmptyResult();
            else return Ok(saveResult);
        }

        [HttpGet]
        [Route("getItemApproved")]
        public async Task<IActionResult> GetItemApprovedByBiddingId([FromQuery(Name = "id")] String biddingId)
        {
            var svc = new PurchaseBidding(AppUserData);

            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                var record = svc.getItemApproved(biddingId);
                result.Status.Success = (record != null);
                result.Data = record;
                return result;
            });

            if (detailResult == null) return new EmptyResult();
            else return Ok(detailResult);
        }


        [HttpPost]
        [Route("save")]
        public async Task<IActionResult> SaveBidding([FromBody] string[] vendorQuotationIds, [FromQuery] String biddingId)
        {
            var svc = new PurchaseBidding(AppUserData);

            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                result.Status.Success = svc.saveBidding(vendorQuotationIds, biddingId);
                return result;
            });

            if (detailResult == null) return new EmptyResult();
            else return Ok(detailResult);
        }



    }
}
