using AskDelphi.ContentAdapter.DTO;
using AskDelphi.ContentAdapter.ServiceModel;
using System.Threading.Tasks;

namespace AskDelphi.ContentAdapter.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationContext"></param>
        /// <param name="purpose"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        Task<string> GenerateToken(IOperationContext operationContext, string purpose, ClaimTuple[] claims);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationContext"></param>
        /// <returns></returns>
        Task InvalidateToken(IOperationContext operationContext);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationContext"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<string> Refresh(IOperationContext operationContext, string refreshToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationContext"></param>
        /// <returns></returns>
        Task<bool> ValidateToken(IOperationContext operationContext);
    }
}