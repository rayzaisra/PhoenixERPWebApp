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
    public interface IContractService : IDataServiceHris<Termination>
    {
        Task<Termination> GetById(string id);
    }

    public class ContractService : IContractService
    {
        readonly DataContext context;
        string[] terminationType = new string[] { TerminationType.EndOfContract, TerminationType.EndOfProbation };

        /// <summary>
        /// And endpoint to manage Termination
        /// </summary>
        /// <param name="context">Database context</param>
        public ContractService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(Termination entity)
        {
            entity.Id = await TransID.GetTransId(context, Code.Contract, DateTime.Today);
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> EditAsync(Termination entity)
        {
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
            return data;
        }

        public Task<int> ApproveAsync(Termination entity)
        {
            throw new NotImplementedException();
        }
    }
}