using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Services;
using BusinessLogic.Finance;
using DataTables.AspNet.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using PhoenixWebApi.Base;
using DataAccess.PhoenixERP.Finance;
using CodeMarvel.Infrastructure.ModelShared;
using Newtonsoft.Json;
using CommonTool.JEasyUI.DataGrid;

namespace PhoenixWebApi.Areas.Finance.Controllers
{
    //[Route("api/finance/[controller]")]
    //[ApiExplorerSettings(IgnoreApi = false, GroupName = @"Finance")]
    //[ApiController]
    //public class BankPaymentController: ApiBaseController
    //{
    //    [HttpPost]
    //    [Route("listdatatables")]
    //    public async Task<IActionResult> ListDataTablesBinder([FromForm, ModelBinder(typeof(DataTableModelBinder))] DataTables.AspNet.AspNetCore.DataTableRequestNetCore dtRequest)
    //    {
    //        var Service = new BankPayment(AppUserData);
    //        var listResult = await Task.Run<DataTablesResponseNetCore>(() =>
    //        {
    //            return Service.GetListDataTables(dtRequest);
    //        });
    //        if (listResult == null) return new EmptyResult();
    //        else return Ok(listResult);
    //    }

    //    [HttpGet]
    //    [Route("{Id}/details")]
    //    public async Task<IActionResult> Details(string Id)
    //    {
    //        var service = new BankPayment(AppUserData);
    //        var detailResult = await Task.Run<ApiResponse>(() =>
    //        {
    //            var result = new ApiResponse();
    //            var record = service.Details(Id);
    //            try
    //            {
    //                if (record != null)
    //                {
    //                    result.Status.Success = true;
    //                    result.Data = record;
    //                }
    //                else
    //                {
    //                    result.Status.Message = "Record not found";
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                result.Status.Message = ex.Message;
    //            }
    //            return result;
    //        });
    //        if (detailResult == null) return new EmptyResult();
    //        else return Ok(detailResult);
    //    }

    //    [HttpDelete]
    //    [Route("delete")]
    //    public async Task<IActionResult> Delete([FromBody] string[] Ids)
    //    {
    //        var service = new BankPayment(AppUserData);
    //        appLogger.Debug("Delete " + JsonConvert.SerializeObject(Ids));

    //        var detailResult = await Task.Run<ApiResponse>(() =>
    //        {
    //            var result = new ApiResponse();
    //            service.Delete(Ids, msg => result.Status.Message = msg);
    //            result.Status.Success = true;
    //            return result;
    //        });
    //        if (detailResult == null) return new EmptyResult();
    //        else return Ok(detailResult);
    //    }


    //    [HttpPost]
    //    [Route("savetodraft")]
    //    public async Task<IActionResult> SaveEntity([FromBody] BankPaymentDetailsDTO Data)
    //    {
    //        var svc = new BankPayment(AppUserData);

    //        var saveResult = await Task.Run<ApiResponse>(() =>
    //        {
    //            bool isNew = false;
    //            var result = new ApiResponse();

    //            try
    //            {
    //                result.Status.Success = svc.SaveTransaction(Data, ref isNew);

    //                if (result.Status.Success)
    //                {
    //                    if (isNew)
    //                    {
    //                        result.Status.Message = "The data has been added.";
    //                    }
    //                    else
    //                    {
    //                        result.Status.Message = "The data has been updated.";
    //                    }

    //                    result.Data = new {
    //                        recordID = Data.bank_payment.id,
    //                        paymentNumber = Data.bank_payment.payment_number
    //                    };
    //                }
    //                else
    //                {
    //                    result.Status.Message = "Record is not saved !";
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                result.Status.Message = ex.Message;
    //            }

    //            return result;
    //        });

    //        if (saveResult == null) return new EmptyResult();
    //        else return Ok(saveResult);
    //    }

    //    [HttpGet]
    //    [Route("lookup")]
    //    public async Task<IActionResult> Lookup([FromQuery(Name = "length")] int length = 100,
    //       [FromQuery(Name = "filters")] String filters = null, [FromQuery(Name = "parameters")] String parameters = null)
    //    {
    //        var svc = new BankPayment(AppUserData);

    //        var listResult = await Task.Run<dynamic>(() =>
    //        {
    //            dynamic result = null;
    //            result = svc.Lookup(length: length, optionalFilters: filters, optionalParameters: parameters);
    //            return result;
    //        });

    //        if (listResult == null) return new EmptyResult();
    //        else return Ok(listResult);
    //    }


    //    [HttpPost]
    //    [Route("transactiondetail/dgtable")]
    //    public async Task<IActionResult> DataGridTable([FromForm] DgRequest dgRequest, [FromQuery(Name = "bankpaymentid")] String BankPaymentID)
    //    {
    //        var Service = new BankPayment(AppUserData);
    //        var listResult = await Task.Run<DgResponse>(() =>
    //        {
    //            return Service.GetDataGridEasyUI(dgRequest, BankPaymentID);
    //        });
    //        if (listResult == null) return new EmptyResult();
    //        else return Ok(listResult);
    //    }

    //}
}
