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
    public interface IHCProcessTerminationService : IDataServiceHris<Termination>
    {
        Task<Termination> GetById(string id);
    }

    public class HCProcessTerminationService : IHCProcessTerminationService
    {
        readonly DataContext context;
        readonly FileService uploadFile;

        string[] terminationType = new string[] {TerminationType.Resignation,
            TerminationType.EndOfContract, TerminationType.EndOfProbation,
            TerminationType.Death, TerminationType.ProlongedIllness,
            TerminationType.Sanction, TerminationType.Retired};

        /// <summary>
        /// And endpoint to manage Termination
        /// </summary>
        /// <param name="context">Database context</param>
        public HCProcessTerminationService(DataContext context, FileService uploadFile)
        {
            this.context = context;
            this.uploadFile = uploadFile;
        }

        public Task<int> AddAsync(Termination entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> ApproveAsync(Termination entity)
        {
            //if (entity.TerminationType == TerminationType.Resignation || entity.TerminationType == TerminationType.EndOfContract || entity.TerminationType == TerminationType.EndOfProbation)
            //{

            var file = await uploadFile.Upload(entity.file, entity.ExitInterviewFiles);
            if (!string.IsNullOrEmpty(file))
            {
                entity.ExitInterviewFiles = file;
            }

            //}
            //else if (entity.TerminationType == TerminationType.Sanction)
            //{ 
            //    var file = uploadFile.Upload(entity.file, entity.InterviewWarningFile);
            //    if (!string.IsNullOrEmpty(file))
            //    {
            //        entity.InterviewWarningFile = file;
            //    }
            //}
            var dataEmploye = await context.EmployeeBasicInfos.Where(x => x.Id == entity.EmployeeBasicInfoId).FirstOrDefaultAsync();
            dataEmploye.EmployementStatusEndDate = entity.TerminationDate;
            dataEmploye.IsTerminated = true;
            context.PhoenixEditLite(dataEmploye);
            context.PhoenixApprove(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftAsync(Termination entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditAsync(Termination entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Termination>> Get()
        {
            RequestorService request = new RequestorService(context);
            TerminationService termination = new TerminationService(context);
            var result = await termination.GetTerminationHC(terminationType, true);
            return result;
        }


        public async Task<Termination> Get(string Id) => await context.Terminations.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();

        public async Task<Termination> GetById(string id)
        {
            TerminationService termination = new TerminationService(context);
            var data = await termination.GetTerminationById(null, terminationType, id);
            //if (data.TerminationType == TerminationType.Resignation || data.TerminationType == TerminationType.EndOfContract || data.TerminationType == TerminationType.EndOfProbation)
            //{
            var file = await context.Filemasters.Where(x => x.Id == data.ExitInterviewFiles).FirstOrDefaultAsync();
            if (file != null) data.ExitInterviewFilesName = file.Name;
            //}
            //else 
            if (data.TerminationType == TerminationType.Sanction)
            {
                var fileWarning = await context.Filemasters.Where(x => x.Id == data.InterviewWarningFile).FirstOrDefaultAsync();
                if (fileWarning != null) data.InterviewWarningFileName = fileWarning.Name;

                var filePolice = await context.Filemasters.Where(x => x.Id == data.StatementOfPoliceReport).FirstOrDefaultAsync();
                if (filePolice != null) data.StatementOfPoliceReportName = filePolice.Name;
            }

            if (data.TerminationType == TerminationType.Death || data.TerminationType == TerminationType.ProlongedIllness)
            {
                var fileCerfiticate = await context.Filemasters.Where(x => x.Id == data.MedicalCertificate).FirstOrDefaultAsync();
                if (fileCerfiticate != null) data.InterviewWarningFileName = fileCerfiticate.Name;
            }
            return data;
        }
    }
}