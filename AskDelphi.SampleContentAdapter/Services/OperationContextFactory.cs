using AskDelphi.SampleContentAdapter.DTO;
using AskDelphi.SampleContentAdapter.ServiceModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AskDelphi.SampleContentAdapter.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class OperationContextFactory : IOperationContextFactory
    {
        private bool enableMergingFoAndLevel4EPC;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public OperationContextFactory(IConfiguration configuration)
        {
            enableMergingFoAndLevel4EPC = configuration.GetValue<bool>("EnableMergingFoAndLevel4EPC");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IOperationContext CreateBackgroundOperationContext()
        {
            return new OperationContext
            {
                EnableMergingFoAndLevel4EPC = enableMergingFoAndLevel4EPC
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public IOperationContext CreateOperationContext(HttpContext context)
        {
            var result = new OperationContext()
            {
                EnableMergingFoAndLevel4EPC = enableMergingFoAndLevel4EPC
            };
            result.InitializeClaims(context.User.Identity as ClaimsIdentity);
            return result;
        }

        private class OperationContext : IOperationContext
        {
            private string contextId = $"{Guid.NewGuid()}";
            private string authToken;
            private string askDelphiSystemID;
            private IEnumerable<ClaimTuple> claims = new ClaimTuple[] { };

            public bool EnableMergingFoAndLevel4EPC { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string OperationId => contextId;

            /// <summary>
            /// 
            /// </summary>
            public OperationContext()
            {
            }

            /// <summary>
            /// 
            /// </summary>
            public void Dispose() { }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string GetAuthToken() => authToken;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string GetAskDelphiSystemID() => askDelphiSystemID;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="request"></param>
            public void InitializeFromRequest(HttpRequest request)
            {
                this.authToken = request.Headers.FirstOrDefault(x => string.Equals(x.Key, "Authorization", StringComparison.InvariantCultureIgnoreCase)).Value.FirstOrDefault();
                if (string.IsNullOrWhiteSpace(authToken) && authToken.StartsWith("Bearer ", StringComparison.InvariantCultureIgnoreCase))
                {
                    authToken = authToken.Substring("Bearer ".Length);
                }
            }

            internal void InitializeClaims(ClaimsIdentity claimsIdentity)
            {
                askDelphiSystemID = claimsIdentity.FindFirst(Constants.ClaimTypeSystemID)?.Value;
                claims = claimsIdentity.Claims.Select(claim => new ClaimTuple { Type = claim.Type, Value = claim.Value });
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return $"{GetType().Name}(id=\"{askDelphiSystemID}\", token=\"{authToken}\", claims={{{string.Join(",", claims)}}})";
            }

            public Guid? GetTenantGuid() => AsGuid(claims.FirstOrDefault(x => x.Type == Constants.ClaimTypeTenant)?.Value);
            public Guid? GetProjectGuid() => AsGuid(claims.FirstOrDefault(x => x.Type == Constants.ClaimTypeProject)?.Value);

            private Guid? AsGuid(string value)
            {
                if (!Guid.TryParse(value ?? string.Empty, out Guid result))
                {
                    return null;
                }
                return result;
            }
        }
    }
}
