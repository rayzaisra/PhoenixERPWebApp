using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.ModelShared;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area(nameof(Finance))]
    public class CurrencyController : Controller
    {
        string url = ApiUrl.CurrencyUrl;
        private readonly ApiClientFactory client;

        public CurrencyController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index(bool isView)
        {
            ViewBag.isView = isView;
            return View();
        }

        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = new Currency();
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<Currency>($"{url}/{Id}");
            }

            model.IsDefault = (model.IsDefault ?? false);
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<Currency>>(url) ?? new List<Currency>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(Currency model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<Currency>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<Currency>(url, model);
            }
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Currency model) => Json(await client.DeleteApiResponse<Currency>($"{url}/{model.Id}"));

        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            var model = await client.Get<Currency>($"{url}/{id}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<FileResult> DownloadTemplate()
        {
            var model = await client.Get<List<Currency>>(url) ?? new List<Currency>();
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFFont myFont = (HSSFFont)workbook.CreateFont();
            myFont.FontHeightInPoints = 11;
            myFont.FontName = "Tahoma";

            // Defining a border
            HSSFCellStyle borderedCellStyleHeader = (HSSFCellStyle)workbook.CreateCellStyle();
            borderedCellStyleHeader.SetFont(myFont);
            borderedCellStyleHeader.BorderLeft = BorderStyle.Thin;
            borderedCellStyleHeader.BorderTop = BorderStyle.Thin;
            borderedCellStyleHeader.BorderRight = BorderStyle.Thin;
            borderedCellStyleHeader.BorderBottom = BorderStyle.Thin;
            borderedCellStyleHeader.VerticalAlignment = VerticalAlignment.Center;
            borderedCellStyleHeader.Alignment = HorizontalAlignment.Center;
            borderedCellStyleHeader.FillForegroundColor = HSSFColor.SkyBlue.Index;
            borderedCellStyleHeader.FillPattern = FillPattern.SolidForeground;

            ISheet sheet = workbook.CreateSheet("Currency");
            //Creat The Headers of the excel
            IRow headerRow = sheet.CreateRow(0);
            headerRow.Height = 500;

            //Create The Actual Cells
            CreateCell(headerRow, 0, "Currency Code", borderedCellStyleHeader);
            CreateCell(headerRow, 1, "Currency Name", borderedCellStyleHeader);
            CreateCell(headerRow, 2, "Currency Symbol", borderedCellStyleHeader);
            CreateCell(headerRow, 3, "Buy", borderedCellStyleHeader);
            CreateCell(headerRow, 4, "Sell", borderedCellStyleHeader);


            HSSFCellStyle borderedCellStyleBody = (HSSFCellStyle)workbook.CreateCellStyle();
            borderedCellStyleBody.SetFont(myFont);
            borderedCellStyleBody.BorderLeft = BorderStyle.Thin;
            borderedCellStyleBody.BorderTop = BorderStyle.Thin;
            borderedCellStyleBody.BorderRight = BorderStyle.Thin;
            borderedCellStyleBody.BorderBottom = BorderStyle.Thin;
            borderedCellStyleBody.VerticalAlignment = VerticalAlignment.Center;

            int defaultrows = 1;
            foreach (var item in model)
            {
                var bodyRow = sheet.CreateRow(defaultrows);
                //Create The Actual Cells
                CreateCell(bodyRow, 0, item.CurrencyCode, borderedCellStyleBody);
                CreateCell(bodyRow, 1, item.CurrencyName, borderedCellStyleBody);
                CreateCell(bodyRow, 2, item.CurrencySymbol, borderedCellStyleBody);
                CreateCell(bodyRow, 3, Convert.ToDouble(item.Buy ?? 0), borderedCellStyleBody);
                CreateCell(bodyRow, 4, Convert.ToDouble(item.Sell ?? 0), borderedCellStyleBody);
                defaultrows++;
            }

            // Auto sized all the affected columns
            int lastColumNum = sheet.GetRow(0).LastCellNum;
            for (int i = 0; i <= lastColumNum; i++)
            {
                sheet.AutoSizeColumn(i);
                GC.Collect();
            }

            MemoryStream output = new MemoryStream();
            workbook.Write(output);
            return File(output.ToArray(), "application/vnd.ms-excel", "Currency.xls");
        }

        private void CreateCell(IRow CurrentRow, int CellIndex, string Value, HSSFCellStyle Style)
        {
            ICell Cell = CurrentRow.CreateCell(CellIndex);
            Cell.SetCellValue(Value);
            Cell.CellStyle = Style;
        }

        private void CreateCell(IRow CurrentRow, int CellIndex, double Value, HSSFCellStyle Style)
        {
            ICell Cell = CurrentRow.CreateCell(CellIndex);
            Cell.SetCellValue(Value);
            Cell.CellStyle = Style;
        }

        [HttpGet]
        public IActionResult Upload()
        {
            var maxFileSize = 5000;
            ViewBag.FileSizeByte = maxFileSize * 1024;//KB
            ViewBag.AllowedExtensions = new[] { ".xls", ".xlsx" };
            ViewBag.FormatExtensions = ".xls,.xlsx";
            ViewBag.FileSizeKB = maxFileSize;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Upload(IFormFile upload)
        {
            var response = await client.PostApiResponse<Currency>($"{url}/upload", null, upload);
            return Json(new { success = response.Success, message = response.Message });
        }
    }
}