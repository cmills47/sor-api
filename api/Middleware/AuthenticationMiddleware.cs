using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SiteOfRefuge.API.Middleware
{
    /// <summary>
    /// BearerTokenAuth Middleware.
    /// </summary>
    public class AuthenticationMiddleware
        : IFunctionsWorkerMiddleware
    {
        private readonly ILogger<AuthenticationMiddleware> logger;
        private readonly JwtSecurityTokenHandler _tokenValidator;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly ConfigurationManager<OpenIdConnectConfiguration> _configurationManager;

        public AuthenticationMiddleware(IConfiguration configuration, ILogger<AuthenticationMiddleware> logger)
        {
            var authority = configuration["AuthenticationAuthority"];   // ie: "iss" claim which represents the AADB2C tenant
            var audience = configuration["AuthenticationClientId"];     // ie: "aud" claim which represents the app registered in AADB2C

            _tokenValidator = new JwtSecurityTokenHandler();
            
            // We need to clear the ClaimsType map so we don't cause "sub" to map to
            // http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier
            _tokenValidator.InboundClaimTypeMap.Clear();    

            _tokenValidationParameters = new TokenValidationParameters
            {
                RequireSignedTokens = true,
                ValidAudience = audience,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true
            };

            // https://siteofrefugeb2c.b2clogin.com/siteofrefugeb2c.onmicrosoft.com/b2c_1_sms_registry/v2.0/.well-known/openid-configuration
            _configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                $"{authority}/.well-known/openid-configuration",
                new OpenIdConnectConfigurationRetriever());

            this.logger = logger;
        }
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            if( !GetTokenFromHeaders(context, out var token) ) 
            {
                logger.LogInformation( $"{context.InvocationId.ToString()} - Cannot find bearer token");                
                await context.CreateJsonResponse(System.Net.HttpStatusCode.Unauthorized, new { Message = "Cannot find bearer token" });
                return;
            }

            if (!_tokenValidator.CanReadToken(token))
            {
                // Token is malformed
                logger.LogInformation( $"{context.InvocationId.ToString()} - Malformed bearer token");
                await context.CreateJsonResponse(System.Net.HttpStatusCode.Unauthorized, new { Message = "Malformed bearer token" });                
                return;
            }

            // Get OpenID Connect metadata
            var validationParameters = _tokenValidationParameters.Clone();
            var openIdConfig = await _configurationManager.GetConfigurationAsync(default);
            validationParameters.ValidIssuer = openIdConfig.Issuer;
            validationParameters.IssuerSigningKeys = openIdConfig.SigningKeys;

            try
            {
                // Validate token
                var principal = _tokenValidator.ValidateToken( token, validationParameters, out _);

                // Set principal + token in Features collection
                // They can be accessed from here later in the call chain
                context.Features.Set(new JwtPrincipalFeature(principal, token));

                // Maybe use lookup for role in app and leverage: context.Items.Add("UserRole", "Refugee");

                await next(context);
            }
            catch (SecurityTokenException)
            {
                // Token is not valid (expired etc.)
                logger.LogInformation( $"{context.InvocationId.ToString()} - Invalid bearer token");                
                await context.CreateJsonResponse(System.Net.HttpStatusCode.Unauthorized, new { Message = "Invalid bearer token" });
                return;
            }
        }

        private static bool GetTokenFromHeaders(FunctionContext context, out string token)
        {
            token = null;
            // HTTP headers are in the binding context as a JSON object
            // The first checks ensure that we have the JSON string
            if (!context.BindingContext.BindingData.TryGetValue("Headers", out var headersObj))
            {
                return false;
            }

            if (headersObj is not string headersStr)
            {
                return false;
            }

            // Deserialize headers from JSON
            var headers = JsonSerializer.Deserialize<Dictionary<string, string>>(headersStr);
            var normalizedKeyHeaders = headers.ToDictionary(h => h.Key.ToLowerInvariant(), h => h.Value);
            if (!normalizedKeyHeaders.TryGetValue("authorization", out var authHeaderValue))
            {
                // No Authorization header present
                return false;
            }

            if (!authHeaderValue.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                // Scheme is not Bearer
                return false;
            }

            token = authHeaderValue.Substring("Bearer ".Length).Trim();
            return true;
        }
    }
}