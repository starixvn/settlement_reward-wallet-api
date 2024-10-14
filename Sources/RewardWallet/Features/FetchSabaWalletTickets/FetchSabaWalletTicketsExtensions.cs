using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace RewardWallet.Features;

public static class FetchSabaWalletTicketsExtensions
{
    public static IServiceCollection AddFetchSabaWalletTickets(this IServiceCollection services)
    {
        services
            .AddScoped<FetchSabaWalletTicketsHandler>();

        return services;
    }

    public static IEndpointRouteBuilder MapFetchSabaWalletTickets(this IEndpointRouteBuilder endpoints)
    {
        _ = endpoints.MapGet(
            "/saba-wallet-tickets",
            ([AsParameters] FetchSabaWalletTicketsQuery query,
                [FromServices] FetchSabaWalletTicketsHandler handler,
                CancellationToken cancellation)
                    => handler.HandleAsync(query, cancellation));

        return endpoints;
    }
}
