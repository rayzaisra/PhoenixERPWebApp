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
    public interface IMotherPCAService
    {
        Task<MotherPca> Get(string id);
    }

    public class MotherPCAService : IMotherPCAService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage Pca
        /// </summary>
        /// <param name="context">Database context</param>
        public MotherPCAService(DataContext context)
        {
            this.context = context;
        }

        public async Task<MotherPca> Get(string id)
        {
            var result = await context.MotherPcas.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == id).FirstOrDefaultAsync();
            result.Detail = null;
            return result;
        }
    }
}