using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Services;
using DataTables.AspNet.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using PhoenixWebApi.Base;
using DataAccess.PhoenixERP.Finance;
using CodeMarvel.Infrastructure.ModelShared;
using CommonTool.JEasyUI.DataGrid;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

namespace PhoenixWebApi.Areas.Finance.Controllers
{
    [Route("api/finance/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Finance")]
    [ApiController]
    public class AccountController: ApiBaseController
    {
        private readonly IHostingEnvironment hostingEnvironment;
        public AccountController(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [Route("dgtable")]
        public async Task<IActionResult> DataGridTable([FromForm] DgRequest dgRequest, [FromQuery(Name = "legal_entity_id")] String LegalEntityId)
        {
            var Service = new Account(AppUserData);
            appLogger.Debug(string.Format("dtRequest : {0} ", Newtonsoft.Json.JsonConvert.SerializeObject(dgRequest)));
           
            var listResult = await Task.Run<DgResponse>(() =>
            {
                return Service.GetDataGridEasyUI(dgRequest, LegalEntityId);
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }
        [HttpPost]
        [Route("{legalEntityId}/coaBalanceKendoGrid")]
        public async Task<IActionResult> GetDataCoaBalanceKendoGridBinder([FromBody] CommonTool.KendoUI.Grid.DataRequest dtRequest,String legalEntityId)
        {
            var Service = new Account(AppUserData);
            appLogger.Debug(Newtonsoft.Json.JsonConvert.SerializeObject(dtRequest));
            var listResult = await Task.Run<CommonTool.KendoUI.Grid.DataResponse>(() =>
            {
                return Service.GetListCoaBalanceKendoUIGrid(dtRequest,legalEntityId);
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }

        [HttpPost]
        [Route("KendoLookup")]
        public async Task<IActionResult> KendoLookup([FromBody] CommonTool.KendoUI.Combobox.ComboboxDataRequest dtRequest)
        {
            var Service = new Account(AppUserData);
            var listResult = await Task.Run<dynamic>(() =>
            {
                dynamic result = null;
                result = Service.kendoLookup(dtRequest);
                return result;
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }

        [HttpPost]
        [Route("{legalEntityId}/SyncronizeAccount")]
        public async Task<IActionResult> SyncronizeAccount(String legalEntityId)
        {
            var Service = new Account(AppUserData);
            var listResult = await Task.Run<bool>(() =>
            {
                return Service.SyncronizeAccount(legalEntityId);
            });
            if (listResult == false) return new EmptyResult();
            else return Ok(listResult);
        }


        [HttpPost]
        [Route("{legalEntityId}/CreateCoaBalance")]
        public async Task<IActionResult> CreateCoaBalance(String legalEntityId,[FromQuery]DateTime transactionDate)
        {
            var Service = new Account(AppUserData);
            var listResult = await Task.Run<bool>(() =>
            {
                return Service.CreateCoaBalance(legalEntityId, transactionDate);
            });
            if (listResult == false) return new EmptyResult();
            else return Ok(listResult);
        }


        [HttpPost]
        [Route("listdatatables")]
        public async Task<IActionResult> ListDataTablesBinder([FromForm, ModelBinder(typeof(DataTableModelBinder))] DataTables.AspNet.AspNetCore.DataTableRequestNetCore dtRequest)
        {
            var Service = new Account(AppUserData);
            var listResult = await Task.Run<DataTablesResponseNetCore>(() =>
            {
                return Service.GetListDataTables(dtRequest);
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }

        [HttpGet]
        [Route("{Id}/details")]
        public async Task<IActionResult> Details(string Id)
        {
            var service = new Account(AppUserData);
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

        [HttpGet]
        [Route("{Id}/sharingalloaction")]
        public async Task<IActionResult> DetailsSharingAllocation(string Id)
        {
            var service = new Account(AppUserData);
            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                try
                {
                    var record = service.DetailsCostSharingByAccount(Id);
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
            var service = new Account(AppUserData);

            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                service.Delete(Ids, msg => result.Status.Message = msg);
                result.Status.Success = true;
                return result;
            });
            if (detailResult == null) return new EmptyResult();
            else return Ok(detailResult);
        }

        [HttpPost]
        [Route("CoaBalance/SingleUpdate")]
        public async Task<IActionResult> SingleUpdateCoaBalance([FromBody] vw_coa_balanceDTO record)
        {
            var Service = new Account(AppUserData);
            var listResult = await Task.Run<bool>(() =>
            {
                return Service.SingleUpdateCoaBalance(record);
            });
            return Ok(listResult);
        }

        [HttpPost]
        [Route("save")]
        public async Task<IActionResult> SaveEntity([FromBody] accountDTO Data)
        {
            var service = new Account(AppUserData);
            var saveResult = await Task.Run<ApiResponse>(() =>
            {
                bool isNew = false;
                var result = new ApiResponse();
                try
                {
                    result.Status.Success = service.SaveEntity(Data, ref isNew);
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
                        result.Data = new {
                            recordID = Data.id
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

        [HttpDelete]
        [Route("removeallocationsharing")]
        public async Task<IActionResult> RemoveAllocationSharing([FromQuery(Name = "accountid")] String AccountId)
        {
            var service = new Account(AppUserData);

            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                try
                {
                    result.Status.Success = service.RemoveAllocationSharing(AccountId); ;
                }
                catch (Exception ex)
                {
                    result.Status.Success = false;
                    result.Status.Message = ex.Message;
                }


                return result;
            });
            if (detailResult == null) return new EmptyResult();
            else return Ok(detailResult);
        }

        [HttpPost]
        [Route("createcostsharing")]
        public async Task<IActionResult> CreateAccountCostSharing([FromQuery(Name ="accountid")] String AccountId,
            [FromBody] account_cost_sharingDTO Data)
        {
            var service = new Account(AppUserData);
            var saveResult = await Task.Run<ApiResponse>(() =>
            {
                bool isNew = false;
                var result = new ApiResponse();
                try
                {
                    result.Status.Success = service.SaveCostSharingAllocation(AccountId, Data, ref isNew);
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
                            account_id = AccountId,
                            cost_sharing_allocation_id= Data.cost_sharing_allocation.id
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
        [Route("lookup")]
        public async Task<IActionResult> Lookup([FromQuery(Name = "length")] int length = 100,
         [FromQuery(Name = "filters")] string filters = null, [FromQuery(Name = "parameters")] string parameters = null)
        {
            var Service = new Account(AppUserData);
            var listResult = await Task.Run<dynamic>(() =>
            {
                dynamic result = null;
                result = Service.Lookup(length: length, optionalFilters: filters, optionalParameters: parameters);
                return result;
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }

        [HttpGet]
        [Route("LevelsLookup")]
        public async Task<IActionResult> LevelsLookup()
        {
            var Service = new Account(AppUserData);
            var listResult = await Task.Run<dynamic>(() =>
            {
                dynamic result = null;
                result = Service.AccountLevelsLookup();
                return result;
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }

        protected string GetUploadPathAndFilename(string filename)
        {
            return hostingEnvironment.WebRootPath + "\\files\\upload\\" + filename;
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> Upload()
        {
            var service = new Account(AppUserData);
            var saveResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();

                try
                {
                    var legal_entity_id = "";
                    Microsoft.Extensions.Primitives.StringValues legalEntityId;
                    if (Request.Form.TryGetValue("legal_entity_id", out legalEntityId))
                    {
                        if(legalEntityId.Count > 0)
                        {
                            legal_entity_id = legalEntityId[0];
                        }
                    }

                    long fileSize = 0;
                    string fileName = null;
                    string mimeType = null;
                    if (Request.Form.Files.Count > 0)
                    {
                        //Uploaded file
                        foreach(var file in Request.Form.Files)
                        {
                            //Use the following properties to get file's name, size and MIMEType
                            fileSize = file.Length;
                            fileName = GetUploadPathAndFilename(file.FileName);
                            mimeType = file.ContentType;

                            using (var inputStream = new FileStream(fileName, FileMode.Create))
                            {
                                try
                                {
                                    // read file to stream and save
                                    file.CopyTo(inputStream);                                    
                                }
                                catch (Exception ex)
                                {
                                    appLogger.Error(ex.ToString());
                                    result.Status.Message = ex.Message;
                                }
                            }

                            FileInfo fi = new FileInfo(fileName);
                            if (fi.Exists)
                            {
                                using (var inputStream = new FileStream(fileName, FileMode.Open))
                                {
                                    try
                                    {
                                        ISheet sheet = null;
                                        if (Path.GetExtension(fileName).ToLower() == ".xls")
                                        {
                                            HSSFWorkbook hssfwb = new HSSFWorkbook(inputStream);
                                            sheet = hssfwb.GetSheetAt(0);
                                        }
                                        else if (Path.GetExtension(fileName).ToLower() == ".xlsx")
                                        {
                                            XSSFWorkbook hssfwb = new XSSFWorkbook(inputStream);
                                            sheet = hssfwb.GetSheetAt(0);
                                        }

                                        if(sheet != null)
                                        {
                                            var header = new Dictionary<string, int>();
                                            var newAccounts = new List<Dictionary<int, string>>();

                                            //Loop the records upto filled row  
                                            for (int row = 0; row <= sheet.LastRowNum; row++) 
                                            {
                                                //null is when the row only contains empty cells   
                                                if (sheet.GetRow(row) != null) 
                                                {
                                                    IRow xrow = sheet.GetRow(row);
                                                    // first row is header
                                                    if (row == 0)
                                                    {
                                                        for(int col = 0; col < xrow.LastCellNum; col++)
                                                        {
                                                            string value = xrow.GetCell(col).StringCellValue;
                                                            header.Add(value, col);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        var newAccount = new Dictionary<int, string>();
                                                        for (int col = 0; col < xrow.LastCellNum; col++)
                                                        {
                                                            string value = xrow.GetCell(col).StringCellValue;
                                                            newAccount.Add(col, value);
                                                        }

                                                        newAccounts.Add(newAccount);
                                                    }
                                                }
                                            }

                                            var errorMessage = "";
                                            result.Status.Success = 
                                                service.ImportData(legal_entity_id, header, newAccounts, out errorMessage);
                                            if (!result.Status.Success)
                                            {
                                                result.Status.Message = errorMessage;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        appLogger.Error(ex.ToString());
                                        result.Status.Message = ex.Message;
                                    }
                                }

                                result.Status.Success = true;
                            }

                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Status.Message = ex.ToString();
                }

                return result;
            });

            if (saveResult == null) return new EmptyResult();
            else return Ok(saveResult);
        }

        [HttpGet]
        [Route("download")]
        public async Task<IActionResult> Download([FromQuery(Name = "LegalEntityId")] string LegalEntityId,
            [FromQuery(Name = "FileFormat")] string FileFormat)
        {
            var service = new Account(AppUserData);            

            var fileStream = await Task.Run<Stream>(() =>
            {
                MemoryStream stream = null;
                Dictionary<string, int> header;
                List<Dictionary<int, string>> data;
                string errorMessage;

                if (service.ExportData(LegalEntityId, out header, out data, out errorMessage))
                {
                    ISheet sheet = null;
                    HSSFWorkbook hssfwb = null;
                    XSSFWorkbook xssfwb = null;

                    var fileFormat = string.IsNullOrEmpty(FileFormat) ? "xlsx" : FileFormat;
                    if (fileFormat.ToLower() == "xls")
                    {
                        hssfwb = new HSSFWorkbook();
                        sheet = hssfwb.CreateSheet("Accounts");
                    }
                    else if (fileFormat.ToLower() == "xlsx")
                    {
                        xssfwb = new XSSFWorkbook();
                        sheet = xssfwb.CreateSheet("Accounts");
                    }

                    if(sheet != null)
                    {
                        var rowIndex = 0;

                        #region Add header

                        var row = sheet.CreateRow(rowIndex);
                        foreach(var hdr in header)
                        {
                            row.CreateCell(hdr.Value).SetCellValue(hdr.Key);
                            rowIndex++;
                        }                        

                        #endregion

                        #region Add rows

                        foreach(var acc in data)
                        {
                            row = sheet.CreateRow(rowIndex);

                            foreach (var c in acc)
                            {
                                row.CreateCell(c.Key).SetCellValue(c.Value);
                            }

                            rowIndex++;
                        }

                        #endregion

                        if(hssfwb != null)
                        {
                            stream = new MemoryStream();
                            hssfwb.Write(stream);
                        }
                        else if (xssfwb != null)
                        {
                            stream = new MemoryStream();
                            xssfwb.Write(stream);
                        }
                    }
                }

                return stream;
            });

            if (fileStream == null) return new EmptyResult();
            else return File(fileStream, "application/octet-stream", string.Format("Accounts.{0}", FileFormat));
        }
    }
}
