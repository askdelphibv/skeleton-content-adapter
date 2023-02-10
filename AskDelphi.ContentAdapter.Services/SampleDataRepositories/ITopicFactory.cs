using AskDelphi.ContentAdapter.DTO;
using AskDelphi.ContentAdapter.ServiceModel;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AskDelphi.ContentAdapter.Services.SampleDataRepositories
{
    public interface ITopicFactory
    {
        Task<SCR<TopicContent[]>> CreateTopic(IOperationContext operationContext, string topicId, FileInfo file);
        Task<IEnumerable<TopicDescriptor>> GetDescriptors(FileInfo file, string topicId);
    }
}