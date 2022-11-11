using System;
using System.Collections.Generic;

namespace AskDelphi.SampleContentAdapter.DTO
{
    /// <summary>
    /// 
    /// </summary>
    public class TopicContentRelationReference
    {
        public TopicContentRelationReference()
        {
            Metadata = new();
        }

        /// <summary>
        /// 
        /// </summary>
        public List<KeyValuePair> Metadata { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SequenceNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string View { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Use { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TargetTopicNamespaceUri { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TargetTopicTitle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid TargetTopicGuid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid RelationTypeKey { get; set; } // from content design topic type allowed relations????
        /// <summary>
        /// Pyramid level for the relation
        /// </summary>
        public string PyramidLevel { get; set; }
        /// <summary>
        /// Should be set to a true value if the relation is inferred from the content and is not an actual 'pyramid-level' relation.
        /// </summary>
        public bool IsInferredFromContent { get; set; }
    }
}