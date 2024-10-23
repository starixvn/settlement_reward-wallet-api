namespace RewardWallet.Features.FetchSabaWalletTickets;

public class FetchSabaWalletTicketsCriteria : CriteriaBase
{
    public int CustId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int StatusId { get; set; } = 1;
    public int WalletId { get; set; } = 2;

    public override string GetSettingKey() => "NAP_SabaWalletTicket_Sel";

    public override bool IsValid() => CustId > 0;
}