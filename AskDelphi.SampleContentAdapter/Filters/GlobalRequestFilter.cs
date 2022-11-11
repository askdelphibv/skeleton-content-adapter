using AskDelphi.SampleContentAdapter.DTO;
using AskDelphi.SampleContentAdapter.ServiceModel;
using AskDelphi.SampleContentAdapter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace AskDelphi.SampleContentAdapter.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class GlobalRequestFilter : IActionFilter
    {
        private readonly ILogger<GlobalRequestFilter> logger;
        private readonly IOperationContextFactory operationContextFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="operationContextFactory"></param>
        public GlobalRequestFilter(ILogger<GlobalRequestFilter> logger, IOperationContextFactory operationContextFactory)
        {
            this.logger = logger;
            this.operationContextFactory = operationContextFactory;
        }

        private Guid GuidOrDefault(string value)
        {
            if (Guid.TryParse(value ?? "", out Guid result))
            {
                return result;
            }
            return Guid.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            IOperationContext operationContext = operationContextFactory.CreateOperationContext(context.HttpContext);
            context.HttpContext.SetOperationContext(operationContext);

            logger.LogInformation($"START,{operationContext.OperationId},{context.HttpContext.Request.Method},{context.HttpContext.Request.Path},{context.HttpContext.Request.QueryString}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            IOperationContext operationContext = context.HttpContext.GetOperationContext();
            string operationId = string.Empty;
            if (null != operationContext)
            {
                logger.LogInformation($"END,{operationContext.OperationId},{context.HttpContext.Request.Method},{context.HttpContext.Request.Path},{context.HttpContext.Request.QueryString}");
                operationContext.Dispose();
            }

            if (context.Exception != null)
            {
                logger.LogError(context.Exception, $"Unhandled exception in operation: {operationContext.OperationId}");
                var errorResponse = new ErrorResponse(operationContext)
                {
                    Code = Constants.ErrorInternal,
                    Message = Constants.ErrorInternalMessage,
                    Success = false
                };
                context.Result = new JsonResult(errorResponse);
                context.Exception = null;
            }
        }
    }
}
