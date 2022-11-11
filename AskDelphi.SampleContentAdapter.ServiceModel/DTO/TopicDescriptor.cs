using System;

namespace AskDelphi.SampleContentAdapter.DTO
{
    /// <summary>
    /// 
    /// </summary>
    public class TopicDescriptor
    {
        /// <summary>
        /// hould provide enough information for the implementing system to uniquely identify the topic.
        /// </summary>
        public string TopicId { get; set; }
        /// <summary>
        /// The display name of the topic, as it’s shown to the user
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// A short string identifying the status of this document. This is shown in the  AskDelphi authoring environment as additional information to help the user select the resource they want.
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Topic type programmatic title, as used in content design of the target project.
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// The namesapace for the topic.
        /// </summary>
        public string Namespace { get; set; }
        /// <summary>
        /// Version string in the form “<major>.<minor>” representing the version of the object as reported earlier in the metadata.
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// The topic guid
        /// </summary>
        public Guid Guid { get; set; }
    }
}