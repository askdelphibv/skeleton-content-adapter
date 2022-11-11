using AskDelphi.SampleContentAdapter.ServiceModel;
using AskDelphi.SampleContentAdapter.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AskDelphi.SampleContentAdapter.HostedServices
{
    /// <summary>
    /// 
    /// </summary>
    public class CacheSynchronizationService : IHostedService, IDisposable
    {
        private readonly ILogger logger;
        private readonly IOperationContextFactory operationContextFactory;
        private readonly ITopicRepository topicRepository;
        private Timer timer;

        /// <summary>
        /// 
        /// </summary>
        public CacheSynchronizationService(
            ILogger<CacheSynchronizationService> logger, 
            IOperationContextFactory operationContextFactory, 
            ITopicRepository topicRepository)
        {
            this.logger = logger;
            this.operationContextFactory = operationContextFactory;
            this.topicRepository = topicRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("CacheSynchronizationService is starting.");

            timer = new Timer(async (_) => await DoWorkAsync(), null, TimeSpan.Zero, TimeSpan.FromMinutes(60));

            return Task.CompletedTask;
        }

        private async Task DoWorkAsync()
        {
            logger.LogInformation("CacheSynchronizationService is working.");

            try
            {
                // TODO: Implement whatever is needed for the hourly sync of the content
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Cache sync failed");
            }

            await Task.FromResult(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("CacheSynchronizationService is stopping.");

            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
