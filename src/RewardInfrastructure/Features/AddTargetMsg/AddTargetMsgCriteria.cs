using Fanex.Data.Repository;

namespace RewardInfrastructure.Features;

internal class AddTargetMsgCriteria : NonQueryCommand
{
    public string XMLMessage { get; set; } = string.Empty;

    public int CreatorId { get; set; }

    public int ErrorCode { get; set; }

    public string ErrorMsg { get; set; } = string.Empty;

    public override string GetSettingKey() => "NAP_SpecialDelete_BatchMessage_Upload";

    public override bool IsValid() => !string.IsNullOrEmpty(XMLMessage);
}
