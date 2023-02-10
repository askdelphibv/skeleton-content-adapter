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

        internal int Initialize(SCR<(IEnumerable<FolderDescriptor> folders, IEnumerable<ResourceDescriptor> resources)> scr)
        {
            if (!scr.IsError)
            {
                Folders = (scr.Result.folders ?? new FolderDescriptor[] { }).Select(x => x).ToArray();
                Resources= (scr.Result.resources ?? new ResourceDescriptor[] { }).Select(x => x).ToArray();
            }
            return base.InitializeFromSCR(scr);
        }
    }
}
