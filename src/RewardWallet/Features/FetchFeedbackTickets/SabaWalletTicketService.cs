using RewardWallet.Contracts;

namespace RewardWallet.Features;

public class SabaWalletTicketService : ISabaWalletTicketService
{
    private readonly IDynamicRepository repository;

    public SabaWalletTicketService(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task<List<SabaWalletTicketItem>> GetFeedback(int cusId, long transId)
    {
        var criteria = new FetchFeedbackTicketsCriteria
        {
            CustId = cusId,
            TransId = transId
        };

        var results = await repository.FetchAsync<SabaWalletTicketItem>(criteria);

        return results.ToList();
    }
}
