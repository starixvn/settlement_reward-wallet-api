namespace RewardWallet.Features.AddSabaWalletTicket;

public class UpdateSabaProgramStatusCriteria : INonQueryCommand
{
    public int SId { get; set; }
    public byte StatusId { get; set; } = 2; // completed

    public string GetSettingKey() => "NAP_SabaProgramStatus_Upd";

    public bool IsValid() => SId > 0;
}