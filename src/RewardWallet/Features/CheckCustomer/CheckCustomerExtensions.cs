using Microsoft.Extensions.DependencyInjection;

namespace RewardWallet.Features.CheckCustomer;

public static class CheckCustomerExtensions
{
    public static IServiceCollection AddCheckCustomer(this IServiceCollection services)
    {
        services.AddScoped<CheckCustomerHandler>();

        return services;
    }
}