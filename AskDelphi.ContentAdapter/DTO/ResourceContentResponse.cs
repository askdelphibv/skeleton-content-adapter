using AskDelphi.ContentAdapter.ServiceModel;
using AskDelphi.ContentAdapter.ServiceModel.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AskDelphi.ContentAdapter.DTO
{
    /// <summary>
    /// Resource content response
    /// </summary>
    public class ResourceContentResponse: APIResponseBase
    {
        /// <summary></summary>
        public ResourceContentResponse(IOperationContext operationContext) : base(Constants.APIVersion1, operationContext) { }
    }
}
