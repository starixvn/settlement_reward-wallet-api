using System.Text.Json.Serialization;

namespace RewardWallet.Features.AddSabaWalletTicket;

public record ListCustItem(
    [property: JsonPropertyName("c")] int CustId,
    [property: JsonPropertyName("a")] decimal Amount);

public class AddSabaWalletTicketCriteria : INonQueryCommand
{
    public int SId { get; set; }
    public short ProgramId { get; set; }
    public byte WalletId { get; set; } = 2;
    public int Creator { get; set; }
    public string ListCust { get; set; }

    public string GetSettingKey() => "NAP_SabaWalletTicket_Ins";

    public bool IsValid() => !string.IsNullOrEmpty(ListCust);
}