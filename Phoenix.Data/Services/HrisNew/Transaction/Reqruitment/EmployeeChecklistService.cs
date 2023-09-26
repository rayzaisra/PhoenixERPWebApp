using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.ApiExtension.Extensions;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IEmployeeChecklistService : IDataService<EmployeeChecklist>
    {
		Task<List<FormModelEmployeeChecklist>> GetByEmployeeBasicInfoId(string empid);
        Task<List<FormModelEmployeeHistory>> GetByEmployeeHistory(string empid);
        Task<List<EmployeeContractSign>> GetByContractSign(string empid);
    }

    public class EmployeeChecklistService : IEmployeeChecklistService
	{
        readonly DataContext context;
        /// <summary>
        /// And endpoint to manage Termination
        /// </summary>
        /// <param name="context">Database context</param>
        public EmployeeChecklistService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(EmployeeChecklist entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }
		
		public Task<int> AddRangeAsync(params EmployeeChecklist[] entities)
        {
			context.PhoenixAddRange(entities);
			return context.SaveChangesAsync();
		}

        public Task<int> DeleteAsync(EmployeeChecklist entity)
        {
			context.Remove(entity);
			return context.SaveChangesAsync();
		}

        public Task<int> DeleteRageAsync(params EmployeeChecklist[] entities)
        {
			context.RemoveRange(entities);
			return context.SaveChangesAsync();
		}

        public async Task<int> DeleteSoftAsync(EmployeeChecklist entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params EmployeeChecklist[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(EmployeeChecklist entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(EmployeeChecklist entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<EmployeeChecklist>> Get() => await context.EmployeeChecklists.AsNoTracking().Where(x => x.IsDeleted != true).ToListAsync();

        public Task<List<EmployeeChecklist>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<EmployeeChecklist> Get(string Id) => await context.EmployeeChecklists.AsNoTracking().Where(x => x.IsDeleted != true && x.Id == Id).FirstOrDefaultAsync();
		public async Task<List<FormModelEmployeeChecklist>> GetByEmployeeBasicInfoId(string empid)
		{
            List<FormModelEmployeeChecklist> newList = new List<FormModelEmployeeChecklist>();
            var listChecklist = await context.EmployeeChecklists.AsNoTracking().Where(x => x.IsDeleted != true && x.EmployeeBasicInfoId == empid)
			.Select(x => new FormModelEmployeeChecklist
			{
				Id = x.Id,
				EchkEmployeeBasicInfoId = x.EmployeeBasicInfoId,
				EchkChecklistitem = x.Checklistitem,
				EchkDescription = x.Description,
				EchkWhentoalert = x.Whentoalert,
				EchkEmployeeChecklistCategoryId = x.EmployeeChecklistCategoryId,
				EchkBusinessUnitPicId = x.BusinessUnitPicId,
				EchkEmployeeChecklistCategory = string.IsNullOrEmpty(x.EmployeeChecklistCategoryId) ? "" : (from a in context.EmployeeChecklistCategorys where a.Id == x.EmployeeChecklistCategoryId select a.ChecklistCategory).FirstOrDefault()
			}).ToListAsync();

            var groupedCustomerList = listChecklist.GroupBy(u => u.EchkChecklistitem).Select(grp => grp.ToList()).ToList();
            foreach (var item in groupedCustomerList)
            {
                FormModelEmployeeChecklist m = new FormModelEmployeeChecklist();
                m.EchkEmployeeBasicInfoId = item[0].EchkEmployeeBasicInfoId;
                m.EchkChecklistitem = item[0].EchkChecklistitem;
                m.EchkDescription = item[0].EchkDescription;
                m.EchkWhentoalert = item[0].EchkWhentoalert;
                m.Id = item[0].Id;
                m.EchkBusinessUnitPicId = item[0].EchkBusinessUnitPicId;
                foreach (var item2 in item)
                {
                    m.EchkEmployeeChecklistCategory += (from a in context.EmployeeChecklistCategorys where a.Id == item2.EchkEmployeeChecklistCategoryId select a.ChecklistCategory).FirstOrDefault() + ";";
                    m.EchkEmployeeChecklistCategoryId += item2.EchkEmployeeChecklistCategoryId + ",";
                }
                m.EchkEmployeeChecklistCategoryId = m.EchkEmployeeChecklistCategoryId.Remove(m.EchkEmployeeChecklistCategoryId.Length - 1);
                newList.Add(m);
            }

            return newList;

        }

        public async Task<List<FormModelEmployeeHistory>> GetByEmployeeHistory(string empid)
        {
            List<FormModelEmployeeHistory> resp = new List<FormModelEmployeeHistory>();

            var promotion = await (from x in context.Promotions
                                   join c in context.TrTemplateApprovals on x.Id equals c.RefId
                                   where x.IsDeleted != true && x.EmployeeBasicInfoId == empid
                                   select new FormModelEmployeeHistory()
                                   {
                                       Id = x.Id,
                                       EHistEmployeeBasicInfoId = x.EmployeeBasicInfoId,
                                       Date = x.RequestDate,
                                       Category = "Promotion",
                                       EHistEmployeeName = (context.EmployeeBasicInfos.Where(y => y.Id == empid).Select(y => y.NameEmployee).FirstOrDefault()),
                                       Remarks = x.Notes,
                                       Status = c.StatusApprovedDescription,
                                       FileId = ""

                                   }
                                 ).ToListAsync();

            var demotion = await (from x in context.Demotions
                                   join c in context.TrTemplateApprovals on x.Id equals c.RefId
                                   where x.IsDeleted != true && x.EmployeeBasicInfoId == empid
                                   select new FormModelEmployeeHistory()
                                   {
                                       Id = x.Id,
                                       EHistEmployeeBasicInfoId = x.EmployeeBasicInfoId,
                                       Date = x.RequestDate,
                                       Category = "Demotion",
                                       EHistEmployeeName = (context.EmployeeBasicInfos.Where(y => y.Id == empid).Select(y => y.NameEmployee).FirstOrDefault()),
                                       Remarks = x.Notes,
                                       Status = c.StatusApprovedDescription,
                                       FileId = x.WarningLetter
                                   }
                                ).ToListAsync();

            var sanction = await (from x in context.Terminations
                                  join c in context.TrTemplateApprovals on x.Id equals c.RefId
                                  where x.IsDeleted != true && x.EmployeeBasicInfoId == empid && x.TerminationType == "6"
                                  select new FormModelEmployeeHistory()
                                  {
                                      Id = x.Id,
                                      EHistEmployeeBasicInfoId = x.EmployeeBasicInfoId,
                                      Date = x.RequestDate,
                                      Category = "Sanction",
                                      EHistEmployeeName = (context.EmployeeBasicInfos.Where(y => y.Id == empid).Select(y => y.NameEmployee).FirstOrDefault()),
                                      Remarks = x.Reason,
                                      Status = c.StatusApprovedDescription,
                                      FileId = x.InterviewWarningFile
                                  }
                                  ).ToListAsync();

            var retired = await (from x in context.Terminations
                                  join c in context.TrTemplateApprovals on x.Id equals c.RefId
                                  where x.IsDeleted != true && x.EmployeeBasicInfoId == empid && x.TerminationType == "7"
                                  select new FormModelEmployeeHistory()
                                  {
                                      Id = x.Id,
                                      EHistEmployeeBasicInfoId = x.EmployeeBasicInfoId,
                                      Date = x.RequestDate,
                                      Category = "Retired",
                                      EHistEmployeeName = (context.EmployeeBasicInfos.Where(y => y.Id == empid).Select(y => y.NameEmployee).FirstOrDefault()),
                                      Remarks = x.Reason,
                                      Status = c.StatusApprovedDescription,
                                      FileId = x.InterviewWarningFile
                                  }
                                  ).ToListAsync();

            var kpi = await (from x in context.PdrApprailsalM1Headers
                             join y in context.PdrApprailsalM1s on x.Id equals y.PdrApprailsalM1HeaderId
                             join c in context.TrTemplateApprovals on x.Id equals c.RefId
                             where x.IsDeleted != true && x.EmployeeId == empid
                             select new FormModelEmployeeHistory()
                             {
                                 Id = x.Id,
                                 EHistEmployeeBasicInfoId = x.EmployeeId,
                                 Date = x.ReviewStart,
                                 Category = "KPI",
                                 EHistEmployeeName = (context.EmployeeBasicInfos.Where(y => y.Id == empid).Select(y => y.NameEmployee).FirstOrDefault()),
                                 Remarks = "",
                                 Status = c.StatusApprovedDescription,
                                 FileId = ""
                             }
                                  ).ToListAsync();

            var pdr = await (from x in context.PdrEmployees
                             join c in context.TrTemplateApprovals on x.Id equals c.RefId
                             where x.IsDeleted != true && x.EmployeeBasicInfoId == empid
                             select new FormModelEmployeeHistory()
                             {
                                 Id = x.Id,
                                 EHistEmployeeBasicInfoId = x.EmployeeBasicInfoId,
                                 Date = x.ReviewStart,
                                 Category = "PDR",
                                 EHistEmployeeName = (context.EmployeeBasicInfos.Where(y => y.Id == empid).Select(y => y.NameEmployee).FirstOrDefault()),
                                 Remarks = x.Notes,
                                 Status = c.StatusApprovedDescription,
                                 FileId = x.FileResult
                             }
                                  ).ToListAsync();

            resp.AddRange(promotion);
            resp.AddRange(demotion);
            resp.AddRange(sanction);
            resp.AddRange(retired);
            resp.AddRange(pdr);
            resp.AddRange(kpi);

            resp = resp.OrderByDescending(x => x.Date).ToList();
            return resp;

        }

        public async Task<List<EmployeeContractSign>> GetByContractSign(string empid)
        {
            List<EmployeeContractSign> resp = new List<EmployeeContractSign>();

            var ctr = await (from x in context.EmployeeContracts
                             join c in context.CandidateRecruitmentsSignContract on x.CandidateRecruitmentId equals c.candidate_recruitment_id
                             join y in context.EmployeeBasicInfos on x.EmployeeBasicInfoId equals y.Id
                             where x.IsDeleted != true && x.EmployeeBasicInfoId == empid
                             select new EmployeeContractSign()
                             {
                                 ContractName = c.remarks,
                                 ContractFile = x.FilemasterId,
                                 StartDate = x.StartDate,
                                 EndDate = x.EndDate,
                                 ContractType = c.contracttype,
                                 Ptpp = c.ptpp,
                                 FileId = x.FilemasterId
                             }).ToListAsync();
            resp.AddRange(ctr);
          
            return resp;

        }



    }
}