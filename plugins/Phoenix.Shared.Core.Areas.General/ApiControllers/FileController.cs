using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.General.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using System.Threading.Tasks;
using Phoenix.Shared.Core.Areas.General.Dtos;
using System.Net.Http;
using System.Net;
using System.IO;
using System;
using Phoenix.Shared.Core.Entities;
using Phoenix.ApiExtension.Helpers;
using Phoenix.Data.Attributes;
using Phoenix.Data.RestApi;
using Microsoft.Extensions.Options;
using Phoenix.Shared.Core.Areas.General.Dtos.Config;

namespace Phoenix.Api.Areas.General.Controllers
{
    [Area(PhoenixModule.GENERAL)]
    [Route(PhoenixModule.GENERAL_ROUTE_API)]
    [JwtAuthentication]
    [ApiExplorerSettings(GroupName = PhoenixModule.GENERAL)]
    public class FileController : Controller
    {
        private readonly IOptions<MsGraphSetting> _msGraphSetting;
        private IFileServices _service;
        private readonly Phoenix.Data.Services.Um.IManageAzurePortalSetupService _repoAzure;
        public FileController(IFileServices service, Phoenix.Data.Services.Um.IManageAzurePortalSetupService repoAzure, IOptions<MsGraphSetting> msGraphSetting)
        {
            _service = service;
            _repoAzure = repoAzure;
            _msGraphSetting = msGraphSetting;
        }
        [HttpPost]
        public async Task<GeneralResponse<FileMasterDto>> Upload(DamUpload files)
        {
             return await _service.Upload(files);
        }

        [HttpGet("{id}")]
        public GeneralResponseList<FileMasterDto> GetFilemasterByRecordId(string id)
        {
            return  _service.GetFilemasterByRecordId(id);
        }

        [HttpGet("{id}")]
        public GeneralResponseList<PeFileDto> GetFileByRecordId(string id)
        {
            return _service.GetFileByRecordId(id);
        }

        [HttpPost]
        public async Task<GeneralResponse<FileMasterDto>> UploadOneDrive(DamUpload files)
        {
            //var model = _repoAzure.GetOnlyOne().Result;
            var model = new MsGraphSetting()
            {
                clientId = _msGraphSetting.Value.clientId,
                clientSecret = _msGraphSetting.Value.clientSecret,
                objectId = _msGraphSetting.Value.objectId,
                office365Password = _msGraphSetting.Value.office365Password,
                office365Username = _msGraphSetting.Value.office365Username,
                tenantId = _msGraphSetting.Value.tenantId,
                useMsLogin = _msGraphSetting.Value.useMsLogin

            };
            return await _service.UploadOneDrive(files,model);
        }

        [HttpPost]
        public async Task<GeneralResponse<FileMasterDto>> UploadOneDrivePeFolder(UploadFileDto files)
        {
            //var model = _repoAzure.GetOnlyOne().Result;
            var model = new MsGraphSetting()
            {
                clientId = _msGraphSetting.Value.clientId,
                clientSecret = _msGraphSetting.Value.clientSecret,
                objectId = _msGraphSetting.Value.objectId,
                office365Password = _msGraphSetting.Value.office365Password,
                office365Username = _msGraphSetting.Value.office365Username,
                tenantId = _msGraphSetting.Value.tenantId,
                useMsLogin = _msGraphSetting.Value.useMsLogin

            };
            return await _service.UploadOneDrivePeFolder(files, model);
        }
        //[HttpGet("{id}")]
        //public async Task<IActionResult> getfile(string id)
        //{

        //    Filemaster filemaster = _service.getfile(id);

        //    if (filemaster == null)
        //        return Content("filename not present");

        //    var path = filemaster.filepath;

        //    var memory = new MemoryStream();
        //    using (var stream = new FileStream(path, FileMode.Open))
        //    {
        //        await stream.CopyToAsync(memory);
        //    }
        //    memory.Position = 0;
        //    return File(memory, filemaster.mimetype, Path.GetFileName(path));
        //}

        [HttpGet]
       
        public async Task<IActionResult> getfile(string id)
        {
            var model = new MsGraphSetting()
            {
                clientId = _msGraphSetting.Value.clientId,
                clientSecret = _msGraphSetting.Value.clientSecret,
                objectId = _msGraphSetting.Value.objectId,
                office365Password = _msGraphSetting.Value.office365Password,
                office365Username = _msGraphSetting.Value.office365Username,
                tenantId = _msGraphSetting.Value.tenantId,
                useMsLogin = _msGraphSetting.Value.useMsLogin

            };
            var result = await _service.Download(id,model);
           
            return Ok(new ApiResponse(result));
            //return File(result.File, result.MimeType, result.FileName);
        }

        [HttpGet]

        public async Task<IActionResult> getfilePe(string id)
        {
            var model = new MsGraphSetting()
            {
                clientId = _msGraphSetting.Value.clientId,
                clientSecret = _msGraphSetting.Value.clientSecret,
                objectId = _msGraphSetting.Value.objectId,
                office365Password = _msGraphSetting.Value.office365Password,
                office365Username = _msGraphSetting.Value.office365Username,
                tenantId = _msGraphSetting.Value.tenantId,
                useMsLogin = _msGraphSetting.Value.useMsLogin

            };
            var result = await _service.DownloadPe(id, model);

            return Ok(new ApiResponse(result));
            //return File(result.File, result.MimeType, result.FileName);
        }


    }

    
}