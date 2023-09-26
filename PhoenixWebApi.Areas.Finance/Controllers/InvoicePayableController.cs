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
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Options;
using Phoenix.Shared.Core.Areas.General.Dtos;
using Microsoft.AspNetCore.Hosting.Server;
using BusinessLogic.Finance;

namespace PhoenixWebApi.Areas.Finance.Controllers
{
    [Route("api/finance/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Finance")]
    [ApiController]
    public class InvoicePayableController: ApiBaseController
    {
        FileConfig config;

        public InvoicePayableController(IOptions<FileConfig> _config )
        {
            config = _config.Value;
        }

        [HttpPost]
        [Route("listdatatables")]
        public async Task<IActionResult> ListDataTablesBinder([FromForm, 
            ModelBinder(typeof(DataTableModelBinder))] DataTableRequestNetCore dtRequest)
        {
            var svc = new InvoicePayable(AppUserData);

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
            var svc = new InvoicePayable(AppUserData);

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

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] string[] Ids)
        {
            var svc = new InvoicePayable(AppUserData);
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
        [Route("savetodraft")]
        public async Task<IActionResult> SaveEntity([FromBody] InvoicePayableDTO Data)
        {
            var svc = new InvoicePayable(AppUserData);

            var saveResult = await Task.Run<ApiResponse>(() =>
            {
                bool isNew = false;
                string id = "";
                var result = new ApiResponse();

                try
                {
                    result.Status.Success = svc.SaveInvoice(Data, ref isNew,ref id);

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
                            recordID = id
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
        public async Task<IActionResult> Submited([FromQuery(Name = "RecordId")] String RecordId,[FromBody] InvoicePayableDTO invoiceApproval)
        {
            var service = new InvoicePayable(AppUserData);
            var saveResult = await Task.Run<ApiResponse>(() =>
            {
                bool isNew = false;
                var result = new ApiResponse();
                try
                {
                    result.Status.Success = service.SubmitInvoicePayable(RecordId, invoiceApproval, ref isNew);
                    if (result.Status.Success)
                    {
                        result.Status.Message = "The data has been submited.";
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
            var svc = new InvoicePayable(AppUserData);

            var listResult = await Task.Run<dynamic>(() =>
            {
                dynamic result = null;

                result = svc.Lookup(length: length, optionalFilters: filters, optionalParameters: parameters);

                return result;
            });

            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }

        [HttpGet]
        [Route("nextautonumber")]
        public async Task<IActionResult> NextAutoNumber()
        {
            var svc = new InvoicePayable(AppUserData);

            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                
                try
                {
                    var autoNumber = svc.GetNextAutoNumber();

                    if (autoNumber != null)
                    {
                        result.Status.Success = true;
                        result.Data = autoNumber;
                    }
                    else
                    {
                        result.Status.Message = "Fail to get next auto number";
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

        [HttpPost("{Id}/UploadFiles")]
        public async Task<IActionResult> UploadAttachment([FromForm(Name = "files")]IList<IFormFile> files, [FromForm(Name = "document_type_id")] string DocumentTypeId,string Id)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = config.Path;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    filePath = filePath + formFile.FileName;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            var svc = new InvoicePayableAttachment(AppUserData);
            
            var saveResult = await Task.Run<ApiResponse>(() =>
            {
                bool isNew = false;
                var result = new ApiResponse();
                var Data = new invoice_payable_attachmentDTO();
                Data.file_name = filePath;
                Data.invoice_payable_id = Id;
                Data.financial_document_type_id = DocumentTypeId;
                try
                {
                    result.Status.Success = svc.SaveEntity(Data, ref isNew);

                    if (result.Status.Success)
                    {
                        if (isNew)
                        {
                            result.Status.Message = "The File has been Uploaded.";
                        }
                        else
                        {
                            result.Status.Message = "The File has been Uploaded.";
                        }

                        result.Data = new
                        {
                            recordID = new { count = files.Count, size, filePath }
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

            //return Ok(new { count = files.Count, size, filePath });
        }

        [HttpPost]
        [Route("KendoGrid")]
        public async Task<IActionResult> KendoGrid(CommonTool.KendoUI.Grid.DataRequest dtRequest)
        {
            var Service = new InvoicePayable(AppUserData);
            var listResult = await Task.Run<CommonTool.KendoUI.Grid.DataResponse>(() =>
            {
                return Service.GetListKendoUIGrid(dtRequest);
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }

        [HttpPost]
        [Route("KendoLookup")]
        public async Task<IActionResult> KendoLookup([FromBody] CommonTool.KendoUI.Combobox.ComboboxDataRequest dtRequest)
        {
            var Service = new InvoicePayable(AppUserData);
            var listResult = await Task.Run<dynamic>(() =>
            {
                dynamic result = null;
                result = Service.kendoLookup(dtRequest);
                return result;
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }

    }
}
