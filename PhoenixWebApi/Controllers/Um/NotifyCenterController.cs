using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Services.Um;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models.ViewModel;
using PhoenixWebApi.Services;
using Phoenix.Data.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;

namespace PhoenixWebApi.Controllers.Core
{
    [Route("api/notifycenter")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Um")]
    public class NotifyCenterController : Controller
    {
        private IHostingEnvironment environment;
        private readonly INotificationService _repo;
        private readonly INotificationCenter _notify;
        public NotifyCenterController(INotificationService repo, INotificationCenter notify, IHostingEnvironment environment)
        {
            _repo = repo;
            _notify = notify;
            this.environment = environment;
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            var userId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetUserId(HttpContext);
            return Ok(await _repo.GetListNotificationByUserCode(userId));
        }

        [AllowAnonymousRole]
        [HttpPost]
        [Route("UpdateIsSeen")]
        public async Task<IActionResult> UpdateIsSeen([FromBody]CodeNotifyVm data)
        {
            var model = await _repo.Get(data.Codex);
            model.IsSeen = true;
            await _repo.EditAsync(model);
            return Ok(new { msg ="success update seen"});
        }

        private void CaraMemakaiFunctionNotification()
        {
            //instan Interface INotificationCenter in Controller API or Class..like in this controller...

            //call function with overload entity class parameter...
            var model = new Notification();
            //just this propertie, dont need else..
            model.NotificationTypeId = "pm.approval-team";//this is just sample..
            model.UserId = "delegeate to who member";// ....value must be UserAppId
            model.ItemId = "I dont know what this mean";
            model.Module = "MOdule name like HRIS | PM | FINANCE | OTHERS";
            ///Juist that...
            Task.Run(()=> _notify.AddNotification(model));

            //Overload MEthod value parameter...
            Task.Run(() => _notify.AddNotification("Notification Type","UserAppId","ITem Id","Module Name"));
            //Or just this...
            Task.Run(() => _notify.AddNotification("Notification Type", "UserAppId", "Module Name"));


        }

        #region For Dashboard
        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetDsGeneral")]
        public async Task<IActionResult> GetDashboardGeneral()
        {
            var userId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetUserId(HttpContext);
            var empId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(HttpContext);
            return Ok(await _repo.GetDashboardGeneral(userId,empId));
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetDsKanban")]
        public async Task<IActionResult> GetDashboardKanban()
        {
            var userId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetUserId(HttpContext);
            return Ok(await _repo.GetDashboardKanban(userId));
        }


        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetDsSummary")]
        public async Task<IActionResult> GetDsSummary()
        {
            var userId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetUserId(HttpContext);
            return Ok(await _repo.GetDashboardSummary());
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetDsNewPm")]
        public async Task<IActionResult> GetDsNewPm()
        {
            var userId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetUserId(HttpContext);
            return Ok(await _repo.GetDsNewPm(userId));
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetDsHris")]
        public async Task<IActionResult> GetDsHris()
        {
            var userId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetUserId(HttpContext);
            return Ok(await _repo.GetDsHris(userId));
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("Getlinktotimesheetgrid")]
        public async Task<IActionResult> Getlinktotimesheetgrid(string type)
        {
            var userId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(HttpContext);
            return Ok(await _repo.Getlinktotimesheetgrid(type, userId));
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetDsPmApproval")]
        public async Task<IActionResult> GetDsPmApproval()
        {
            var userId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetUserId(HttpContext);
            return Ok(await _repo.GetDashboardPmApproval());
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetDsPmMyTask")]
        public async Task<IActionResult> GetDsPmMyTask()
        {
            var userId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetUserId(HttpContext);
            return Ok(await _repo.GetDashboardPmMyTask(userId));
        }
        #endregion

        #region PIVOT TABLE
        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetListTable")]
        public async Task<IActionResult> GetListTable()
        {
            return Ok(await _repo.GetListOfTable());
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetDataRows/{sc}/{tb}")]
        public async Task<IActionResult> GetRowsTable(string sc,string tb)
        {
            var data = await _repo.GetRowsTable(sc,tb);
            return Ok(data);
        }
        #endregion

        #region Import Data Tools
        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetTemplateImportData/{sc}/{tb}")]
        public async Task<IActionResult> GetTemplateImportData(string sc, string tb)
        {
            var data = await _repo.GetTemplateDataForImportData(sc, tb);
            var pathDownload = Path.Combine(environment.WebRootPath, "TemplateImportData");
            pathDownload = pathDownload + "\\" + tb + ".xlsx";

            if (System.IO.File.Exists(pathDownload)) System.IO.File.Delete(pathDownload);

            DataTableToExcel(data,pathDownload);
            var pathRelative = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;
            var fullpath = pathRelative + "\\TemplateImportData\\" + tb + ".xlsx";
            return Ok(new { path = fullpath });
        }
        [AllowAnonymousRole]
        [HttpPost]
        [Route("importdata")]
        public async Task<IActionResult> ImprtDataExcelToDb([FromForm]VmUploadImportData data)
        {
            var patFile = Path.Combine(environment.WebRootPath, "TemplateImportData");
            patFile = patFile + "\\FileImportData\\" + data.File.FileName;
            var stream = data.File.OpenReadStream();
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                using (FileStream file = new FileStream(patFile, FileMode.Create, FileAccess.Write))
                {
                    ms.WriteTo(file);
                    file.Close();
                    ms.Close();
                }
            }
            var dt = ReadExcel(patFile);
            //Proses insert data to table....
            await _repo.InsertDataFromImportExcel(dt, data.SchemaName, data.TableName);
            // end of proses insert data...

            System.IO.File.Delete(patFile);
            return Ok(await Task.FromResult("import data success"));
        }


        public static void DataTableToExcel(DataTable data, string filePath)
        {
            DataTable table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(data), (typeof(DataTable)));
            var memoryStream = new MemoryStream();

            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Sheet1");

                List<String> columns = new List<string>();
                IRow row = excelSheet.CreateRow(0);
                int columnIndex = 0;

                foreach (System.Data.DataColumn column in table.Columns)
                {
                    columns.Add(column.ColumnName);
                    row.CreateCell(columnIndex).SetCellValue(column.ColumnName);
                    columnIndex++;
                }

                int rowIndex = 1;
                foreach (DataRow dsrow in table.Rows)
                {
                    row = excelSheet.CreateRow(rowIndex);
                    int cellIndex = 0;
                    foreach (String col in columns)
                    {
                        if (string.IsNullOrEmpty(dsrow[col].ToString()))
                            row.CreateCell(cellIndex).SetCellValue("NULL");
                        else
                            row.CreateCell(cellIndex).SetCellValue("-");
                        //row.CreateCell(cellIndex).SetCellValue(dsrow[col].ToString());
                        cellIndex++;
                    }
                    rowIndex++;
                }
                workbook.Write(fs);
                fs.Close();
            }
        }
        static DataTable ReadExcel(string path)
        {
            DataTable dtTable = new DataTable();
            List<string> rowList = new List<string>();
            ISheet sheet;
            using (var stream = new FileStream(path, FileMode.Open))
            {
                stream.Position = 0;
                XSSFWorkbook xssWorkbook = new XSSFWorkbook(stream);
                sheet = xssWorkbook.GetSheetAt(0);
                IRow headerRow = sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;
                for (int j = 0; j < cellCount; j++)
                {
                    ICell cell = headerRow.GetCell(j);
                    if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                    {
                        dtTable.Columns.Add(cell.ToString());
                    }
                }
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.Cells.TrueForAll(d => d.CellType == CellType.Blank)) continue;
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            if (!string.IsNullOrEmpty(row.GetCell(j).ToString()) && !string.IsNullOrWhiteSpace(row.GetCell(j).ToString()))
                            {
                                rowList.Add(row.GetCell(j).ToString());
                            }
                        }
                    }
                    if(rowList.Count>0)
                    dtTable.Rows.Add(rowList.ToArray());
                    rowList.Clear(); 
                }

                stream.Close();
            }
            return dtTable;
        }

        public class VmUploadImportData
        {
            public string SchemaName { get; set; }
            public string TableName { get; set; }
            public IFormFile File { get; set; }
        }
        #endregion
    }
}