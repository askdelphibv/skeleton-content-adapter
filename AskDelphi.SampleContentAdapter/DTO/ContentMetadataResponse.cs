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
    public class ContentMetadataResponse : APIResponseBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationContext"></param>
        public ContentMetadataResponse(IOperationContext operationContext) : base(Constants.APIVersion1, operationContext) { }

        /// <summary>
        /// 
        /// </summary>
        public TopicMetadata Meta { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal int Initialize(SCR<TopicMetadata> scr)
        {
            if (!scr.IsError)
            {
                Meta = scr.Result;
            }
            return base.InitializeFromSCR(scr);
        }
    }
}
