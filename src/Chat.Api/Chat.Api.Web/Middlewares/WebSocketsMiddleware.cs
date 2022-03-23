using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Chat.Api.Web.Middlewares
{
    public class WebSocketsMiddleware
    {
        private readonly RequestDelegate _next;

        public WebSocketsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var request = httpContext.Request;

            // web sockets cannot pass headers so we must take the access token from query param and
            // add it to the header before authentication middleware runs
            //if (request.Path.StartsWithSegments("/chatsocket", StringComparison.OrdinalIgnoreCase) &&
            //    request.Query.TryGetValue("access_token", out var accessToken))
            //{
            //    request.Headers.Add("Authorization", $"Bearer {accessToken}");
            //}

            if (request.Path.StartsWithSegments("/chatsocket", StringComparison.OrdinalIgnoreCase) &&
                request.Query.TryGetValue("userId", out var userId) &&
                request.Query.TryGetValue("apiKey", out var apiKey) &&
                request.Query.TryGetValue("appId", out var appId))
            {

                request.Headers.Add("x-api-key", apiKey);
                request.Headers.Add("x-app-id", appId);
                request.Headers.Add("x-user-id", userId);
            }

            //if (request.Path.StartsWithSegments("/chatsocket", StringComparison.OrdinalIgnoreCase))
            //{
            //    var headerKey = httpContext.Request.Headers["x-api-key"].FirstOrDefault();
            //    var appId = httpContext.Request.Headers["x-app-id"].FirstOrDefault();
            //    var userId = httpContext.Request.Headers["x-user-id"].FirstOrDefault();

            //    request.Headers.Add("x-api-key", headerKey);
            //    request.Headers.Add("x-app-id", appId);
            //    request.Headers.Add("x-user-id", userId);
            //}

            await _next(httpContext);
        }
    }
}
