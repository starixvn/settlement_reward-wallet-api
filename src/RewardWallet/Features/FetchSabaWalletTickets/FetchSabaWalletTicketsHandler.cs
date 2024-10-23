namespace RewardWallet.Features.FetchSabaWalletTickets;

public class FetchSabaWalletTicketsHandler(IDynamicRepository repository)
{
    public async Task<FetchSabaWalletTicketsResponse> HandleAsync(FetchSabaWalletTicketsQuery query, CancellationToken cancellationToken)
    {
        var criteria = query.Adapt<FetchSabaWalletTicketsCriteria>();

        var results = await repository.FetchAsync<FetchSabaWalletTicketsItemResponse>(criteria);

        return new FetchSabaWalletTicketsResponse(results);
    }
}