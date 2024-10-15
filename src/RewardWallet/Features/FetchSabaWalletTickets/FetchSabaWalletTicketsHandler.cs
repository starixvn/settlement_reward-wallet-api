namespace RewardWallet.Features;

public class FetchSabaWalletTicketsHandler
{
    private readonly IDynamicRepository repository;

    public FetchSabaWalletTicketsHandler(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task<List<FetchSabaWalletTicketsResponse>> HandleAsync(FetchSabaWalletTicketsQuery query, CancellationToken cancellationToken)
    {
        var criteria = query.Adapt<FetchSabaWalletTicketsCriteria>();

        var results = await repository.FetchAsync<FetchSabaWalletTicketsResponse>(criteria, cancellationToken);

        return results.ToList();

    }

}
