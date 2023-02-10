using AskDelphi.ContentAdapter.ServiceModel;
using AskDelphi.ContentAdapter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;

namespace AskDelphi.ContentAdapter.DTO
{
    /// <summary>
    /// 
    /// </summary>
    public class ResourcesMetadataResponse : APIResponseBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationContext"></param>
        public ResourcesMetadataResponse(IOperationContext operationContext) : base(Constants.APIVersion1, operationContext) { }

        /// <summary>
        /// 
        /// </summary>
        public ResourceMetadata Meta { get; set; }

        internal int Initialize(SCR<ResourceMetadata> scr)
        {
            if (!scr.IsError)
            {
                Meta = scr.Result;
            }
            return base.InitializeFromSCR(scr);
        }
    }
}
