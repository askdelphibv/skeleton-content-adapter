using AskDelphi.ContentAdapter.ServiceModel;
using AskDelphi.ContentAdapter.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace AskDelphi.ContentAdapter.HostedServices
{
    /// <summary>
    /// 
    /// </summary>
    public class CacheSynchronizationService : IHostedService, IDisposable
    {
        private readonly ILogger logger;
        private readonly IOperationContextFactory operationContextFactory;
        private readonly IConfiguration configuration;
        private readonly ITopicRepository topicRepository;

        /// <summary>
        /// Delay before the first update cycle is is started on the content repository.
        /// </summary>
        public TimeSpan InitialDelay { get; }
        /// <summary>
        /// Interval between updates of the content repository.
        /// </summary>
        /// 
        public TimeSpan Interval { get; }

        private Timer timer;

        /// <summary>
        /// 
        /// </summary>
        public CacheSynchronizationService(
            ILogger<CacheSynchronizationService> logger, 
            IOperationContextFactory operationContextFactory,
            IConfiguration configuration,
            ITopicRepository topicRepository)
        {
            this.logger = logger;
            this.operationContextFactory = operationContextFactory;
            this.configuration = configuration;
            this.topicRepository = topicRepository;

            this.InitialDelay = XmlConvert.ToTimeSpan(configuration.GetValue<string>("ContentUpdates:InitialDelay"));
            this.Interval = XmlConvert.ToTimeSpan(configuration.GetValue<string>("ContentUpdates:Interval"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("CacheSynchronizationService is starting.");

            timer = new Timer(async (_) => await DoWorkAsync(), null, InitialDelay, Interval);

            return Task.CompletedTask;
        }

        private async Task DoWorkAsync()
        {
            logger.LogInformation("CacheSynchronizationService is working.");

            try
            {
                await topicRepository.RefreshAsync(operationContextFactory.CreateBackgroundOperationContext());
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
