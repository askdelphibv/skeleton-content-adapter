using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace AskDelphi.SampleContentAdapter.Services.Utilities
{
    /// <summary>
    /// Utility for manipulating topic ids
    /// </summary>
    public static class TopicIdUtils
    {
        /// <summary>
        /// Creates a valid topic id for a subtopic of a parent topic
        /// </summary>
        /// <param name="topicId">Parent topic id</param>
        /// <param name="subTopicGuid">Subtopic unique id</param>
        /// <returns></returns>
        public static string BuildTopicIdForSubTopic(string topicId, Guid subTopicGuid)
        {
            string topicUrl = $"remote-uri://{topicId.TrimStart('/')}";

            Uri topicUri = new Uri(topicUrl);
            Uri updatedTopicUri = UriUtils.AddParameter(topicUri, "subTopicGuid", $"{subTopicGuid}");
            return $"/{updatedTopicUri.Host}/{updatedTopicUri.PathAndQuery.TrimStart('/')}";

        }

        /// <summary>
        /// Returns sub-topic guid, which is part of topic id or null
        /// </summary>
        /// <param name="topicId">Target topic id</param>
        /// <returns></returns>
        public static Guid? GetSubTopicId(string topicId)
        {
            string topicUrl = $"remote-uri://{topicId.TrimStart('/')}";
            Uri topicUri = new Uri(topicUrl);

            string subTopicId = UriUtils.GetQueryParam(topicUri, "subTopicGuid");

            if (!string.IsNullOrWhiteSpace(subTopicId) && Guid.TryParse(subTopicId, out Guid result))
            {
                return result;
            }

            return null;
        }
    }
}
