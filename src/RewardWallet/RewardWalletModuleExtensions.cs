using Core.EndpointFilters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using RewardWallet.Features.AddSabaWalletTicket;
using RewardWallet.Features.CheckCustomer;
using RewardWallet.Features.FetchFeedbackTickets;
using RewardWallet.Features.FetchPendingBalance;
using RewardWallet.Features.FetchSabaWalletTickets;
using RewardWallet.Features.Testing;
using RewardWallet.Features.UpdateSabaWalletTicket;

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
            .AddFetchFeedbackTickets()
            .AddTesting()
            .AddMapster();

        return builder;
    }

    public static IEndpointRouteBuilder MapRewardWalletModule(this IEndpointRouteBuilder endpoints)
    {
        _ = endpoints
            .MapGroup($"api/{Constants.ModuleName.ToLower()}")
            .WithTags($"{Constants.ModuleName}")
            .AddCustomFluentValidationFilter()
            .AddLogRequestFilter()

            .MapFetchBalance()
            .MapFetchSabaWalletTickets()
            .MapAddSabaWalletTicket()
            .MapUpdateSabaWalletTicket()
            .MapTesting();

        return endpoints;
    }
}