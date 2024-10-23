using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace RewardWallet.Features.FetchSabaWalletTickets;

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
        _ = endpoints.MapPost(
            "/get-saba-wallet-tickets",
            (FetchSabaWalletTicketsQuery query,
                FetchSabaWalletTicketsHandler handler,
                CancellationToken cancellation)
                    => handler.HandleAsync(query, cancellation));

        return endpoints;
    }
}