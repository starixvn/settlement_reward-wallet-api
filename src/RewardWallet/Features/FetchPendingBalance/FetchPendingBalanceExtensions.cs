using Core.EndpointFilters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace RewardWallet.Features.FetchPendingBalance;

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
        _ = endpoints.MapPost(
            "/get-pending-balance",
            (FetchPendingBalanceQuery query,
                FetchPendingBalanceHandler handler,
                CancellationToken cancellation)
                    => handler.HandleAsync(query, cancellation))
            .AddEndpointFilter<LogRequestFilter>();

        return endpoints;
    }
}