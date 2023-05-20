using Cronos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjetBack.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace ProjetBack.Services.Implementation
{
    public abstract class ConsumeScopedHostedService : IHostedService, IDisposable
    {
        private System.Timers.Timer _timer;

        private readonly CronExpression _expression;
        //private readonly IServiceProvider service;
        private readonly TimeZoneInfo timeZoneInfo;

        public ConsumeScopedHostedService(/*IServiceProvider service,*/ string cronExpression, TimeZoneInfo timeZoneInfo)
        {
          
            _expression = CronExpression.Parse(cronExpression);
           // this.service = service;
            this.timeZoneInfo = timeZoneInfo;
        }
  
        public virtual async Task StartAsync(CancellationToken cancellationToken)
        { 


            await ScheduleJob(cancellationToken);
        }

        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Stop();
            return Task.CompletedTask;
        }

        public virtual async Task DoWork(CancellationToken cancellationToken)
        {
            await Task.Delay(5000, cancellationToken);
            //using (var scope = service.CreateScope())
            //{

            //    var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IBackgroundEmailSender>();

            //    await scopedProcessingService.DoWork();
            //}

        }



        public virtual void Dispose()
        {
            _timer?.Dispose();
        }


        protected virtual async Task ScheduleJob(CancellationToken cancellationToken)
        {
            var next = _expression.GetNextOccurrence(DateTimeOffset.Now, timeZoneInfo);
            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;
                if (delay.TotalMilliseconds <= 0)   // prevent non-positive values from being passed into Timer
                {
                    await ScheduleJob(cancellationToken);
                }
                _timer = new System.Timers.Timer(delay.TotalMilliseconds);
                _timer.Elapsed += async (sender, args) =>
                {
                    _timer.Dispose();  // reset and dispose timer
                    _timer = null;

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await DoWork(cancellationToken);
                    }

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await ScheduleJob(cancellationToken);    // reschedule next
                    }
                };
                _timer.Start();
            }
            await Task.CompletedTask;
        }











    }
}
     
  