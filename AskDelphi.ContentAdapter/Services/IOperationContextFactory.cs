using AskDelphi.ContentAdapter.ServiceModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskDelphi.ContentAdapter.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOperationContextFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        IOperationContext CreateOperationContext(HttpContext context);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IOperationContext CreateBackgroundOperationContext();
    }
}
