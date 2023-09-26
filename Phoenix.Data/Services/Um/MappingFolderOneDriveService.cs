using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models.Um;
using Phoenix.Data.Models.ViewModel;
using Phoenix.Shared.Helpers;
using Phoenix.Shared.Libraries.SnowFlake;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public class MappingFolderOneDriveService : IMappingFolderOneDriveService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage SalaryComponent
        /// </summary>
        /// <param name="context">Database context</param>
        public MappingFolderOneDriveService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(TrMappingFolderOneDrive entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params TrMappingFolderOneDrive[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(TrMappingFolderOneDrive entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params TrMappingFolderOneDrive[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(TrMappingFolderOneDrive entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params TrMappingFolderOneDrive[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(TrMappingFolderOneDrive entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(TrMappingFolderOneDrive entity)
        {
            throw new NotImplementedException();
        }

        //public async Task<List<TrMappingFolderOneDrive>> GetBySearch(string key) => await context.TrMappingFolderOneDrives.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.GroupName.Contains(key)).ToListAsync();
        public async Task<List<TrMappingFolderOneDrive>> Get() => await context.TrMappingFolderOneDrives.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();
        public async Task<List<TrMappingFolderOneDrive>> Get(string userId, string groupId)
        {
            var data = await context.TrMappingFolderOneDrives.Where(x => x.RefId.Equals(userId) || x.RefId.Equals(groupId)).ToListAsync();
            return data.Where(x => !(x.IsDeleted ?? false)).ToList();
        }

        public Task<List<TrMappingFolderOneDrive>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<TrMappingFolderOneDrive> Get(string Id) => await context.TrMappingFolderOneDrives.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).AsNoTracking().FirstOrDefaultAsync();

        public Task<int> DeleteAllMappingByItemId(string itemId)
        {
            var commandText = @"UPDATE um.TrMappingFolderOneDrive SET
	                                    IsDeleted = 1,
	                                    DeletedBy = @DeletedBy,
	                                    DeletedOn = GETDATE()
                                    where ItemId = @Id";
            var deletedBy = new SqlParameter("@DeletedBy", context.GetUserId());
            var driveId = new SqlParameter("@Id", itemId);
            return context.Database.ExecuteSqlCommandAsync(commandText, deletedBy, driveId);
        }
        public Task<int> DeleteByItemIdAndRefId(string itemId, string refId)
        {
            var commandText = @"UPDATE um.TrMappingFolderOneDrive SET
	                                    IsDeleted = 1,
	                                    DeletedBy = @DeletedBy,
	                                    DeletedOn = GETDATE()
                                    where ItemId = @Id AND RefId = @RefId";
            var deletedBy = new SqlParameter("@DeletedBy", context.GetUserId());
            var driveId = new SqlParameter("@Id", itemId);
            var referenceId = new SqlParameter("@RefId", refId);
            return context.Database.ExecuteSqlCommandAsync(commandText, deletedBy, driveId, referenceId);
        }
        public async Task<List<VmMapFodAccess>> GetByItemId(string itemid)
        {
            var sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@ItemId",itemid)
            };
            return await context.ExecuteStoredProcedure<VmMapFodAccess>("dbo.uspGet_MappingOneDriveByItemId", sqlParameters);
        }

        public async Task<List<VmFodUserList>> GetUserList(string itemid, string key)
        {
            var sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@key",key),
                new SqlParameter("@ItemId",itemid)
            };
            return await context.ExecuteStoredProcedure<VmFodUserList>("dbo.uspGet_FodUserNotInMapping", sqlParameters);
        }
        public async Task<List<VmFodGroupList>> GetGroupList(string itemid, string key)
        {
            var sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@key",key),
                new SqlParameter("@ItemId",itemid)
            };
            return await context.ExecuteStoredProcedure<VmFodGroupList>("dbo.uspGet_FodGroupNotInMapping", sqlParameters);
        }
        public Task<int> DeleteFilePe(PeFileDto data)
        {
            var commandText = @"UPDATE pm.pe_file SET is_deleted = true where file_path = @filepath)";
            var filePath = new SqlParameter("@filepath", data.file_path);
            return context.Database.ExecuteSqlCommandAsync(commandText, filePath);
        }

        public Task<int> InsertUploadFilePe(PeFileDto data)
        {
            var snowflake = new IdWorker();
            var commandText = @"INSERT INTO pm.pe_file (id,file_name, file_type, file_size, upload_date, file_path, created_by, created_on, is_active, parent_id, tags, mimetype, is_deleted) values (@id, @filename, @filetype, @filesize, @uploaddate, @filepath, @createdby, @createdon, @isactive, @parentid, @tags, @mimetype, @isdeleted)";
            var id = new SqlParameter("@id", snowflake.NextId());
            var filename = new SqlParameter("@filename", data.file_name);
            var filetype = new SqlParameter("@filetype", data.file_type);
            var filesize = new SqlParameter("@filesize", data.file_size);
            var uploaddate = new SqlParameter("@uploaddate", data.upload_date);
            var filepath = new SqlParameter("@filepath", data.file_path);
            var createdby = new SqlParameter("@createdby", Glosing.Instance.EmployeeId);
            var createdon = new SqlParameter("@createdon", DateTime.Now);
            var isactive = new SqlParameter("@isactive", true);
            var isdeleted = new SqlParameter("@isdeleted", false);
            var parentid = new SqlParameter("@parentid", data.parent_id);
            var tags = new SqlParameter("@tags", data.tags);
            var mimetype = new SqlParameter("@mimetype", data.mimetype);

            return context.Database.ExecuteSqlCommandAsync(commandText, id, filename, filetype, filesize, uploaddate, filepath, createdby, createdon, isactive, parentid, tags, mimetype, isdeleted);
        }



        public async Task<VmFodUserList> GetUserUploadFIle(string key, string itemid)
        {
            var sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@key",key),
                new SqlParameter("@ItemId",itemid)
            };
            DataTable result = await context.ExecuteStoredProcedure("dbo.uspGet_UserLastModifiedFile", sqlParameters);
            VmFodUserList data = new VmFodUserList();
            if (result.Rows.Count > 0)
            {
                data.Id = result.Rows[0]["Id"].ToString();
                data.Email = result.Rows[0]["Email"].ToString();
                data.AliasName = result.Rows[0]["AliasName"].ToString();
                data.FullPath = result.Rows[0]["FullPath"].ToString();
            }
            else
            {
                data.Id = "";
                data.Email = "";
                data.AliasName = "";
                data.FullPath = "";
            }

            return data;
        }
        public async Task<List<TrainingSchedule>> GetTrainingSchedule()
        {
            List<TrainingSchedule> resp = new List<TrainingSchedule>();
            try
            {

                //IQueryable<TrainingSchedule> que = from tb1 in context.TrainingShareSession
                //                                   join tb2 in context.TrainingRequisition on tb1.TrainingRequisitionId equals tb2.Id
                //                                   join tb3 in context.Training on tb2.TrainingId equals tb3.Id
                //                                   select new TrainingSchedule()
                //                                   {
                //                                       className = "m-fc-event--warning m-fc-event--solid-primary",
                //                                       description = tb2.Notes,
                //                                       title = tb3.Name,
                //                                       start = tb1.StartDate.ToString(),
                //                                       end = tb1.EndDate.ToString()
                //                                   };
                //resp = que.ToList();

                var x = await context.ExecuteStoredProcedure<TrainingSchedule>("dbo.uspGetTrainingSchedule", null);
                resp = x.ToList();

            }
            catch (Exception ex)
            {

            }
            return resp;
        }

        public async Task<List<VmFileFodUserList>> GetDataShareMyDrive(string shareid)
        {
            var query = await (from q in context.DamFileSharings
                               join r in context.PeFiles on q.file_id equals r.Id
                               join s in context.EmployeeBasicInfos on r.created_by equals s.Id
                               where q.share_to == shareid && q.status == "Approved" && q.is_deleted == false
                               select new VmFileFodUserList()
                               {
                                   AliasName = s.NameEmployee,
                                   Id = s.Id,
                                   Email = s.Email,
                                   FileName = r.file_name,
                                   JobId = q.job_id,
                                   FullPath = q.file_path
                               }).ToListAsync();
            return query;
        }
        public async Task<bool> CreateFolderToDB(string key, string itemid, string parent_entity, string userid)
        {
            var success = false;
            var snowflake = new IdWorker();
            var id = snowflake.NextId();
            var createdBy = userid;
            var sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@id",id),
                new SqlParameter("@key",key),
                new SqlParameter("@path",itemid),
                new SqlParameter("@createdBy",createdBy),
                new SqlParameter("@parentEntity",parent_entity)
            };
            DataTable result = await context.ExecuteStoredProcedure("dbo.uspCreateFolder", sqlParameters);
            VmFodUserList data = new VmFodUserList();
            if (result.Rows.Count > 0)
            {
                success = true;
            }

            return success;
        }

        #region MY DAM
        public Task<int> InsertUploadFileMyDam(MyDamFileDto data)
        {
           
            try
            {
                var snowflake = new IdWorker();
                var commandText = @"INSERT INTO pm.mydam (id,file_name, file_type, file_size, upload_date, file_path, created_by, created_on, is_active, is_deleted, mimetype, parent_id) 
                                values (@id, @filename, @filetype, @filesize, @uploaddate, @filepath, @createdby, @createdon, @isactive, @isdeleted,@mimetype, @parentid)";
                var id = new SqlParameter("@id", snowflake.NextId());
                var filename = new SqlParameter("@filename", data.file_name);
                var filetype = new SqlParameter("@filetype", data.file_type);
                var filesize = new SqlParameter("@filesize", data.file_size);
                var uploaddate = new SqlParameter("@uploaddate", data.upload_date);
                var filepath = new SqlParameter("@filepath", data.file_path);
                var createdby = new SqlParameter("@createdby", data.created_by);
                var createdon = new SqlParameter("@createdon", DateTime.Now);
                var isactive = new SqlParameter("@isactive", true);
                var isdeleted= new SqlParameter("@isdeleted", false);
                var mimetype = new SqlParameter("@mimetype", data.mimetype);
                var parentId = new SqlParameter("@parentid", data.parent_id);
                return context.Database.ExecuteSqlCommandAsync(commandText, id, filename, filetype, filesize, uploaddate, filepath, createdby, createdon, isactive, isdeleted,mimetype, parentId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return Task.FromResult(0);
        }
        #endregion

    }
}

public class PeFileDto
{

    public string Id { get; set; }

    public string file_name { get; set; }

    public string file_path { get; set; }
    public long file_size { get; set; }

    public string file_type { get; set; }
    public int? flag { get; set; }
    public DateTime? upload_date { get; set; }
    public string tags { get; set; }
    public string created_by { get; set; }
    public string mimetype { get; set; }
    public string parent_id { get; set; }
}


public class MyDamFileDto
{
    public string Id { get; set; }
    public string file_name { get; set; }
    public string file_path { get; set; }
    public long file_size { get; set; }
    public string file_type { get; set; }
    public int? flag { get; set; }
    public DateTime? upload_date { get; set; }
    public string tags { get; set; }
    public string created_by { get; set; }
    public string mimetype { get; set; }
    public string parent_id { get; set; }

}
