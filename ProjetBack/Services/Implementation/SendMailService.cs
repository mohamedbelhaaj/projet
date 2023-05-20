using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProjetBack.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjetBack.Services.Implementation
{
    public class SendMailService : ConsumeScopedHostedService
    {
        private readonly ILogger<SendMailService> _logger;
        private readonly IServiceProvider service;

        public SendMailService(IScheduleConfig<SendMailService> config, ILogger<SendMailService> logger, IServiceProvider service) : base( config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            this.service = service;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ScheduleJob starts.");
            return base.StartAsync(cancellationToken);
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            using (var scope = service.CreateScope())
            {

                var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IBackgroundEmailSender>();

                await scopedProcessingService.DoWork();
            }
           // return Ok(Task.CompletedTask);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ScheduleJob is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
