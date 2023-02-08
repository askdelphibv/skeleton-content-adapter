using AskDelphi.ContentAdapter.DTO;
using AskDelphi.ContentAdapter.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AskDelphi.ContentAdapter.Controllers
{
    /// <summary>
    /// AuthenticationController
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> logger;
        private readonly IAuthenticationService authenticationService;

        /// <summary>
        /// AuthenticationController
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="authenticationService"></param>
        public AuthenticationController(ILogger<AuthenticationController> logger, IAuthenticationService authenticationService)
        {
            this.logger = logger;
            this.authenticationService = authenticationService;
        }

        /// <summary>
        /// This method can be used to ‘log in’ to the adapter using the secret API key.
        /// </summary>
        /// <remarks>
        /// This method can be used to ‘log in’ to the adapter using the secret API key 
        /// that should only be known to systems that have access to the API.
        /// 
        /// If claims are specified, the API may encode some details of the logged-in 
        /// user in the claims part of the returned JWT token.Having those details in
        /// there could allow other calls to this API to customize results based on the
        /// logged-in user’s authentication claims.
        /// 
        /// A refresh token may be returned to allow an implementation to extend the
        /// lifetime of a JWT token. Supporting this functionality is entirely optional. 
        /// Functionally there is no need to support refresh tokens, as the caller should
        /// be able to use the login endpoint again to get a new token.
        /// 
        /// The lifetime of the returned JWT token should be limited and should not
        /// exceed 60 minutes.
        /// 
        /// JWT Tokens must comply with[RFC7519] and must include “aud”, “nbf” and 
        /// “exp” fields
        /// </remarks>
        /// <param name="claims">JSON-encoded array of tuples { "type": &lt;string>, "value": &lt;string> } 
        /// containing the claims of the currently logged-in user.The adapter
        /// that implements this function may use this to determine the user’s
        /// identity on the customer CMS.
        /// The caller of this API will always include a special claim of type
        /// http://tempuri.org/askdelphi/remote-system-id with a string value. 
        /// This special claim provides the system with an identification of itself
        /// inside the AskDelphi environment.
        /// If the content APIs need to return automatically generated remote
        /// content objects, they can include the value of this claim as an ID. If
        /// the claim is not present, they can use an empty string.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<AuthLoginResponse> PostAuthLogin([FromBody(EmptyBodyBehavior = Microsoft.AspNetCore.Mvc.ModelBinding.EmptyBodyBehavior.Allow)] ClaimTuple[] claims = null)
        {
            var authHeader = Request.Headers.FirstOrDefault(x => string.Equals(x.Key, "Authorization", StringComparison.InvariantCultureIgnoreCase));

            AuthLoginResponse response = new AuthLoginResponse(HttpContext.GetOperationContext());
            bool success = await authenticationService.Login(HttpContext.GetOperationContext(), response, authHeader, claims);
            if (success)
            {
                response.Success = true;
                Response.StatusCode = StatusCodes.Status200OK;
            }
            else
            {
                response.Success = false;
                response.Code = Constants.ErrorInvalidCredentialsCode;
                response.Message = Constants.ErrorInvalidCredentialsMessage;
                Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            return response;
        }

        
        /// <summary>
        /// This method can be used to log out a token, invalidating the token immediately.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("logout")]
        [Authorize]
        public async Task<AuthLogoutResponse> GetAuthLogout()
        {
            AuthLogoutResponse response = new AuthLogoutResponse(HttpContext.GetOperationContext());
            bool success = await authenticationService.Logout(HttpContext.GetOperationContext(), response);
            if (success)
            {
                response.Success = true;
                Response.StatusCode = StatusCodes.Status200OK;
            }
            else
            {
                response.Success = false;
                response.Code = Constants.ErrorFailedCode;
                response.Message = Constants.ErrorFailedMessage;
                Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            return response;
        }

        /// <summary>
        /// This method is not currently supported by this adapter.
        /// </summary>
        /// <param name="refresh"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("refresh")]
        [Authorize]
        public async Task<AuthRefreshResponse> GetAuthRefresh([FromQuery] string refresh)
        {
            AuthRefreshResponse response = new AuthRefreshResponse(HttpContext.GetOperationContext());
            bool success = await authenticationService.Refresh(HttpContext.GetOperationContext(), response, refresh);
            if (success)
            {
                response.Success = true;
                Response.StatusCode = StatusCodes.Status200OK;
            }
            else
            {
                response.Success = false;
                response.Code = Constants.ErrorNotSupportedCode;
                response.Message = Constants.ErrorNotSupportedMessage;
                Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            return response;
        }
    }
}
