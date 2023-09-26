using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Helpers;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core;
using System.IO;
using Phoenix.Data.MsGraphExtention;

namespace Phoenix.Data
{

    public interface IMyDamService : IDataService<MyDam>
    {
        //Task<List<PcaTask>> GetTaskList(string pcaId);
        //Task<int> ApproveAsync(Pca entity);
        //Task<RequestForQuotation> RFQAsync(Pca entity);
        //Task<dynamic> GetClientByJobId(string jobId);
        //Task<List<Pca>> GetList(string status);
        Task<MyDam> ApproveAsync(MyDam model);
        Task<List<MyDam>> GetWaitingApproval();
        Task<int> AddCustomAsync(RequestMyDam model);
        Task<List<MyDam>> GetByFilename(string filename);
        Task<List<MyDam>> GetFolderName();
        Task<int> MoveFile(RequestMyDam model);
        Task<Phoenix.Shared.Core.Entities.PeFileFolder> GetFolderById(string id);
        Task<int> DeleteFolderPe(string id);
        Task<List<MyDam>> GetListFileNameBasedOnPathFolder(string path);
    }
    public class MyDamService : IMyDamService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly Services.Um.IManageMenuService menuService;
        readonly StatusLogService log;
        readonly FileService uploadFile;
        private readonly Phoenix.Data.Services.Um.IManageAzurePortalSetupService _repoAzure;

