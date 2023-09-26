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
    public interface IPMTeamService
    {
        bool IsPMTeam(string jobId);
    }

    public class PMTeamService : IPMTeamService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage Pca
        /// </summary>
        /// <param name="context">Database context</param>
        public PMTeamService(DataContext context)
        {
            this.context = context;
        }

        public bool IsPMTeam(string jobId)
        {
            var userId = context.GetUserId();
            var team = (from a in context.PmTeams
                        join b in context.PmTeamMembers on a.Id equals b.TeamId
                        where !(a.IsDeleted ?? false) && !(b.IsDeleted ?? false) && b.ApplicationUserId == userId && a.JobId == jobId
                        select new { a.JobId }).ToList();
            return team.Count > 0;
        }
    }
}