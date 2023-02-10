using AskDelphi.ContentAdapter.ServiceModel;
using System;
using System.Threading.Tasks;

namespace AskDelphi.ContentAdapter.Services.SampleDataRepositories
{
    /// <summary>
    /// Abstracts from the interface to the AskDelphi CMS.
    /// For this demo implementation we will not implement an actual live link, instead we'll pre-fetch the data.
    /// </summary>
    public interface ICMSAdapter
    {
        /// <summary>
        /// Should return a relation type key for a specific relation.
        /// </summary>
        /// <param name="operationContext"></param>
        /// <param name="processTaskRelationPyramidLevelTitle"></param>
        /// <param name="taskTopicTypeTitle"></param>
        /// <param name="taskTopicNamespace"></param>
        /// <returns></returns>
        Task<Guid> GetRelationTypeKeyFor(IOperationContext operationContext, string processTaskRelationPyramidLevelTitle, string taskTopicTypeTitle, string taskTopicNamespace, string use, string view);
    }
}