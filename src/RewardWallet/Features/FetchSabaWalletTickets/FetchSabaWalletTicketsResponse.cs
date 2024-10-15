namespace RewardWallet.Features;

public class FetchSabaWalletTicketsResponse
{
    public long TransId { get; set; }
    public decimal Amount { get; set; }
    public short ProgramId { get; set; }
    public DateTime APaymentDate { get; set; }
    public bool IsComplaint { get; set; }
}