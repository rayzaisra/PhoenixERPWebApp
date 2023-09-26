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
    public interface IDeathService : IDataServiceHris<Termination>
    {
        Task<Termination> GetById(string id);
    }

    public class DeathService : IDeathService
    {
        readonly DataContext context;
        readonly FileService uploadFile;

        string[] terminationType = new string[] { TerminationType.Death, TerminationType.ProlongedIllness, TerminationType.Sanction, TerminationType.Retired };

        /// <summary>
        /// And endpoint to manage Termination
        /// </summary>
        /// <param name="context">Database context</param>
        public DeathService(DataContext context, FileService uploadFile)
        {
            this.context = context;
            this.uploadFile = uploadFile;
        }

        public async Task<int> AddAsync(Termination entity)
        {
            if (entity.TerminationType == TerminationType.Retired)
            {
                entity.Id = await TransID.GetTransId(context, Code.Retired, DateTime.Today);
            }
            else
            {
                entity.Id = await TransID.GetTransId(context, Code.Death, DateTime.Today);
            }
            
            var file = await uploadFile.Upload(entity.file, null);
            if (!string.IsNullOrEmpty(file))
            {
                entity.MedicalCertificate = file;
            }
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> EditAsync(Termination entity)
        {
            var file = await uploadFile.Upload(entity.file, entity.MedicalCertificate);
            if (!string.IsNullOrEmpty(file))
            {
                entity.MedicalCertificate = file;
            }
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<List<Termination>> Get()
        {
            TerminationService termination = new TerminationService(context);
            var result = await termination.GetTermination(context.GetEmployeeId(), terminationType);
            return result;
        }
        public async Task<int> DeleteSoftAsync(Termination entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<Termination> Get(string Id) => await context.Terminations.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();

        public async Task<Termination> GetById(string id)
        {
            TerminationService termination = new TerminationService(context);
            var data = await termination.GetTerminationById(context.GetEmployeeId(), terminationType, id);
            if (data.MedicalCertificate != null)
            {
                var file = await context.Filemasters.Where(x => x.Id == data.MedicalCertificate).FirstOrDefaultAsync();
                if (file != null) data.MedicalCertificateName = file.Name;
            }

            if (data.InterviewWarningFile != null)
            {
                var InterviewWarning = await context.Filemasters.Where(x => x.Id == data.InterviewWarningFile).FirstOrDefaultAsync();
                if (InterviewWarning != null) data.InterviewWarningFileName = InterviewWarning.Name;
            }

            if (data.StatementOfPoliceReport != null)
            {
                var StatementOfPoliceReport = await context.Filemasters.Where(x => x.Id == data.StatementOfPoliceReport).FirstOrDefaultAsync();
                if (StatementOfPoliceReport != null) data.StatementOfPoliceReportName = StatementOfPoliceReport.Name;
            }
            return data;
        }

        public Task<int> ApproveAsync(Termination entity)
        {
            throw new NotImplementedException();
        }
    }
}