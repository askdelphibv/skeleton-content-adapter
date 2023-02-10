using AskDelphi.ContentAdapter.DTO;
using AskDelphi.ContentAdapter.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.ContentAdapter.Services.SampleDataRepositories
{
    public class SampleDataTopicRepository : ITopicRepository
    {
        public Task<SCR<IEnumerable<FolderDescriptor>>> FindFoldersAsync(IOperationContext operationContext, string folderId)
        {
            throw new NotImplementedException();
        }

        public Task<SCR<IEnumerable<TopicDescriptor>>> FindMatchingTopicsAsync(IOperationContext operationContext, ContentSearchRequest query)
        {
            throw new NotImplementedException();
        }

        public Task<SCR<TopicContent[]>> GetContent(IOperationContext operationContext, string topicId)
        {
            throw new NotImplementedException();
        }

        public Task<SCR<DateTimeOffset?>> GetContentLastChangedDate(IOperationContext operationContext, string topicId)
        {
            throw new NotImplementedException();
        }

        public Task<SCR<TopicMetadata>> GetMetadata(IOperationContext operationContext, string topicId)
        {
            throw new NotImplementedException();
        }

        public Task<SCR<bool>> RefreshAsync(IOperationContext operationContext)
        {
            throw new NotImplementedException();
        }
    }
}
