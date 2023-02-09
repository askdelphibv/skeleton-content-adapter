using System;

namespace AskDelphi.ContentAdapter.DTO
{
    /// <summary>
    /// 
    /// </summary>
    public class TopicContent
    {
        public TopicContent()
        {
            Relations = new();
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TopicContentBasicData BasicData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TopicContentRelationData Relations { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TopicId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Content { get; set; }

        public override string ToString()
        {
            return $"{GetType().Name}, Guid={Guid}, Title={BasicData?.TopicTitle}, ID={TopicId}";
        }
    }
}