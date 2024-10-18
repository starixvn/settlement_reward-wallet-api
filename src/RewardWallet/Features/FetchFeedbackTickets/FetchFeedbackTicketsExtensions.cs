using Microsoft.Extensions.DependencyInjection;
using RewardWallet.Contracts;

namespace RewardWallet.Features;

public static class FetchFeedbackTicketsExtensions
{
    public static IServiceCollection AddFetchFeedbackTickets(this IServiceCollection services)
    {
        services
            .AddScoped<ISabaWalletTicketService, SabaWalletTicketService>();

        return services;
    }
}
