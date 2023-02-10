using AskDelphi.ContentAdapter.DTO;
using AskDelphi.ContentAdapter.ServiceModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AskDelphi.ContentAdapter.Services.SampleDataRepositories
{
    /// <summary>
    /// Very simple topic repository that reads JSON document containing process descriptions from a folder.
    /// </summary>
    public class SampleDataTopicRepository : ITopicRepository
    {
        private readonly DirectoryInfo dataFolder;
        private readonly IConfiguration configuration;
        private readonly ILogger<SampleDataTopicRepository> logger;
        private readonly ITopicFactory topicFactory;
        private List<TopicDescriptor> topicDescriptors = new List<TopicDescriptor>();
        private ManualResetEvent dataLoaded = new ManualResetEvent(false);


        public SampleDataTopicRepository(IConfiguration configuration, ILogger<SampleDataTopicRepository> logger, ITopicFactory topicFactory)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.topicFactory = topicFactory;
            this.dataFolder = new DirectoryInfo(configuration["DataFolder"]);
            if (!dataFolder.Exists)
            {
                throw new InvalidOperationException($"Can't find required folder '{dataFolder}'. Update appsettings.json to point DataFolder to an existing folder.");
            }
        }

        public async Task<SCR<IEnumerable<FolderDescriptor>>> FindFoldersAsync(IOperationContext operationContext, string folderId)
        {
            // In this implementation there is only one folder.
            return SCR<IEnumerable<FolderDescriptor>>.FromData(await Task.FromResult(new FolderDescriptor[] { }));
        }

        public async Task<SCR<IEnumerable<TopicDescriptor>>> FindMatchingTopicsAsync(IOperationContext operationContext, ContentSearchRequest query)
        {
            dataLoaded.WaitOne(); // wait until the data was loaded at least once

            List<TopicDescriptor> matches = topicDescriptors.Where(x => x.Title?.IndexOf(query.Query ?? "", StringComparison.OrdinalIgnoreCase) != -1).ToList();
            await Task.FromResult(0);
            return SCR<IEnumerable<TopicDescriptor>>.FromData(matches.Skip(Math.Max(0, query.Page - 1) * query.Size).Take(query.Size));
        }

        public async Task<SCR<TopicContent[]>> GetContent(IOperationContext operationContext, string topicId)
        {
            dataLoaded.WaitOne(); // wait until the data was loaded at least once

            var topicDescriptor = topicDescriptors.Where(x => x.TopicId == topicId).FirstOrDefault();
            if (null == topicDescriptor)
            {
                return SCR<TopicContent[]>.FromError(ErrorCodes.TopicRepositoryFileNotFound, ErrorCodes.TopicRepositoryFileNotFoundMessage, System.Net.HttpStatusCode.NotFound);
            }
            var file = new FileInfo(Path.Combine(dataFolder.FullName, topicId.Split('\\').FirstOrDefault()));
            return await topicFactory.CreateTopic(operationContext, topicId, file);
        }

        public async Task<SCR<DateTimeOffset?>> GetContentLastChangedDate(IOperationContext operationContext, string topicId)
        {
            dataLoaded.WaitOne(); // wait until the data was loaded at least once

            var topicDescriptor = topicDescriptors.Where(x => x.TopicId == topicId).FirstOrDefault();
            if (null == topicDescriptor)
            {
                return SCR<DateTimeOffset?>.FromError(ErrorCodes.TopicRepositoryFileNotFound, ErrorCodes.TopicRepositoryFileNotFoundMessage, System.Net.HttpStatusCode.NotFound);
            }
            var file = new FileInfo(Path.Combine(dataFolder.FullName, topicId.Split('\\').FirstOrDefault()));
            return SCR<DateTimeOffset?>.FromData(await Task.FromResult(file.LastWriteTimeUtc));
        }

        public async Task<SCR<TopicMetadata>> GetMetadata(IOperationContext operationContext, string topicId)
        {
            dataLoaded.WaitOne(); // wait until the data was loaded at least once

            var topicDescriptor = topicDescriptors.Where(x => x.TopicId == topicId).FirstOrDefault();
            if (null == topicDescriptor)
            {
                return SCR<TopicMetadata>.FromError(ErrorCodes.TopicRepositoryFileNotFound, ErrorCodes.TopicRepositoryFileNotFoundMessage, System.Net.HttpStatusCode.NotFound);
            }
            TopicMetadata result = new TopicMetadata
            {
                Description = string.Empty,
                IndexContents = string.Empty,
                Tags = new TaxonomyValues[] { }
            };
            return SCR<TopicMetadata>.FromData(await Task.FromResult(result));
        }

        public async Task<SCR<bool>> RefreshAsync(IOperationContext operationContext)
        {
            // To check if the first load is completed, check the event was Set() already, if not we need to set it once
            // After it was set once, we will never set it again, we'll just load an additional collection in the 
            // background and swap out the one we have when we're done.
            bool firstLoadWasCompleted = dataLoaded.WaitOne(TimeSpan.FromSeconds(0));

            List<TopicDescriptor> freshTopicDescriptors = new List<TopicDescriptor>();
            foreach (var file in dataFolder.GetFiles())
            {
                string topicId = $"{file.Name}";
                IEnumerable<TopicDescriptor> descriptors = await topicFactory.GetDescriptors(file, topicId);
                freshTopicDescriptors.AddRange(descriptors);
            }

            // Replace the cached version with the new one
            this.topicDescriptors = freshTopicDescriptors;

            if (!firstLoadWasCompleted)
            {
                dataLoaded.Set();
                logger.LogInformation($"First topic refresh completed.");
            }
            else
            {
                logger.LogInformation($"Topic refresh completed.");
            }
            return SCR<bool>.FromData(await Task.FromResult(true));
        }
    }
}
