using AskDelphi.ContentAdapter.ServiceModel;
using AskDelphi.ContentAdapter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AskDelphi.ContentAdapter.DTO
{
    /// <summary>
    /// 
    /// </summary>
    public class ContentFoldersResponse : APIResponseBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationContext"></param>
        public ContentFoldersResponse(IOperationContext operationContext) : base(Constants.APIVersion1, operationContext) { }

        /// <summary>
        /// 
        /// </summary>
        public FolderDescriptor[] Folders { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal int Initialize(SCR<IEnumerable<FolderDescriptor>> scr)
        {
            if (!scr.IsError)
            {
                this.Folders = scr.Result?.ToArray() ?? new FolderDescriptor[] { };
            }
            return base.InitializeFromSCR(scr);
        }
    }
}
