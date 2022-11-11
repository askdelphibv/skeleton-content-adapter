using AskDelphi.SampleContentAdapter.ServiceModel;
using AskDelphi.SampleContentAdapter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskDelphi.SampleContentAdapter.DTO
{
    /// <summary>
    /// 
    /// </summary>
    public class ContentSearchResponse : APIResponseBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationContext"></param>
        public ContentSearchResponse(IOperationContext operationContext) : base(Constants.APIVersion1, operationContext) { }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string ContinuationToken { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TopicDescriptor[] Topics { get; set; }

        internal int Initialize(SCR<IEnumerable<TopicDescriptor>> scr)
        {
            if (!scr.IsError)
            {
                this.TotalCount = scr.Result?.Count() ?? 0;
                this.ContinuationToken = string.Empty;
                this.Topics= scr.Result?.ToArray() ?? new TopicDescriptor[] { };
            }
            return base.InitializeFromSCR(scr);
        }
    }
}
