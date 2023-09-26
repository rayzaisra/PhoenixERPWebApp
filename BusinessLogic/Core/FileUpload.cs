using System;
using DataAccess;
using DataAccess.PhoenixERP;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Omu.ValueInjecter;
using NLog;
using System.Transactions;
using System.Dynamic;
using System.Linq;
using BusinessLogic.Services.General;
using NPoco;
using DataAccess.PhoenixERP.General;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net.Http.Headers;
using Phoenix.Shared.Core.Areas.General;
using Phoenix.Shared.Core.Areas.General.Dtos;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace BusinessLogic.Core
{

    public partial class FileMaster : FilemasterService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();
        private readonly FileConfig _fileconfig;

        public FileMaster(DataContext AppUserData, FileConfig settings) : base(AppUserData)
        {
            _fileconfig = settings;
        }



        public List<filemasterDTO> Upload(List<DamUpload> files)
        {
            var result = new List<filemasterDTO>();
            try
            {
                long size = files.Sum(f => f.file.Length);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
            }
            return result;
        }
        //public async Task<filemasterDTO> Upload(DamUpload file)
        //{
        //    var result = new filemasterDTO();
        //    var success = await Save(file, (response) => { result = response; });
        //    return result;
        //}

        public async Task<filemasterDTO> Upload(DamUpload file, String entityId, String recordId)
        {
            var result = new filemasterDTO();
            var success = false;
            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    var entity = application_entity.FirstOrDefault("WHERE id=@0", entityId);
                    if (entity == null) throw new Exception($"Entity Id {entityId} not found");
                    //var qry = Sql.Builder.Append("SELECT [dbo].[ufn_get_filemaster_ref](@0) AS total", entityId);
                    //var supported = db.ExecuteScalar<bool>(qry);
                    //if (!supported) throw new Exception($"Please add column filemaster_id to Table {entity.entity_name} ");

                    success = await Save(file, entityId,recordId,(response) => { result = response; });
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return result;
        }


        public List<filemasterDTO> Get(String entityId, String recordId)
        {
            var result = new List<filemasterDTO>();
            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    var entity = application_entity.FirstOrDefault("WHERE id=@0", entityId);
                    if (entity == null) throw new Exception($"Entity Id {entityId} not found");

                    var data = db.Fetch<filemaster>("WHERE application_entity_id=@0 AND record_id=@1", entityId,recordId);
                    if (data == null) throw new Exception($"filemaster entity id {entityId} and record id {recordId} not found ");
                    result = data.Select(x => new filemasterDTO().InjectFrom(x)).Cast<filemasterDTO>().ToList();
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                    appLogger.Debug($"Entity Id {entityId} > Record Id {recordId}");
                    appLogger.Debug(db.LastCommand);
                    result = null;
                    throw;
                }
            }
            return result;
        }

        public bool Remove(String id)
        {
            var result = false;
            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    var data = filemaster.FirstOrDefault("WHERE id=@0", id);
                    if (data == null) throw new Exception($"filemaster id {id} not found ");

                    var path = data.filepath;
                    var filename = data.name;

                    if (File.Exists(Path.Combine(path, filename)))
                    {
                        File.Delete(Path.Combine(path, filename));
                        result = true;
                    }
                    //result &= Context.SaveEntity<filemaster>(data, false);
                    if (result)
                    {
                        db.Delete(data);
                    }
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                    appLogger.Debug(db.LastCommand);
                    throw;
                }
            }
            return result;
        }


        public async Task<bool> Save(DamUpload file,String applicationEntityId, String recordId, Action<filemasterDTO> ActionContinuation = null)
        {
            var success = false;
            try
            {
                var record = new filemaster();
                int? size = (int)file?.file?.Length;
                if (size != null && size > 0)
                {

                    string filePath = _fileconfig.Path;

                    string fullPath = filePath + file.file.FileName;
                    string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
                    string extension = Path.GetExtension(fullPath);
                    string newFullPath = fullPath;
                    string newfilename = file.file.FileName;
                    string mime = MimeTypes.GetMimeType(newfilename);
                    int count = 0;
                    while (File.Exists(newFullPath))
                    {
                        string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                        newfilename = tempFileName + extension;
                        newFullPath = Path.Combine(filePath, tempFileName + extension);
                        appLogger.Debug($"Renew Filename : {newfilename}");
                    }

                    record.application_entity_id = applicationEntityId;
                    record.record_id = recordId;
                    record.filepath = filePath;
                    record.extension = extension;
                    record.mimetype = mime;
                    record.size = size;
                    record.tags = file.tags;
                    record.name = newfilename;

                    using (var stream = new FileStream(newFullPath, FileMode.Create))
                    {
                        try
                        {
                            await file.file.CopyToAsync(stream);
                            success = true;
                            if (success)
                            {
                                Context.SaveEntity<filemaster>(record, true);
                            }
                        }
                        catch (Exception ex)
                        {
                            appLogger.Error(ex);
                            success = false;
                            throw;
                        }

                    }
                }
                var recordDto = new filemasterDTO();
                recordDto.InjectFrom(record);
                ActionContinuation?.Invoke(recordDto);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
            }

            return success;
        }
    }
}
