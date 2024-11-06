using Core.EndpointFilters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace RewardWallet.Features.AddSabaWalletTicket;

public static class AddSabaWalletTicketExtensions
{
    public static IServiceCollection AddAddSabaWalletTicket(this IServiceCollection services)
    {
        services
            .AddScoped<AddSabaWalletTicketHandler>();

        return services;
    }

    public static IEndpointRouteBuilder MapAddSabaWalletTicket(this IEndpointRouteBuilder endpoints)
    {
        _ = endpoints.MapPost(
            "/insert-wallet",
            (AddSabaWalletTicketCommand command,
                AddSabaWalletTicketHandler handler,
                CancellationToken cancellation)
                    => handler.HandleAsync(command, cancellation))
            .AddEndpointFilter<LogRequestFilter>();

        return endpoints;
    }
}