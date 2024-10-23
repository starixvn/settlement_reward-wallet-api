namespace RewardWallet.Features.AddSabaWalletTicket;

public class AddSabaProgramStatusCriteria : INonQueryCommand
{
    public short ProgramId { get; set; }
    public int SId { get; set; }

    public string GetSettingKey() => "NAP_SabaProgramStatus_Ins";

    public bool IsValid() => ProgramId > 0;
}