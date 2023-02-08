namespace AskDelphi.ContentAdapter.DTO
{
    /// <summary>
    /// 
    /// </summary>
    public class TopicContentBasicData
    {
        /// <summary>
        /// 
        /// </summary>
        public string TopicTitle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TopicTitleMarkup { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ModificationDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TopicType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string[] MetricsTags { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsPublished { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsEmpty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDescriptionCalculated { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Namespace { get; set; }
    }
}