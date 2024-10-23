namespace RewardWallet.Features.AddSabaWalletTicket;

public class WriteUserLogCriteria : INonQueryCommand
{
    public int UserId { get; set; }
    public int CustId { get; set; }
    public string SPName { get; set; }
    public string Doing { get; set; }
    public int ActionId { get; set; } = 95123; // for only project
    public string TrackingValue { get; set; }

    public string GetSettingKey() => "WriteLog_UserLog_RunSP";

    public bool IsValid() => true;
}