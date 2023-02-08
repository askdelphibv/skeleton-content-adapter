using AskDelphi.ContentAdapter.ServiceModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskDelphi.ContentAdapter
{
    /// <summary></summary>
    public static class HttpContextExtensions
    {
        const string ContextKeyOperationContext = "OperationContext";

        /// <summary></summary>
        public static IOperationContext GetOperationContext(this HttpContext context)
        {
            return context.Items[ContextKeyOperationContext] as IOperationContext;
        }

        /// <summary></summary>
        public static void SetOperationContext(this HttpContext context, IOperationContext operationContext)
        {
            context.Items[ContextKeyOperationContext] = operationContext;
        }
    }
}
