using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SiteOfRefuge.API.Middleware
{
    /// <summary>
    /// Authorization middleware.
    /// </summary>
    public class AuthorizationMiddleware
        : IFunctionsWorkerMiddleware
    {
        private readonly ILogger<AuthorizationMiddleware> logger;

        public AuthorizationMiddleware(ILogger<AuthorizationMiddleware> logger)
        {
            this.logger = logger;
        }
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            if (context.IsHttpTriggerFunction())
            {
                var principalFeature = context.Features.Get<JwtPrincipalFeature>();
                var claimsIdentity = (ClaimsIdentity)principalFeature.Principal.Identity;
                System.Console.WriteLine( "Claims: " );
                foreach(var claim in claimsIdentity.Claims)
                {
                    System.Console.WriteLine( $"\t{claim.Type}: {claim.Value}" );
                }

                await next(context);
            }
            else
            {
                await next(context);
            }
        }
    }
}