using ExpenseTrackerApi.Controllers;
using ExpenseTrackerApi.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApi.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly CheckMiddlewareClaimsService _checkMiddlewareClaimsService;
        private readonly DecryptService _decryptService;
        private readonly IConfiguration _configuration;
        public AuthenticationMiddleware(RequestDelegate next, CheckMiddlewareClaimsService checkMiddlewareClaimsService, DecryptService decryptService, IConfiguration configuration)
        {
            _next = next;
            _checkMiddlewareClaimsService = checkMiddlewareClaimsService;
            _decryptService = decryptService;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            string? authHeader = context.Request.Headers["Authorization"];
            string requestPath = context.Request.Path;
            if (requestPath == "/api/User/account/login" || requestPath == "/api/User/account/register" || requestPath == "/api/encrypt" || requestPath == "/api/decrypt" || requestPath == "/weatherforecast" || requestPath == "/favicon.ico" || requestPath.Contains(".png") || requestPath.Contains(".jpeg") || requestPath.Contains(".jpg"))
            {
                await _next.Invoke(context);
            }
            else
            {
                if (authHeader != null && authHeader.StartsWith("Bearer"))
                {
                    string[] header_and_token = authHeader.Split(' ');
                    string header = header_and_token[0];
                    string token = header_and_token[1];
                    TokenValidationController tokenVaidationController = new(_checkMiddlewareClaimsService, _decryptService, _configuration);
                    ObjectResult objectResult = (ObjectResult)await tokenVaidationController.ValidateJwtToken(context);

                    if (objectResult.StatusCode == 200)
                    {
                        if (requestPath == "/api/jwt")
                        {
                            context.Response.StatusCode = 200;
                            return;
                        }
                        else
                        {
                            await _next.Invoke(context);
                        }

                    }
                    else
                    {
                        context.Response.StatusCode = 401;
                        return;
                    }
                }
                //if authHeader null or not Bearer
                else
                {
                    context.Response.StatusCode = 401;
                    return;
                }
            }
        }
    }
}