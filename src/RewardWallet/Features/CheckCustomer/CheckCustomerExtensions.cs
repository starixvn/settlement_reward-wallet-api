using Microsoft.Extensions.DependencyInjection;

namespace RewardWallet.Features;

public static class CheckCustomerExtensions
{
    public static IServiceCollection AddCheckCustomer(this IServiceCollection services)
    {
        services.AddScoped<CheckCustomerHandler>();

        return services;
    }
}
