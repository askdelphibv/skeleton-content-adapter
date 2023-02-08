using AskDelphi.ContentAdapter.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskDelphi.ContentAdapter.DTO
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorResponse : APIResponseBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationContext"></param>
        public ErrorResponse(IOperationContext operationContext) : base("1.0", operationContext)
        {
        }
    }
}
