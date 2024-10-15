namespace RewardWallet.Features;

public record class UpdateSabaWalletTicketCommand(
    int CustId,
    string TransIds,
    short SiteDbGroupId,
    bool IsComplaint);