namespace RewardWallet.Features.UpdateSabaWalletTicket;

public record UpdateSabaWalletTicketCommand(
    int CustId,
    long TransId,
    short SiteDbGroupId,
    bool IsComplaint);