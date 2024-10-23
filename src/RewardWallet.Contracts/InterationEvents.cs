namespace RewardWallet.Contracts;

public record RewardWalletAdded;

public record RewardWalletUpdated(
    int CustId,
    long TransId,
    bool? IsComplaint,
    bool? BeforeIsComplaint);