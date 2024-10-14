using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace RewardWallet.Features;

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
            "/saba-wallet-ticket",
            (AddSabaWalletTicketCommand command,
                AddSabaWalletTicketHandler handler,
                CancellationToken cancellation)
                    => handler.HandleAsync(command, cancellation));

        return endpoints;
    }
}
