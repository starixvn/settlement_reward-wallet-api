using System.Collections;
using RewardWallet.Contracts;

namespace RewardWallet.Features;

public record class FetchSabaWalletTicketsResponse(IEnumerable<SabaWalletTicketItem> Items) : IEnumerable<SabaWalletTicketItem>
{
    public IEnumerator<SabaWalletTicketItem> GetEnumerator()
        => Items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable)Items).GetEnumerator();
}