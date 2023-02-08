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
    public class ContentContentResponse : APIResponseBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationContext"></param>
        public ContentContentResponse(IOperationContext operationContext) : base(Constants.APIVersion1, operationContext) { }

        /// <summary>
        /// 
        /// </summary>
        public TopicContent[] Contents { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal int Initialize(SCR<TopicContent[]> scr)
        {
            if (!scr.IsError)
            {
                Contents = scr.Result?.ToArray();
            }
            return base.InitializeFromSCR(scr);
        }
    }
}
