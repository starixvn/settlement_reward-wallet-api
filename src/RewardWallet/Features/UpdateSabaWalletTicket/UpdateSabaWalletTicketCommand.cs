namespace RewardWallet.Features;

public record class UpdateSabaWalletTicketCommand(
    int CustId,
    long TransId,
    short SiteDbGroupId,
    bool IsComplaint);