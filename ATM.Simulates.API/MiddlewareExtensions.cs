using Microsoft.AspNetCore.Builder;

namespace ATM.Simulates.API
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseExcuteMiddleware(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<RequestMiddleware>();
            builder.UseMiddleware<ResponseMiddleware>();
            return builder;
        }
    }
}