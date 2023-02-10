using System.Collections.Generic;

namespace AskDelphi.ContentAdapter.Services.SampleDataRepositories
{
    /// <summary>
    /// Process or Task entries in our JSON file in our same data folder use this structure.
    /// </summary>
    public class ProcessEntry
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        public string Content { get; set; }
        public IEnumerable<ProcessEntry> Tasks { get; set; }
    }
}