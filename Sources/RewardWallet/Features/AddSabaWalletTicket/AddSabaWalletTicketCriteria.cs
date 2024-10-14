namespace RewardWallet.Features;

public class AddSabaWalletTicketCriteria : INonQueryCommand
{
    public short SId { get; set; }
    public short ProgramId { get; set; }
    public byte WalletId { get; set; }
    public int Creator { get; set; }
    public string ListCust { get; set; }
    public string GetSettingKey() => "NAP_SabaWalletTicket_Ins";

    public bool IsValid() => !string.IsNullOrEmpty(ListCust);
}
