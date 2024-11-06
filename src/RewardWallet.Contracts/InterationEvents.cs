namespace RewardWallet.Contracts;


public record class RewardWalletUpdated(
    int CustId,
    long TransId,
    bool? IsComplaint,
    bool? BeforeIsComplaint);