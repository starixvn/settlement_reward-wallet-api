using RewardWallet.Contracts;

namespace RewardWallet.Features.FetchFeedbackTickets;

public class SabaWalletTicketService(IDynamicRepository repository) : ISabaWalletTicketService
{
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