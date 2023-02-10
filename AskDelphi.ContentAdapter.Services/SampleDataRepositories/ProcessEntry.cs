using System.Collections.Generic;

namespace AskDelphi.ContentAdapter.Services.SampleDataRepositories
{
    public partial class TopicFactory
    {
        public class ProcessEntry
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string Thumbnail { get; set; }
            public string Content { get; set; }
            public IEnumerable<ProcessEntry> Tasks { get; set; }
        }
    }
}