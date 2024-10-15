using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace RewardWallet.Features;

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
                    => handler.HandleAsync(command, cancellation));

        return endpoints;
    }
}
