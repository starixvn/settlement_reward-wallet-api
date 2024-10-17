using System.Text;
using System.Text.Json;
using Core.Models;
using Fanex.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace Core.EndpointFilters;

public class LogRequestFilter(IOptions<AppSettings> appSettingsKey) : IEndpointFilter
{
    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        if (appSettingsKey.Value.EnableLogRequest)
        {
            var model = context.Arguments[0];
            var serialString = JsonSerializer.Serialize(model);
            var url = context.HttpContext.Request.GetDisplayUrl();
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"RewardWalletApi - Url: {url}");
            stringBuilder.AppendLine($"Params: {serialString}");

            await new Logger().InfoAsync(stringBuilder.ToString());
        }

        return await next(context);
    }
}

public static class LogRequestFilterExtensions
{
    public static RouteGroupBuilder AddLogRequestFilter(this RouteGroupBuilder builder)
    {
        builder.AddEndpointFilter<LogRequestFilter>();
        return builder;
    }
}