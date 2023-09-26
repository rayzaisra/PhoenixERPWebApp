using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Finance.Transaction
{
    public interface IOrderReceiptService : IDataService<OrderReceipt>
    {

    }

    public class OrderReceiptService : IOrderReceiptService
    {
        readonly DataContext context;
        readonly FileService uploadFile;

        public OrderReceiptService(DataContext context, FileService uploadFile)
        {
            this.context = context;
            this.uploadFile = uploadFile;
        }

        private async Task<OrderReceipt> ProcessUpload(OrderReceipt entity)
        {
            var file1 = await uploadFile.Upload(entity.fileImage, null);
            if (!string.IsNullOrEmpty(file1))
            {
                entity.UploadImage = file1;
            }
            var file2 = await uploadFile.Upload(entity.fileDoc, null);
            if (!string.IsNullOrEmpty(file2))
            {
                entity.UploadDoc = file2;
            }

            return entity;
        }


        private async Task<OrderReceipt> GetFile(OrderReceipt data)
        {
            if (data.UploadImage != null)
            {
                var file1 = await context.Filemasters.Where(x => x.Id == data.UploadImage).FirstOrDefaultAsync();
                if (file1 != null) data.ImageName = file1.Name;
            }

            if (data.UploadDoc != null)
            {
                var file2 = await context.Filemasters.Where(x => x.Id == data.UploadDoc).FirstOrDefaultAsync();
                if (file2 != null) data.DocName = file2.Name;
            }

            return data;
        }

        public async Task<int> AddAsync(OrderReceipt entity)
        {
            var model = await ProcessUpload(entity);
            model.Id = Guid.NewGuid().ToString();
            model.ReceiptOrderNumber = await TransID.GetTransId(context,"OR", DateTime.Today);
            await context.PhoenixAddAsync(model);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params OrderReceipt[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(OrderReceipt entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params OrderReceipt[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(OrderReceipt entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params OrderReceipt[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(OrderReceipt entity)
        {
            var model = await ProcessUpload(entity);
            context.PhoenixEdit(model);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(OrderReceipt entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<OrderReceipt>> Get()
        {   
            return await context.OrderReceipt.Where(x => x.IsDeleted.Equals(false)).ToListAsync();
        }

        public Task<List<OrderReceipt>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderReceipt> Get(string id)
        {
            var query = await context.OrderReceipt.Where(x => x.IsDeleted.Equals(false) && x.Id == id).FirstOrDefaultAsync();
            return await GetFile(query);
        }
    }
}
