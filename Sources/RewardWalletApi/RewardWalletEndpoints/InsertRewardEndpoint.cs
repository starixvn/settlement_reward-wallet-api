using MinimalApi.Endpoint;
using RewardWallet.Models;
using RewardWallet.Requests;
using RewardWallet.Services;
using RewardWalletApi.EndpointFilters;

namespace RewardWalletApi.RewardWalletEndpoints;

public class InsertRewardEndpoint(RewardService rewardService) : IEndpoint<IResult, InsertRewardRequest>
{
    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet("api/promotion/reward-wallet",
                async (InsertRewardRequest request) => await HandleAsync(request))
            .Produces<Result>()
            .AddEndpointFilter<LogRequestFilter>()
            .WithTags("RewardWalletEndpoints");
    }

    public async Task<IResult> HandleAsync(InsertRewardRequest request)
    {
        var result = await rewardService.Insert(request);

        return Results.Ok(result);
    }
}