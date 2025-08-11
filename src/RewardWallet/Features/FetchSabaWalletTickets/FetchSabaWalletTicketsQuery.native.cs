namespace RewardWallet.Features.FetchSabaWalletTickets;

public record class FetchSabaWalletTicketsQuery(
    int CustId,
    DateTime FromDate,
    DateTime ToDate);