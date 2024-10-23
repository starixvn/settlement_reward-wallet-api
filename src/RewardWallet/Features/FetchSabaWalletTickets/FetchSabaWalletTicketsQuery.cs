namespace RewardWallet.Features.FetchSabaWalletTickets;

public record FetchSabaWalletTicketsQuery(
    int CustId,
    DateTime FromDate,
    DateTime ToDate);