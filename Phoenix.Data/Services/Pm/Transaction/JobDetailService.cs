using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Finance.Master;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IJobDetailService
    {
        Task<JobDetail> Get(string Id);
    }

    public class JobDetailService : IJobDetailService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage Pca
        /// </summary>
        /// <param name="context">Database context</param>
        public JobDetailService(DataContext context)
        {
            this.context = context;
        }

        public async Task<JobDetail> Get(string Id)
        {
            return await context.JobDetail.Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<dynamic> Get()
        {
            var result = await (from a in context.JobDetail
                                join b in context.BusinessUnits on a.BusinessUnitDivisionId equals b.Id
                                join c in context.LegalEntity on a.LegalEntityId equals c.Id
                                join d in context.CompanyBrands on a.BrandId equals d.Id
                                join e in context.Companys on a.CompanyId equals e.Id
                                join f in context.ClientBrief on a.ClientBriefId equals f.Id
                                join z in context.BusinessUnitTypes on b.BusinessUnitTypeId equals z.Id
                                where !(a.IsDeleted ?? false) && d.IsClient == true && e.IsClient == true &&
                                z.BusinessUnitLevel == BusinessUnitLevelCode.Division
                                select new
                                {
                                    a.Id,
                                    a.JobName,
                                    DivisionName = b.UnitName,
                                    c.LegalEntityName,
                                    ClientName = e.CompanyName,
                                    BrandName = d.CompanyName,
                                    a.Deadline,
                                    f.CampaignName,
                                    JobStatus = a.JobStatusId,
                                    Status = StatusTransactionName.Draft
                                }).ToListAsync();
            return result;
        }
    }
}