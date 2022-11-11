using AskDelphi.SampleContentAdapter.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.SampleContentAdapter.ServiceModel
{
    public interface ITopicRepository
    {
        /// <summary>
        /// Returns the topics matching the specified query.
        /// </summary>
        /// <returns></returns>
        Task<SCR<IEnumerable<TopicDescriptor>>> FindMatchingTopicsAsync(IOperationContext operationContext, ContentSearchRequest query);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<SCR<IEnumerable<FolderDescriptor>>> FindFoldersAsync(IOperationContext operationContext, string folderId);

        /// <summary>
        /// Refresh the repository's contents from the source.
        /// </summary>
        /// <returns></returns>
        Task<SCR<bool>> RefreshAsync(IOperationContext operationContext);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationContext"></param>
        /// <param name="topicId"></param>
        /// <returns></returns>
        Task<SCR<TopicMetadata>> GetMetadata(IOperationContext operationContext, string topicId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationContext"></param>
        /// <param name="topicId"></param>
        /// <returns></returns>
        Task<SCR<TopicContent[]>> GetContent(IOperationContext operationContext, string topicId);
    }
}
