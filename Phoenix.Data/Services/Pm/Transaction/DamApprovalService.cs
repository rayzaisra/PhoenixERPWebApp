using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Pm.ViewModel;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;
using Phoenix.Shared.Helpers;

namespace Phoenix.Data.Services.Pm.Transaction
{

    public interface IDamApprovalService : IDataService<DamFileShared>
    {
        Task<int> ApproveAsync(DamFileShared entity);
    }
    public class DamApprovalService : IDamApprovalService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly StatusLogService log;
        /// <summary>
        /// And endpoint to manage MasterOutlet
        /// </summary>
        /// <param name="context">Database context</param>
        public DamApprovalService(DataContext context, GlobalFunctionApproval globalFcApproval, StatusLogService log)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.log = log;
        }

        public async Task<int> ApproveAsync(DamFileShared dto)
        {
            var status = 0;
            var remarks = "";
            if (dto.statusApproval == "Approved")
            {
                status = 3;
                remarks = "Data has been agreed to be accepted.";
            }
            else
            {
                status = 4;
                remarks = "Data has been agreed to be rejected.";
            }
            var entity = context.DamFileSharings.Where(x => x.Id == dto.Id).FirstOrDefault();
            var userid = context.GetEmployeeId();

            await globalFcApproval.ProcessApproval<Phoenix.Shared.Core.Entities.ProjectManagement.DamFileSharing>(dto.Id, status, remarks, entity);
            var modeltrapp = await context.TrTemplateApprovals.Where(x => x.RefId == dto.Id).FirstOrDefaultAsync();
            if (modeltrapp != null)
            {
                if (modeltrapp.StatusApprovedDescription == Phoenix.Data.Models.StatusTransaction.StatusName(status))
                {
                    entity.status = Phoenix.Data.Models.StatusTransaction.StatusName(status);
                }
                else
                {
                    entity.status = Phoenix.Data.Models.StatusTransaction.StatusName(Phoenix.Data.Models.StatusTransaction.WaitingApproval);
                    status = 2;
                }
            }
            else
            {
                entity.status = Phoenix.Data.Models.StatusTransaction.StatusName(Phoenix.Data.Models.StatusTransaction.WaitingApproval);
                status = 2;
            }

            await log.AddAsync(new Phoenix.Data.Models.StatusLog()
            {
                TransactionId = dto.Id,
                Status = Phoenix.Data.Models.StatusTransaction.StatusName(status),
                Description = "Process " + Phoenix.Data.Models.StatusTransaction.StatusName(status)
            });

            context.PhoenixEdit(entity);
            var save = context.SaveChanges();

            return save;
        }

        public Task<int> AddAsync(DamFileShared entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddRangeAsync(params DamFileShared[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(DamFileShared entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params DamFileShared[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(DamFileShared entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params DamFileShared[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditAsync(DamFileShared entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditRangeAsync(DamFileShared entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DamFileShared>> Get()
        {
            var employeeId = context.GetEmployeeId();
            var query = await (from q in context.DamFileSharings
                               join r in context.TrTemplateApprovals on q.Id equals r.RefId
                               join s in context.EmployeeBasicInfos on q.share_to equals s.Id
                               join t in context.PeFiles on q.file_id equals t.Id
                               join u in context.JobDetail on q.job_id equals u.Id
                               join v in context.TrUserApprovals on r.Id equals v.TrTempApprovalId
                               join w in context.EmployeeBasicInfos on q.created_by equals w.Id
                               where q.is_deleted == false && v.EmployeeBasicInfoId == employeeId
                               select new DamFileShared()
                               {
                                   Id = q.Id,
                                   share_to = q.share_to,
                                   share_to_name = s.NameEmployee,
                                   file_id = q.file_id,
                                   file_name = t.file_name,
                                   file_path = q.file_path,
                                   job_id = q.job_id,
                                   job_Name = u.JobName,
                                   status = q.status,
                                   IsActive = q.is_active,
                                   IsDeleted = q.is_deleted,
                                   requester_id = q.created_by,
                                   requester_name = w.NameEmployee,
                                   requester_on = q.created_on,
                                   CreatedBy = q.created_by,
                                   CreatedOn = q.created_on
                               }).ToListAsync();
            return query;
        }

        public Task<List<DamFileShared>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<DamFileShared> Get(string id)
        {
            var employeeId = context.GetEmployeeId();
            var query = await (from q in context.DamFileSharings
                               join r in context.TrTemplateApprovals on q.Id equals r.RefId
                               join s in context.EmployeeBasicInfos on q.share_to equals s.Id
                               join t in context.PeFiles on q.file_id equals t.Id
                               join u in context.JobDetail on q.job_id equals u.Id
                               join v in context.TrUserApprovals on r.Id equals v.TrTempApprovalId
                               join w in context.EmployeeBasicInfos on q.created_by equals w.Id
                               where q.is_deleted == false && v.EmployeeBasicInfoId == employeeId && q.Id == id
                               select new DamFileShared()
                               {
                                   Id = q.Id,
                                   share_to = q.share_to,
                                   share_to_name = s.NameEmployee,
                                   file_id = q.file_id,
                                   file_name = t.file_name,
                                   file_path = q.file_path,
                                   job_id = q.job_id,
                                   job_Name = u.JobName,
                                   status = q.status,
                                   IsActive = q.is_active,
                                   IsDeleted = q.is_deleted,
                                   requester_id = q.created_by,
                                   requester_name = w.NameEmployee,
                                   requester_on = q.created_on,
                                   CreatedBy = q.created_by,
                                   CreatedOn = q.created_on
                               }).FirstOrDefaultAsync();
            return query;
        }
    }
}