        public MyDamService(DataContext context, GlobalFunctionApproval globalFcApproval, Services.Um.IManageMenuService menuService, StatusLogService log, Phoenix.Data.Services.Um.IManageAzurePortalSetupService repoAzure, FileService uploadFile)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
            this.log = log;
            this.uploadFile = uploadFile;
            _repoAzure = repoAzure;
        }

        public async Task<Phoenix.Shared.Core.Entities.PeFileFolder> GetFolderById(string id) {
            return await context.peFileFolders.Where(x => x.Id == id).FirstOrDefaultAsync();

        }

        public async Task<List<MyDam>> GetListFileNameBasedOnPathFolder(string path) {
            return await context.MyDam.Where(x => x.FilePath.StartsWith(path)).ToListAsync();
        }

        public async Task<int> DeleteFolderPe(string id) { 
            var entity = await context.peFileFolders.Where(x => x.Id == id).FirstOrDefaultAsync();
            entity.is_deleted = true;
            entity.deleted_by = Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
            entity.deleted_on = DateTime.Now;
            context.PhoenixEdit(entity);
            var save = await context.SaveChangesAsync();
            return save;
        }

        public async Task<MyDam> ApproveAsync(MyDam model)
        {
            MyDamApproval DataMyDamApproval = new MyDamApproval();
            var remarks = "";
            if (model.Status == 3)
            {
                remarks = "Data has been agreed to be accepted.";
            }
            else
            {
                remarks = "Data has been agreed to be rejected.";
            }
            var entity = await context.MyDamApproval.AsNoTracking().Where(x => x.MyDamId == model.Id).FirstOrDefaultAsync();
            entity.Status = model.Status;
            context.PhoenixEdit(entity);
            var save = await context.SaveChangesAsync();

            await globalFcApproval.ProcessApproval<MyDamApproval>(model.Id, model.Status, remarks, entity);
            await log.AddAsync(new StatusLog() { TransactionId = model.Id, Status = Phoenix.Data.Models.StatusTransaction.StatusName(model.Status), Description = remarks });

            var modeltrapp = await context.TrTemplateApprovals.Where(x => x.RefId == model.Id).FirstOrDefaultAsync();
            if (modeltrapp != null)
            {
                if (modeltrapp.StatusApprovedDescription == Phoenix.Data.Models.StatusTransaction.StatusName(3))
                {
                    model.Status = 3;
                }
                else
                {
                    model.Status = 2;
                }
            }
            else
            {
                model.Status = 2;
            }
            return model;
        }

        public Task<int> AddAsync(MyDam entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> MoveFile(RequestMyDam model)
        {
            var result = 1;
            string[] data = model.SelectedId.Split(new string[] { "," }, StringSplitOptions.None);
            var newFolderPath = await context.peFileFolders.Where(x => x.parent_id == "mydam" && x.Id == model.FolderId).Select(x => x.folder_path).FirstOrDefaultAsync();
            foreach (var item in data)
            {
                var entity = await context.MyDam.AsNoTracking().Where(x => x.Id == item).FirstOrDefaultAsync();
               
                var modelAzure = _repoAzure.GetOnlyOne().Result;
                var graph = new MsGraph(modelAzure.ClientId, modelAzure.TenantId);
                var authResult = await graph.GetAuthentication(modelAzure.EmailAccount, modelAzure.SecurityPword);
                var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);
                string NewDirectory = newFolderPath;
                string oldDirectoryWithFileName = entity.FilePath;
                var filename = entity.FileName;
                var Move = await MsGraphExtension.MoveFolder(graphClient, NewDirectory, filename, oldDirectoryWithFileName);

                if (!Move.Success)
                {
                    return 0;
                }
                else
                {
                    entity.FilePath = newFolderPath + "/" + entity.FileName;
                    context.PhoenixEdit(entity);
                    await context.SaveChangesAsync();
                }
            }

            return result;
        }

        public async Task<int> AddCustomAsync(RequestMyDam model)
        {
            var result = 1;
            MyDamApproval DataMyDamApproval = new MyDamApproval();
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                var isApprove = true;
                string[] data = model.SelectedId.Split(new string[] { "," }, StringSplitOptions.None);

                foreach (var item in data)
                {
                    var entity = await context.MyDam.AsNoTracking().Where(x => x.Id == item).FirstOrDefaultAsync();
                    DataMyDamApproval.Id = Guid.NewGuid().ToString();
                    DataMyDamApproval.Status = model.Status;
                    DataMyDamApproval.MyDamId = item;
                    DataMyDamApproval.RequestBy = model.RequestBy;
                    DataMyDamApproval.RequestDate = model.RequestDate;
                    DataMyDamApproval.IsActive = true;
                    DataMyDamApproval.IsDeleted = false;
                    DataMyDamApproval.CreatedBy = model.RequestBy;
                    DataMyDamApproval.CreatedOn = model.RequestDate;
                    await context.PhoenixAddAsync(DataMyDamApproval);
                    await context.SaveChangesAsync();

                    var approval = await context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == entity.Id).ToListAsync();
                    try
                    {
                        if (approval.Count > 0)
                        {
                            var userApproval = await context.TrUserApprovals.AsNoTracking().Where(x => approval.Any(y => y.Id == x.TrTempApprovalId)).ToListAsync();
                            var conditionUserApproval = await context.TrConditionUserApprovals.AsNoTracking().Where(x => userApproval.Any(y => y.Id == x.TrUserApprovalId)).ToListAsync();

                            context.TrConditionUserApprovals.RemoveRange(conditionUserApproval);
                            context.TrUserApprovals.RemoveRange(userApproval);
                            context.TrTemplateApprovals.RemoveRange(approval);
                        }

                        var dataMenu = await menuService.GetByUniqueName(Data.Models.MenuUnique.MyDam).ConfigureAwait(false);
                        var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
                        var vm = new Data.Models.ViewModel.TransApprovalHrisVm()
                        {
                            IdTemplate = dataTempate.Id,
                            MenuId = dataMenu.Id,
                            RefId = item,
                            DetailLink = $"{Data.Models.ApprovalLink.MyDam}?Id={item}",
                            NotificationType = Phoenix.Data.Models.NotificationTypeCode.MyDamNotification,
                            Module = Phoenix.Data.Models.NotificationModuleType.ModulePM
                        };

                        var subGroupId = Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                        var divisionId = Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                        var employeeId = Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);

                        isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);
                        
                        await log.AddAsync(new Phoenix.Data.Models.StatusLog()
                        {
                            TransactionId = entity.Id,
                            Status = Phoenix.Data.Models.StatusTransaction.StatusName(Phoenix.Data.Models.StatusTransaction.WaitingApproval),
                            Description = "Process submit request"
                        });

                        
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }

                if (isApprove)
                {
                    var save = await context.SaveChangesAsync();
                    transaction.Commit();
                    result = save;
                }
                else
                {
                    transaction.Rollback();
                    throw new Exception("please check the approval template that will be processed");
                }
                return result;

            }
        }

        public Task<int> AddRangeAsync(params MyDam[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(MyDam entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params MyDam[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(MyDam entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params MyDam[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditAsync(MyDam entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditRangeAsync(MyDam entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MyDam>> Get()
        {
            List<MyDam> result = new List<MyDam>();
            var query = await (from q in context.MyDam
                               join r in context.MyDamApproval on q.Id equals r.MyDamId into lj1
                               from qr in lj1.DefaultIfEmpty()
                               join s in context.EmployeeBasicInfos on q.CreatedBy equals s.Id
                               join t in context.JobDetail on q.ParentId equals t.Id into lj2
                               from qt in lj2.DefaultIfEmpty()
                               join u in context.ClientBrief on qt.ClientBriefId equals u.Id into lj3
                               from tu in lj3.DefaultIfEmpty()
                               join v in context.Companys on tu.ClientId equals v.Id into lj4
                               from tuv in lj4.DefaultIfEmpty()
                               where !(q.IsDeleted ?? false)
                               select new MyDam()
                               {
                                   Id = q.Id,
                                   FileName = q.FileName,
                                   ParentId = q.ParentId,
                                   FilePath = q.FilePath,
                                   FileType = q.FileType,
                                   MimeType = q.MimeType,
                                   RequestBy = qr.RequestBy,
                                   RequestDate = qr.RequestDate,
                                   Status = qr.Status,
                                   CreatedBy = s.NameEmployee,
                                   CreatedOn = q.CreatedOn,
                                   JobName = qt.Id + " - " + qt.JobName,
                                   JobId = qt.Id,
                                   ClientBriefId = tu.Id,
                                   CampaignName = tu.CampaignName,
                                   ClientName = tuv.CompanyName
                               }).ToListAsync();

            query = query.Where(x => Path.GetDirectoryName(x.FilePath) == "PROJECTMANAGEMENT\\DAM").ToList();
            var query2 = await (from q in context.peFileFolders
                                where q.parent_id == "mydam" && q.is_deleted == false
                                select new MyDam()
                                {
                                    Id = q.Id,
                                    FileName = Path.GetFileName(q.folder_path),
                                    ParentId = q.parent_id,
                                    FilePath = q.folder_path
                                }).ToListAsync();
            query2 = query2.Where(x => Path.GetDirectoryName(x.FilePath) == "PROJECTMANAGEMENT\\DAM").ToList();
            result.AddRange(query2);
            result.AddRange(query);
            return result;
        }

        public async Task<List<MyDam>> GetByFilename(string filename)
        {
            List<MyDam> result = new List<MyDam>();
            string[] Param = filename.Split(new string[] { "::" }, StringSplitOptions.None);
            var ParamFileName = string.Empty;
            var ParamJobId = string.Empty;
            var ParamClientId = string.Empty;
            var ParamCampaignId = string.Empty;
            var ParamJobCreate = string.Empty;
            var ParamJobCreateEnd = string.Empty;
            var ParamFolderName = string.Empty;

            for (int i = 0; i < Param.Length; i++)
            {
                if (i==0)
                {
                    ParamFileName = Param[i];
                }

                if (i == 1)
                {
                    ParamJobId = Param[i];
                }

                if (i == 2)
                {
                    ParamClientId = Param[i];
                }

                if (i == 3)
                {
                    ParamCampaignId = Param[i];
                }

                if (i == 4)
                {
                    ParamJobCreate = Param[i];
                }

                if (i == 5)
                {
                    ParamJobCreateEnd = Param[i];
                }

                if (i == 6)
                {
                    ParamFolderName = Param[i];
                }
            }
           
            ParamFolderName = ParamFolderName.Replace("*", "/");

            var query = await (from q in context.MyDam
                               join r in context.MyDamApproval on q.Id equals r.MyDamId into lj1
                               from qr in lj1.DefaultIfEmpty()
                               join s in context.EmployeeBasicInfos on q.CreatedBy equals s.Id
                               join t in context.JobDetail on q.ParentId equals t.Id into lj2
                               from qt in lj2.DefaultIfEmpty()
                               join u in context.ClientBrief on qt.ClientBriefId equals u.Id into lj3
                               from tu in lj3.DefaultIfEmpty()
                               join v in context.Companys on tu.ClientId equals v.Id into lj4
                               from tuv in lj4.DefaultIfEmpty()
                               where !(q.IsDeleted ?? false)
                               select new MyDam()
                               {
                                   Id = q.Id,
                                   FileName = q.FileName,
                                   ParentId = q.ParentId,
                                   FilePath = q.FilePath,
                                   FileType = q.FileType,
                                   MimeType = q.MimeType,
                                   RequestBy = qr.RequestBy,
                                   RequestDate = qr.RequestDate,
                                   Status = qr.Status,
                                   CreatedBy = s.NameEmployee,
                                   CreatedOn = q.CreatedOn,
                                   JobName = !(string.IsNullOrEmpty(qt.Id)) ? qt.Id + " - " + qt.JobName : "",
                                   JobId = qt.Id,
                                   ClientBriefId = tu.Id,
                                   CampaignName = tu.CampaignName,
                                   ClientName = tuv.CompanyName,
                                   JobCreateOn = qt.CreatedOn,
                                   ClientId = tu.ClientId
                               }).ToListAsync();

            // show folder root
            var query2 = new MyDam()
            {
                Id = "1",
                FileName = "ROOT",
                ParentId = "",
                FilePath = "PROJECTMANAGEMENT/DAM/"
            };
            result.Add(query2);

            // show folder inside root
            if (!(string.IsNullOrEmpty(ParamFolderName)))
            {
                
                var query3 = await (from q in context.peFileFolders
                                    where q.parent_id == "mydam" && q.folder_path.Contains(ParamFolderName) && q.is_deleted == false
                                    select new MyDam()
                                    {
                                        Id = q.Id,
                                        FileName = Path.GetFileName(q.folder_path),
                                        ParentId = q.parent_id,
                                        FilePath = q.folder_path.Replace("PROJECTMANAGEMENT/DAM/","")
                                    }).ToListAsync();
                //query3 = query3.Where(x => Path.GetDirectoryName(x.FilePath) != "PROJECTMANAGEMENT\\DAM").ToList();
                query3 = query3.Where(x => x.FilePath != ParamFolderName).ToList();
                result.AddRange(query3);
                query = query.Where(x => x.FilePath.Replace("PROJECTMANAGEMENT/DAM/" + ParamFolderName + "/", "") == x.FileName).ToList();
            }


            if (!(string.IsNullOrEmpty(ParamFileName)))
            {
                query = query.Where(x => x.FilePath.Contains(ParamFileName)).ToList();
            }

            if (!(string.IsNullOrEmpty(ParamJobId)))
            {
                query = query.Where(x => x.JobId == ParamJobId).ToList();
            }

            if (!(string.IsNullOrEmpty(ParamClientId)))
            {
                query = query.Where(x => x.ClientId == ParamClientId).ToList();
            }

            if (!(string.IsNullOrEmpty(ParamCampaignId)))
            {
                query = query.Where(x => x.ClientBriefId == ParamCampaignId).ToList();
            }

            if (!(string.IsNullOrEmpty(ParamJobCreate)))
            {
                query = query.Where(x => x.JobCreateOn >= DateTime.Parse(ParamJobCreate)).ToList();
            }

            if (!(string.IsNullOrEmpty(ParamJobCreateEnd)))
            {
                query = query.Where(x => x.JobCreateOn <= DateTime.Parse(ParamJobCreateEnd)).ToList();
            }

            result.AddRange(query);
            
            return result;
           
        }

        public async Task<List<MyDam>> GetFolderName()
        {
            var query = await(from q in context.peFileFolders
                               where q.parent_id == "mydam"
                               select new MyDam()
                               {
                                   Id = q.Id,
                                   FileName = Path.GetFileName(q.folder_path),
                                   ParentId = q.parent_id,
                                   FilePath = q.folder_path
                               }).ToListAsync();
            return query;
        }

        public async Task<List<MyDam>> GetWaitingApproval()
        {
            var query = await (from q in context.MyDam
                               join r in context.MyDamApproval on q.Id equals r.MyDamId 
                               join s in context.EmployeeBasicInfos on q.CreatedBy equals s.Id
                               join t in context.JobDetail on q.ParentId equals t.Id into lj2
                               join u in context.EmployeeBasicInfos on  r.RequestBy equals u.Id
                               from qt in lj2.DefaultIfEmpty()
                               where !(q.IsDeleted ?? false)
                               select new MyDam()
                               {
                                   Id = q.Id,
                                   FileName = q.FileName,
                                   ParentId = q.ParentId,
                                   FilePath = q.FilePath,
                                   FileType = q.FileType,
                                   MimeType = q.MimeType,
                                   RequestBy = u.NameEmployee,
                                   RequestDate = r.RequestDate,
                                   Status = r.Status,
                                   CreatedBy = s.NameEmployee,
                                   CreatedOn = q.CreatedOn,
                                   JobName = !(string.IsNullOrEmpty(qt.Id))? qt.Id + " - " + qt.JobName : "",
                                   JobId = qt.Id
                               }).ToListAsync();
            return query;
        }
        public Task<List<MyDam>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<MyDam> Get(string id)
        {
            var query = await (from q in context.MyDam
                               join r in context.MyDamApproval on q.Id equals r.MyDamId into lj1
                               from qr in lj1.DefaultIfEmpty()
                               join s in context.EmployeeBasicInfos on q.CreatedBy equals s.Id
                               join t in context.JobDetail on q.ParentId equals t.Id into lj2
                               from qt in lj2.DefaultIfEmpty()
                               join u in context.ClientBrief on qt.ClientBriefId equals u.Id into lj3
                               from tu in lj3.DefaultIfEmpty()
                               join v in context.Companys on tu.ClientId equals v.Id into lj4
                               from tuv in lj4.DefaultIfEmpty()
                               where !(q.IsDeleted ?? false) && q.Id == id
                               select new MyDam()
                               {
                                   Id = q.Id,
                                   FileName = q.FileName,
                                   ParentId = q.ParentId,
                                   RequestBy = qr.RequestBy,
                                    RequestByName  = (context.EmployeeBasicInfos.Where(x=>x.Id == qr.RequestBy)).Select(x=>x.NameEmployee).FirstOrDefault(),
                                   RequestDate = qr.RequestDate,
                                   Status = qr.Status,
                                   FilePath = q.FilePath,
                                   CreatedBy = s.NameEmployee,
                                   CreatedOn = q.CreatedOn,
                                   JobName = !(string.IsNullOrEmpty(qt.Id)) ? qt.Id + " - " + qt.JobName : "",
                                   JobId = qt.Id,
                                   ClientBriefId = tu.Id,
                                   CampaignName = tu.CampaignName,
                                   ClientName = tuv.CompanyName
                               }).FirstOrDefaultAsync();
            return query;
        }

       
    }

   
}
