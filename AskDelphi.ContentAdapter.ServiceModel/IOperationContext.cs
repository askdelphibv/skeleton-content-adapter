using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskDelphi.ContentAdapter.ServiceModel
{
    /// <summary></summary>
    public interface IOperationContext : IDisposable
    {
        /// <summary></summary>
        string OperationId { get; }

        /// <summary></summary>
        string GetAuthToken();
        /// <summary></summary>
        string GetAskDelphiSystemID();

        /// <summary></summary>
        void InitializeFromRequest(HttpRequest request);

        Guid? GetTenantGuid();
        Guid? GetProjectGuid();

        /// <summary></summary>
        bool EnableMergingFoAndLevel4EPC { get; }
    }
}
