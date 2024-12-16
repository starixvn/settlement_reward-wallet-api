namespace RewardWallet.Features.FetchPendingBalance;

public class FetchPendingBalanceHandler(IDynamicRepository repository)
{
    public async Task<FetchPendingBalanceResponse> HandleAsync(FetchPendingBalanceQuery query, CancellationToken cancellationToken)
    {
        var criteria = query.Adapt<FetchPendingBalanceCriteria>();
        var result = await repository.GetAsync<FetchPendingBalanceResponse>(criteria, cancellationToken: cancellationToken);

        return result ?? new FetchPendingBalanceResponse();
    }
}