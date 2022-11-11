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
    public class ResourcesSearchResponse : APIResponseBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationContext"></param>
        public ResourcesSearchResponse(IOperationContext operationContext) : base(Constants.APIVersion1, operationContext) { }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ContinuationToken { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ResourceDescriptor[] Resouces { get; set; }
    }
}
