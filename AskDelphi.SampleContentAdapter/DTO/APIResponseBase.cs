using AskDelphi.SampleContentAdapter.ServiceModel;
using AskDelphi.SampleContentAdapter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AskDelphi.SampleContentAdapter.DTO
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class APIResponseBase
    {
        private readonly string version;
        /// <summary></summary>
        protected readonly IOperationContext operationContext;

        /// <summary>
        /// True if the operation succeeded, false otherwise.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// API version used.
        /// </summary>
        public string Version => version;

        internal int InitializeFromSCR<T>(SCR<T> scr)
        {
            if (scr.IsError)
            {
                Success = false;
                Code = scr.ErrorCode;
                Message = scr.ErrorMessage;                
            }
            else
            {
                Success = true;
            }
            return (int)scr.HttpStatusCode;
        }

        /// <summary>
        /// The identifier for the operation.
        /// </summary>
        public string Id => operationContext.OperationId;

        /// <summary>
        /// When success is false, this contains the unique error code for the failure.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// When success is false, this contains the error message containing more details about what failed.
        /// </summary>
        public string Message { get; set; }

        /// <summary></summary>
        public APIResponseBase(string version, IOperationContext operationContext)
        {
            this.version = version;
            this.operationContext = operationContext;
        }
    }
}
