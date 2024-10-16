using Core.EndpointFilters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using RewardWallet.Features;

namespace RewardWallet;

public static class RewardWalletModuleExtensions
{
    public static WebApplicationBuilder AddRewardWalletModule(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddFetchPendingBalance()
            .AddFetchSabaWalletTickets()
            .AddAddSabaWalletTicket()
            .AddCheckCustomer()
            .AddUpdateSabaWalletTicket()
            .AddMapster();

        return builder;
    }

    public static IEndpointRouteBuilder MapRewardWalletModule(this IEndpointRouteBuilder endpoints)
    {
        _ = endpoints
            .MapGroup($"api/{Constants.ModuleName.ToLower()}")
            .WithTags($"{Constants.ModuleName}")
            .AddCustomFluentValidationFilter()

            .MapFetchBalance()
            .MapFetchSabaWalletTickets()
            .MapAddSabaWalletTicket()
            .MapUpdateSabaWalletTicket();

        return endpoints;
    }
}
