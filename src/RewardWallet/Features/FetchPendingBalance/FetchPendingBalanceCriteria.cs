namespace RewardWallet.Features.FetchPendingBalance;

public class FetchPendingBalanceCriteria : CriteriaBase
{
    public int CustId { get; set; }
    public int WalletId { get; set; } = 2;

    public override string GetSettingKey() => "NAP_SabaProgram_PendingBalance_Sel";

    public override bool IsValid() => CustId > 0;
}