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
    public interface IRateCardService
    {
        Task<TaskRateCard> GetTaskRateCard(string rateCardId, string taskRateCardName);
        Task<SubTaskRateCard> GetSubTaskRateCard(string rateCardId, string taskRateCardName, string subTaskRateCardName);
    }

    public class RateCardService : IRateCardService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage Pca
        /// </summary>
        /// <param name="context">Database context</param>
        public RateCardService(DataContext context)
        {
            this.context = context;
        }

        public async Task<TaskRateCard> GetTaskRateCard(string rateCardId, string taskRateCardName)
        {
            var model = await context.RateCards.Where(x => (x.IsDeleted ?? false) == false && x.Id == rateCardId).FirstOrDefaultAsync();
            if (model != null)
            {
                var detail = model.Detail;
                if (!string.IsNullOrEmpty(detail))
                {
                    var list = JsonConvert.DeserializeObject<List<TaskRateCard>>(detail);
                    //var task = list.Where(x => x.task_name == taskRateCardName).FirstOrDefault();
                    var task = list.Where(x => x.task_name.Contains(taskRateCardName)).FirstOrDefault();
                    if (task != null) return task;
                }
            }
            return new TaskRateCard();
        }

        public async Task<SubTaskRateCard> GetSubTaskRateCard(string rateCardId, string taskRateCardName, string subTaskRateCardName)
        {
            var model = await context.RateCards.Where(x => (x.IsDeleted ?? false) == false && x.Id == rateCardId).FirstOrDefaultAsync();
            if (model != null)
            {
                var detail = model.Detail;
                if (!string.IsNullOrEmpty(detail))
                {
                    var list = JsonConvert.DeserializeObject<List<TaskRateCard>>(detail);
                    var task = list.Where(x => x.task_name == taskRateCardName).FirstOrDefault();
                    if (task != null)
                    {
                        var subtask = task.sub_Task.Where(x => x.subtask == subTaskRateCardName).FirstOrDefault();
                        if (subtask != null) return subtask;
                    }
                }
            }
            return new SubTaskRateCard();
        }
    }
}