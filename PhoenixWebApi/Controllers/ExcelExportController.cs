using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phoenix.ApiExtension.Extensions.GenericExcelExport;
using Phoenix.Data.RestApi;

namespace PhoenixWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelExportController : ControllerBase
    {
        readonly IAbstractDataExport service;

        public ExcelExportController(IAbstractDataExport service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<byte[]> Post([FromBody] ExcelExportDto model)
        {
            //ExcelExportDto model = new ExcelExportDto();
            //var list = new List<TestExport>();
            //list.Add(new TestExport
            //{
            //    Id = 1,
            //    Name = "Test"
            //});
            //model.FileName = "test";
            //model.SheetName = "testSheet";
            var result = service.Export(model.List, model.FileName, model.SheetName);
            var fileBytes = await result.Content.ReadAsByteArrayAsync();
            //return new FileContentResult(b, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { FileDownloadName = fileName };
            //return await Task.FromResult(new FileContentResult(b, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { FileDownloadName = model.FileName + ".xlsx" });
            return fileBytes;
        }
        public IActionResult Get(string path)
        {
            var result = service.ReadExcel(path);
            return Ok(new ApiResponse(result));
        }
    }

    public class ExcelExportDto
    {
        public List<object> List { get; set; }
        public string FileName { get; set; }
        public string SheetName { get; set; }
    }

    public class TestExport
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}