using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NCrontab;
using Phoenix.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PhoenixWebApi.Services
{
    public class JobScheduleServices : BackgroundService, IHostedService//, IDisposable
    {
        private readonly ILogger<JobScheduleServices> logger;
        private readonly IServiceScopeFactory scopeFactory;
        private DateTime nextRun;

        public JobScheduleServices(IServiceScopeFactory scopeFactory, ILogger<JobScheduleServices> logger)
        {
            this.scopeFactory = scopeFactory;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var service = new ScheduleService();
            string valueSysParam = service.GetSchedule(scopeFactory);
            var schedule = CrontabSchedule.Parse(valueSysParam); //every 2 minute cek your schedule code in https://crontab.guru/
            nextRun = schedule.GetNextOccurrence(DateTime.Now);

            logger.LogWarning($"Background Service Process is starting on " + DateTime.Now);
            stoppingToken.Register(() => logger.LogWarning($"Background Service Process is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.Now;
                    if (now > nextRun)
                    {
                        //proses crud dtaa
                        await service.PromotionAsync(scopeFactory);
                        await service.RotationAsync(scopeFactory);
                        nextRun = schedule.GetNextOccurrence(DateTime.Now);
                    }
                    await Task.Delay(60000, stoppingToken); //1 minute delay
                }
                catch (Exception ex)
                {
                    logger.LogWarning($"Background Service Process is stopping." + ex.Message);
                }
            }
        }
    }
}

