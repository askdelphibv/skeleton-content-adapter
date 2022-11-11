using AskDelphi.SampleContentAdapter.ServiceModel;
using AskDelphi.SampleContentAdapter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskDelphi.SampleContentAdapter.DTO
{
    /// <summary></summary>
    public class AuthRefreshResponse : APIResponseBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationContext"></param>
        public AuthRefreshResponse(IOperationContext operationContext) : base(Constants.APIVersion1, operationContext) { }

        /// <summary>
        /// 
        /// </summary>
        public string Token { get; set; }

    }
}
