namespace RewardWallet.Features;

public record class FetchSabaWalletTicketsResponse(
    long TransId,
    decimal Amount,
    short ProgramId,
    DateOnly APaymentDate,
    bool IsComplaint
    );
