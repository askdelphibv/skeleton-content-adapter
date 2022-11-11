using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AskDelphi.SampleContentAdapter.DTO
{
    /// <summary>
    /// 
    /// </summary>
    public class TopicContentRelationData
    {
        public TopicContentRelationData()
        {
            References = new();
        }

        /// <summary>
        /// 
        /// </summary>
        public string KeyVisualGuid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string keyVisualVisualization { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ThumbnailTopicGuid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<TopicContentRelationReference> References { get; set; }

        public void AddReference(TopicContentRelationReference reference)
        {
            // Add a clone, do not add or modify the original
            TopicContentRelationReference clone = JsonConvert.DeserializeObject<TopicContentRelationReference>(JsonConvert.SerializeObject(reference));

            if (clone.Metadata == null || !clone.Metadata.Any(md => md.Key == "classes"))
            {
                List<KeyValuePair> metadata = new(clone.Metadata ?? new());
                metadata.Add(new KeyValuePair
                {
                    Key = "classes",
                    Value = "adapter-provided-relation"
                });
                clone.Metadata = metadata.ToList();
            }

            References.Add(clone);
        }
    }
}