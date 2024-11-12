using System.Collections;

namespace RewardWallet.Features.FetchSabaWalletTickets;

public class FetchSabaWalletTicketsItemResponse
{
    public string TransId { get; set; }
    public decimal Amount { get; set; }
    public short ProgramId { get; set; }
    public DateTime APaymentDate { get; set; }
    public bool? IsComplaint { get; set; }
}

public record class FetchSabaWalletTicketsResponse(IEnumerable<FetchSabaWalletTicketsItemResponse> Items) : IEnumerable<FetchSabaWalletTicketsItemResponse>
{
    public IEnumerator<FetchSabaWalletTicketsItemResponse> GetEnumerator()
        => Items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable)Items).GetEnumerator();
}