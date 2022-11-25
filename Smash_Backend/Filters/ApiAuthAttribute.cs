using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Smash_Backend.Filters
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiAuthAttribute : Attribute, IAsyncActionFilter
    {
        private const string ApiClientIDHeaderName = "client_id";
        private const string ApiSecretIDHeaderName = "client_secret";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!(context.HttpContext.Request.Headers.TryGetValue(ApiClientIDHeaderName, out var ClientId)) || !(context.HttpContext.Request.Headers.TryGetValue(ApiSecretIDHeaderName, out var SecretId)))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var _config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
           
            var clientKey = _config.GetSection("appConfiguration:ApiClientID") != null ? _config.GetSection("appConfiguration:ApiClientID").Value : "";
            var secretKey = _config.GetSection("appConfiguration:ApiSecretId") != null ? _config.GetSection("appConfiguration:ApiSecretId").Value : "";

            if(!(clientKey.Equals(ClientId)) || !(secretKey.Equals(SecretId)))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            await next();            
        }
    }
}
