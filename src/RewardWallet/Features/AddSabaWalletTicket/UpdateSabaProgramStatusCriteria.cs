namespace RewardWallet.Features;

public class UpdateSabaProgramStatusCriteria : INonQueryCommand
{
    public short SId { get; set; }
    public byte StatusId { get; set; } = 2; // completed
    public string GetSettingKey() => "NAP_SabaProgramStatus_Upd";
    public bool IsValid() => SId > 0;
}
