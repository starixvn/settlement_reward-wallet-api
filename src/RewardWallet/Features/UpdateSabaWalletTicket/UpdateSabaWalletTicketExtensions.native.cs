using Core.EndpointFilters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace RewardWallet.Features.UpdateSabaWalletTicket;

public static class UpdateSabaWalletTicketExtensions
{
    public static IServiceCollection AddUpdateSabaWalletTicket(this IServiceCollection services)
    {
        services
            .AddScoped<UpdateSabaWalletTicketHandler>();

        return services;
    }

    public static IEndpointRouteBuilder MapUpdateSabaWalletTicket(this IEndpointRouteBuilder endpoints)
    {
        _ = endpoints.MapPost(
            "/update-saba-wallet-ticket",
            (UpdateSabaWalletTicketCommand command,
                UpdateSabaWalletTicketHandler handler,
                CancellationToken cancellation)
                    => handler.HandleAsync(command, cancellation))
            .AddEndpointFilter<LogRequestFilter>();

        return endpoints;
    }
}