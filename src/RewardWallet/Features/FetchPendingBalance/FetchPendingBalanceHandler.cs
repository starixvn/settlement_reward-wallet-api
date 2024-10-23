using RewardWallet.Features.FetchPendingBalance;

namespace RewardWallet.Features;

public class FetchPendingBalanceHandler
{
    private readonly IDynamicRepository repository;

    public FetchPendingBalanceHandler(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task<FetchPendingBalanceResponse> HandleAsync(FetchPendingBalanceQuery query, CancellationToken cancellationToken)
    {
        var criteria = query.Adapt<FetchPendingBalanceCriteria>();

        var result = await repository.GetAsync<FetchPendingBalanceResponse>(criteria);

        return result;
    }
}