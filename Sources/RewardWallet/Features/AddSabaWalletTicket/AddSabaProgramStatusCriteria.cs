namespace RewardWallet.Features;

public class AddSabaProgramStatusCriteria : INonQueryCommand
{
    public short ProgramId { get; set; }
    public short SId { get; set; }
    public string GetSettingKey() => "NAP_SabaProgramStatus_Ins";

    public bool IsValid() => ProgramId > 0;
}
