namespace RewardWallet.Features.UpdateSabaWalletTicket;

public class UpdateSabaWalletTicketCriteria : CriteriaBase
{
    public int CustId { get; set; }
    public long TransId { get; set; }
    public bool? IsComplaint { get; set; }

    public override string GetSettingKey() => "NAP_SabaWalletTicket_Complaint_Udp";

    public override bool IsValid() => CustId > 0;
}