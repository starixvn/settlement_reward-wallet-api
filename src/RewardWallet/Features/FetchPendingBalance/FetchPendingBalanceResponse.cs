namespace RewardWallet.Features.FetchPendingBalance;

public class FetchPendingBalanceResponse
{
    public decimal Amount { get; set; }
    public string CurrencyCode { get; set; }
};