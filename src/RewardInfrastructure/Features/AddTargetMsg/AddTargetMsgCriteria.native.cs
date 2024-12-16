using Fanex.Data.Repository;
using RewardInfrastructure.Constants;

namespace RewardInfrastructure.Features.AddTargetMsg;

internal class AddTargetMsgCriteria : NonQueryCommand
{
    public string XMLMessage { get; set; }

    public int CreatorId { get; set; } = Creator.Id;

    public int ErrorCode { get; set; }

    public string ErrorMsg { get; set; }

    public override string GetSettingKey() => "NAP_SpecialDelete_BatchMessage_Upload";

    public override bool IsValid() => !string.IsNullOrEmpty(XMLMessage);
}