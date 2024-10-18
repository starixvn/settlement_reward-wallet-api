namespace RewardWallet.Contracts;

public interface ISabaWalletTicketService
{
    Task<List<SabaWalletTicketItem>> GetFeedback(int cusId, long transId);
}
