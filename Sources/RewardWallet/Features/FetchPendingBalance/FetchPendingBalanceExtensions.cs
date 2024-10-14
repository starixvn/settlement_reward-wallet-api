using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace RewardWallet.Features;

public static class FetchPendingBalanceExtensions
{
    public static IServiceCollection AddFetchPendingBalance(this IServiceCollection services)
    {
        services
            .AddScoped<FetchPendingBalanceHandler>();

        return services;
    }

    public static IEndpointRouteBuilder MapFetchBalance(this IEndpointRouteBuilder endpoints)
    {
        _ = endpoints.MapGet(
            "pending-balance/{custId}",
            ([AsParameters] FetchPendingBalanceQuery query,
                [FromServices] FetchPendingBalanceHandler handler,
                CancellationToken cancellation)
                    => handler.HandleAsync(query, cancellation));

        return endpoints;
    }
}
