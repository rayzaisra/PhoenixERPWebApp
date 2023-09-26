using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Services;
using DataTables.AspNet.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using PhoenixWebApi.Base;
using DataAccess.PhoenixERP.Finance;
using CodeMarvel.Infrastructure.ModelShared;
using Newtonsoft.Json;

namespace PhoenixWebApi.Areas.Finance.Controllers
{
    [Route("api/finance/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Finance")]
    [ApiController]
    public class VendorQuotationController: ApiBaseController
    {
        [HttpPost]
        [Route("listdatatables")]
        public async Task<IActionResult> ListDataTablesBinder([FromForm, 
            ModelBinder(typeof(DataTableModelBinder))] DataTableRequestNetCore dtRequest)
        {
            var svc = new VendorQuotation(AppUserData);

            var listResult = await Task.Run<DataTablesResponseNetCore>(() =>
            {
                return svc.GetListDataTables(dtRequest);
            });

            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }

        [HttpGet]
        [Route("{Id}/details")]
        public async Task<IActionResult> Details(string Id)
        {
            var svc = new VendorQuotation(AppUserData);

            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                var record = svc.Details(Id);

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

        [HttpGet]
        [Route("getlistdetails")]
        public async Task<IActionResult> GetListDetails([FromQuery(Name = "rfq_id")] String rfq_id)
        {// di panggil dari page vendor selection
            var svc = new VendorQuotation(AppUserData);
       
            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                var record = svc.GetListVendorQuotation(rfq_id);

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

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] string[] Ids)
        {
            var svc = new VendorQuotation(AppUserData);
            appLogger.Debug("Delete " + JsonConvert.SerializeObject(Ids));

            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();

                svc.Delete(Ids, msg => result.Status.Message = msg);
                result.Status.Success = true;

                return result;
            });

            if (detailResult == null) return new EmptyResult();
            else return Ok(detailResult);
        }


        [HttpPost]
        [Route("saveasdraft")]
        public async Task<IActionResult> SaveEntity([FromBody] vendor_quotation_detailsDTO Data)
        {
            var svc = new VendorQuotation(AppUserData);

            var saveResult = await Task.Run<ApiResponse>(() =>
            {
                bool isNew = false;
                var result = new ApiResponse();

                try
                {
                    result.Status.Success = svc.SaveVendorQuotation(Data, ref isNew);

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
                            recordID = Data.VendorQuotation.id,
                            vendorQuotationNumber = Data.VendorQuotation.vendor_quotation_number
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
        [Route("submited")]
        public async Task<IActionResult> Submitted([FromQuery(Name = "RecordId")] String RecordId)
        {
            var service = new VendorQuotation(AppUserData);
            var saveResult = await Task.Run<ApiResponse>(() =>
            {
                bool isNew = false;
                var result = new ApiResponse();
                try
                {
                    result.Status.Success = service.SubmitVq(RecordId, ref isNew);
                    if (result.Status.Success)
                    {
                        result.Status.Message = "The data has been submited.";
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
        [Route("createpo")]
        public async Task<IActionResult> CreatePO([FromQuery(Name = "vendorquotationid")] String rfqId)
        {
            var svc = new VendorQuotation(AppUserData);

            var saveResult = await Task.Run<ApiResponse>(() =>
            {
                bool isNew = false;
                var result = new ApiResponse();

                try
                {
                    result = svc.createPO(rfqId, ref isNew);

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
        [Route("createpofrombidding")]
        public async Task<IActionResult> CreatePOFromBidding([FromQuery(Name = "purchasebiddingid")] String biddingId,[FromBody]List<string> ListVQDetail_id)
        {
            var svc = new VendorQuotation(AppUserData);

            var saveResult = await Task.Run<ApiResponse>(() =>
            {
                bool isNew = true;
                var result = new ApiResponse();

                try
                {
                    result.Status.Success = svc.createPoFromBidding(biddingId, ListVQDetail_id, ref isNew);

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
        [Route("lookup")]
        public async Task<IActionResult> Lookup([FromQuery(Name = "length")] int length = 100,
           [FromQuery(Name = "filters")] string filters = null, 
           [FromQuery(Name = "parameters")] string parameters = null)
        {
            var svc = new VendorQuotation(AppUserData);

            var listResult = await Task.Run<dynamic>(() =>
            {
                dynamic result = null;

                result = svc.Lookup(length: length, optionalFilters: filters, optionalParameters: parameters);

                return result;
            });

            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }

        [HttpPost]
        [Route("KendoGrid")]
        public async Task<IActionResult> KendoGrid(CommonTool.KendoUI.Grid.DataRequest dtRequest)
        {
            var Service = new VendorQuotation(AppUserData);
            var listResult = await Task.Run<CommonTool.KendoUI.Grid.DataResponse>(() =>
            {
                return Service.GetListKendoUIGrid(dtRequest);
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }

        //[HttpPost]
        //[Route("KendoWithDetailGrid")]
        //public async Task<IActionResult> GetListWithDetailKendoGrid(CommonTool.KendoUI.Grid.DataRequest dtRequest)
        //{
        //    var Service = new VendorQuotation(AppUserData);
        //    var listResult = await Task.Run<CommonTool.KendoUI.Grid.DataResponse>(() =>
        //    {
        //        return Service.GetListKendoWithDetailUIGrid(dtRequest);
        //    });
        //    if (listResult == null) return new EmptyResult();
        //    else return Ok(listResult);
        //}
    }
}
