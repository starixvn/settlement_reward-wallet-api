namespace RewardWallet.Features;

public class CheckCustomerCriteria : CriteriaBase
{
    public string CustIds { get; set; }
    public short SiteDbGroupId { get; set; }
    public override string GetSettingKey() => "NAP_SabaWalletTicket_Validation";

    public override bool IsValid() => !string.IsNullOrEmpty(CustIds);
}
