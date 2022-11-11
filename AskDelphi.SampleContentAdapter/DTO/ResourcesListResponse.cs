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
    public class ResourcesListResponse : APIResponseBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationContext"></param>
        public ResourcesListResponse(IOperationContext operationContext) : base(Constants.APIVersion1, operationContext) { }

        /// <summary>
        /// 
        /// </summary>
        public FolderDescriptor[] Folders { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ResourceDescriptor[] Resources { get; set; }
    }
}
