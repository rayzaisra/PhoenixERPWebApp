using Microsoft.AspNetCore.Http;
using Phoenix.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Phoenix.Data.Services
{
    public interface IUploadMasterTemplateService
    {
        Task<int> AddAsync(TrMediaFileMaster entity);
        Task<TrMediaFileMaster> GetFileType(string id);
    }

    public class UploadMasterTemplateService : IUploadMasterTemplateService
    {
        readonly DataContext context;
        readonly FileService uploadFile;

        public UploadMasterTemplateService(DataContext context, FileService uploadFile)
        {
            this.context = context;
            this.uploadFile = uploadFile;
        }

        private async Task<TrMediaFileMaster> ProcessUpload(TrMediaFileMaster entity)
        {
            var file1 = await uploadFile.Upload(entity.TemplateFile, null);
            if (!string.IsNullOrEmpty(file1))
            {
                entity.FileMasterId = file1;
            }
            return entity;
        }

        public async Task<int> AddAsync(TrMediaFileMaster entity)
        {
            var _model = await ProcessUpload(entity);
            await context.PhoenixAddAsync(_model);
            return await context.SaveChangesAsync();
        }

        public async Task<TrMediaFileMaster> GetFileType(string typeId)
        {
            var model = context.TrMediaFileMasters.Where(x => !(x.IsDeleted ?? false) && (x.TmMediaTypeId == typeId)).OrderByDescending(a=>a.CreatedOn).FirstOrDefault();
            return await GetFile(model);
        }

        private async Task<TrMediaFileMaster> GetFile(TrMediaFileMaster data)
        {
            if (data != null)
            {
                if (data.FileMasterId != null)
                {
                    var a = context.Filemasters.Where(x => x.Id == data.FileMasterId).FirstOrDefault();
                    if (a != null) data.FileMasterName = a.Name;
                }
            }
            else
            {
                data = new TrMediaFileMaster();
            }

            return data;
        }
    }
}
