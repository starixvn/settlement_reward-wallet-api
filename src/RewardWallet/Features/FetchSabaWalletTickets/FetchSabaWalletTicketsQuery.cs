namespace RewardWallet.Features;

public record class FetchSabaWalletTicketsQuery(
    int CustId,
    DateTime FromDate,
    DateTime ToDate);