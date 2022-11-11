﻿using AskDelphi.SampleContentAdapter.DTO;
using AskDelphi.SampleContentAdapter.ServiceModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AskDelphi.SampleContentAdapter.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILogger<AuthenticationService> logger;
        private readonly IConfiguration configuration;
        private readonly ITokenService tokenService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        /// <param name="tokenService"></param>
        public AuthenticationService(ILogger<AuthenticationService> logger, IConfiguration configuration, ITokenService tokenService)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.tokenService = tokenService;
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task<bool> Login(IOperationContext operationContext, AuthLoginResponse response, KeyValuePair<string, StringValues> authHeader, ClaimTuple[] claims)
        {
            // NOTE: claims are ignored here for now, this specific adapter does not need them

            if (authHeader.Value.Count == 0)
            {
                logger.LogWarning($"{operationContext.OperationId} no Authorization header found in request");
                return false;
            }

            if (authHeader.Value.Count != 1)
            {
                logger.LogWarning($"{operationContext.OperationId} too many Authorization headers found in request");
                return false;
            }

            string headerValue = authHeader.Value.FirstOrDefault();
            if (string.IsNullOrEmpty(headerValue))
            {
                logger.LogWarning($"{operationContext.OperationId} Authorization header does not contain any API key values");
                return false;
            }

            if (headerValue.StartsWith("Bearer ", StringComparison.InvariantCultureIgnoreCase))
            {
                headerValue = headerValue.Substring("Bearer ".Length);
            }

            string[] keyParts = headerValue.Split(":");
            if (keyParts.Length != 2)
            {
                logger.LogWarning($"{operationContext.OperationId} Authorization key value is not in the form Purpose:Base64APIKey");
                return false;
            }

            (string purpose, string apiKey) = (keyParts[0], keyParts[1]);

            string registeredKey = GetKeyFromDictionaryForPurpose(purpose);
            if (string.IsNullOrWhiteSpace(registeredKey))
            {
                logger.LogWarning($"{operationContext.OperationId} Authorization key value is not in the key dictionary for the specified purpose");
                return false;
            }

            if (!string.Equals(apiKey, registeredKey))
            {
                logger.LogWarning($"{operationContext.OperationId} Authorization key does not match the one in the key dictionary for the specified purpose");
                return false;
            }

            response.Token = await tokenService.GenerateToken(operationContext, purpose, claims);

            logger.LogInformation($"{operationContext.OperationId} allowing login for session with purpose {purpose}");
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationContext"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public async Task<bool> Logout(IOperationContext operationContext, AuthLogoutResponse response)
        {
            await tokenService.InvalidateToken(operationContext);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationContext"></param>
        /// <param name="response"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public async Task<bool> Refresh(IOperationContext operationContext, AuthRefreshResponse response, string refreshToken)
        {
            string newToken = await tokenService.Refresh(operationContext, refreshToken);
            if (string.IsNullOrEmpty(newToken))
            {
                response.Token = null;
                return false;
            }
            else
            {
                response.Token = newToken;
                return true;
            }
        }


        private string GetKeyFromDictionaryForPurpose(string purpose)
        {
            string result = configuration.GetValue<string>($"Keys:{purpose}");
            return result;
        }
    }
}
