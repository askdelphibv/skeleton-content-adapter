using AskDelphi.ContentAdapter.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.ContentAdapter.ServiceModel
{
    /// <summary>
    /// Interface to the tropic repository for this adapter. 
    /// </summary>
    public interface ITopicRepository
    {
        /// <summary>
        /// Returns the descriptions of the topics that are matching the specified search query.
        /// </summary>
        /// <returns>An enumeration of matching topics, or an empty collection. Never null.</returns>
        Task<SCR<IEnumerable<TopicDescriptor>>> FindMatchingTopicsAsync(IOperationContext operationContext, ContentSearchRequest query);

        /// <summary>
        /// Returns the descriptions of the sub-folders of the folder id.
        /// </summary>
        /// <remarks>The folder id may be null or empty, in which case the root folders (if any) should be returned.</remarks>
        /// <returns>An enumeration of matching folders, or an empty collection. Never null. May return a 404 status if the folder with this id does not exist.</returns>
        Task<SCR<IEnumerable<FolderDescriptor>>> FindFoldersAsync(IOperationContext operationContext, string folderId);

        /// <summary>
        /// Periodically invoked from the background to trigger a refresh of any cached content in the repository. Also invoked once a short while after system start-up (ContentUpdates:InitialDelay setting) to triggger initial loading if needed.
        /// </summary>
        /// <returns>True if refresh was started, false otherwise.</returns>
        Task<SCR<bool>> RefreshAsync(IOperationContext operationContext);

        /// <summary>
        /// Returns topic metadata for the specified topic.
        /// </summary>
        /// <param name="operationContext"></param>
        /// <param name="topicId">May not be null or empty, must be a valid topic ID.</param>
        /// <returns>Returns the topic metadata that belongs ot th etopic with this ID. May return a 404 status if the folder with this id does not exist.</returns>
        Task<SCR<TopicMetadata>> GetMetadata(IOperationContext operationContext, string topicId);

        /// <summary>
        /// Returns an array containing the topic content for the topic with the specified ID, plus all topics that are aggregated intot his topic.
        /// </summary>
        /// <remarks>
        /// The returned array contains the one topic that was requested as a first element. Otherwise a 404 is returned.
        /// The returned array contains all other topics that are considered part of this content; these can't (and need not) be separately offerred in a folder or via search.
        /// The returned topics do need a unique and predictable topic guid and topic id. The same topic gets the same guid every time it is retrieved. Also for every entry in the resulting array, the topic ID that is returned must be usable to fetch the topic again later.
        /// </remarks>
        /// <param name="operationContext"></param>
        /// <param name="topicId"></param>
        /// <returns></returns>
        Task<SCR<TopicContent[]>> GetContent(IOperationContext operationContext, string topicId);

        /// <summary>
        /// Returns an indication as to when a topic was last updated. Returns a null result in the SCR if that time is not known. This may be used in the publication process to determine if a topic needs to be updated.
        /// </summary>
        /// <param name="operationContext"></param>
        /// <param name="topicId"></param>
        /// <returns>A 404 result if the topic does not exist</returns>
        Task<SCR<DateTimeOffset?>> GetContentLastChangedDate(IOperationContext operationContext, string topicId);
    }
}
