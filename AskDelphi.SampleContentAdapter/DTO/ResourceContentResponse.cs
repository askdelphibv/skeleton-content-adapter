using AskDelphi.SampleContentAdapter.ServiceModel;
using AskDelphi.SampleContentAdapter.ServiceModel.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AskDelphi.SampleContentAdapter.DTO
{
    /// <summary>
    /// Resource content response
    /// </summary>
    public class ResourceContentResponse: APIResponseBase
    {
        /// <summary></summary>
        public ResourceContentResponse(IOperationContext operationContext) : base(Constants.APIVersion1, operationContext) { }

        /// <summary></summary>
        internal int Initialize(SCR<ResourceContent> scr)
        {
            return base.InitializeFromSCR(scr);
        }
    }
}
