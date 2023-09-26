using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Core;
using BusinessLogic.Services;
using CodeMarvel.Infrastructure.ModelShared;
using DataAccess.PhoenixERP.General;
using DataTables.AspNet.AspNetCore;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Phoenix.Shared.Core.Areas.General.Dtos;
using PhoenixWebApi.Base;

namespace PhoenixWebApi.Controllers.Core
{
    [Route("api/core/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Core")]
    public class FileUploadController : ApiBaseController
    {
        private readonly FileConfig _fileconfig;

        public FileUploadController(IOptions<FileConfig> settings)
        {
            _fileconfig = settings.Value;
        }

        //[HttpPost, DisableRequestSizeLimit]
        //[DisableCors]
        //public async Task<IActionResult> Post([FromQuery] DamUpload file)
        //{
        //    var service = new FileMaster(AppUserData, _fileconfig);
        //    var result = new ApiResponse();
        //    try
        //    {
        //        if (file.file == null)
        //        {
        //            file.file = Request.Form.Files[0];
        //        }
        //        var data = await service.Upload(file);
        //        result.Status.Success = true;
        //        result.Data = data;
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Status.Success = false;
        //        result.Status.Message = ex.Message;
        //        appLogger.Error(ex);
        //    }
        //    return Ok(result);
        //}


        [HttpPost("entity"), DisableRequestSizeLimit]
        [DisableCors]
        public async Task<IActionResult> Post([FromQuery] IFormFile file, [FromQuery] String entityId, [FromQuery] String recordId, [FromQuery] String tags = "")
        {

            var record = new DamUpload();
            var service = new FileMaster(AppUserData, _fileconfig);
            var result = new ApiResponse();
            try
            {
                if (file == null)
                {
                    record.file = Request.Form.Files[0];
                }
                else
                {
                    record.file = file;
                }

                var data = await service.Upload(record, entityId, recordId);
                result.Status.Success = true;
                result.Data = data;
            }
            catch (Exception ex)
            {
                result.Status.Success = false;
                result.Status.Message = ex.Message;
                appLogger.Error(ex);
            }
            return Ok(result);
        }

        [HttpGet, DisableRequestSizeLimit]
        public async Task<IActionResult> GetFile([FromQuery] String id)
        {
            var service = new FileMaster(AppUserData, _fileconfig);
            string mimeType = string.Empty, filename = string.Empty;
            var result = await Task.Run<vw_filemasterDTO>(() =>
            {
                var record = service.Details(id);
                return record;
            });

            if (result == null) return new EmptyResult();
            else
            {
                var ms = new MemoryStream();
                if(!System.IO.File.Exists(Path.Combine(result.filepath, result.name)))return new EmptyResult();
                using (var stream = new FileStream(Path.Combine(result.filepath, result.name), FileMode.Open))
                {
                    try
                    {
                        await stream.CopyToAsync(ms);
                        ms.Position = 0;
                        return File(ms, result.mimetype, result.name);
                    }
                    catch (Exception ex)
                    {
                        appLogger.Error(ex);
                        return new EmptyResult();
                    }
                }
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Remove([FromForm] String id)
        {
            var service = new FileMaster(AppUserData, _fileconfig);
            var result = await Task.Run<ApiResponse>(() =>
            {
                var response = new ApiResponse();
                try
                {
                    response.Status.Success = service.Remove(id);
                }
                catch (Exception ex)
                {
                    response.Status.Message = ex.Message;
                }
                return response;
            });
            if (result == null) return new EmptyResult();
            else return Ok(result);
        }

        //[HttpGet("entity"), DisableRequestSizeLimit]
        //public async Task<IActionResult> GetFileByEntity([FromQuery] String entityId, [FromQuery] String recordId)
        //{
        //    var service = new FileMaster(AppUserData, _fileconfig);
        //    string mimeType = string.Empty, filename = string.Empty;
        //    var result = await Task.Run<filemasterDTO>(() =>
        //    {
        //        var record = service.Get(entityId,recordId);
        //        return record;
        //    });

        //    if (result == null) return new EmptyResult();
        //    else
        //    {
        //        var ms = new MemoryStream();
        //        using (var stream = new FileStream(Path.Combine(result.filepath, result.name), FileMode.Open))
        //        {
        //            try
        //            {
        //                await stream.CopyToAsync(ms);
        //            }
        //            catch (Exception ex)
        //            {
        //                appLogger.Error(ex);
        //            }
        //        }
        //        ms.Position = 0;
        //        return File(ms, result.mimetype, result.name);
        //    }
        //}


        [HttpGet("entity/record"), DisableRequestSizeLimit]
        public async Task<IActionResult> GetRecordByEntity([FromQuery] String entityId, [FromQuery] String recordId)
        {
            var service = new FileMaster(AppUserData, _fileconfig);
            string mimeType = string.Empty, filename = string.Empty;

            var Dataresult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                try
                {
                    var data = service.Get(entityId, recordId);
                    if (data != null)
                    {
                        result.Status.Success = true;
                        result.Data = data;
                    }
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                }
                return result;
            });

            if (Dataresult == null) return new EmptyResult();
            else return Ok(Dataresult);
        }


    }
}