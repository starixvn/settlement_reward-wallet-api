namespace RewardWallet.Features;

public class UpdateSabaWalletTicketCriteria : INonQueryCommand
{
    public int CustId { get; set; }
    public string TransIds { get; set; }
    public bool IsComplaint { get; set; }
    public string GetSettingKey() => "NAP_SabaWalletTicket_Complaint_Udp";

    public bool IsValid() => CustId > 0;
}
