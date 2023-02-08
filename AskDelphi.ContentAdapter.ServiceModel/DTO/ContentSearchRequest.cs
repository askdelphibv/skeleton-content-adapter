using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskDelphi.ContentAdapter.DTO
{
    /// <summary>
    /// 
    /// </summary>
    public class ContentSearchRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public string FolderId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Query { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> TopicTypes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<TaxonomyValues> Tags { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ContinuationToken { get; set; }

    }
}
