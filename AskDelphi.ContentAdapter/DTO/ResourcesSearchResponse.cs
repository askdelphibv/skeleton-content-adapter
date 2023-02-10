using AskDelphi.ContentAdapter.ServiceModel;
using AskDelphi.ContentAdapter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskDelphi.ContentAdapter.DTO
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

        internal int Initialize(int page, SCR<(IEnumerable<ResourceDescriptor> resources, int totalCount, string continuationToken)> scr)
        {
            if (!scr.IsError)
            {
                TotalCount = scr.Result.totalCount;
                Page = page;
                ContinuationToken = scr.Result.continuationToken;
                Resouces = (scr.Result.resources ?? new ResourceDescriptor[] { }).ToArray();
            }
            return base.InitializeFromSCR(scr);
        }
    }
}
