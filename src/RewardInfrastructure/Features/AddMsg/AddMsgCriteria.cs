using Fanex.Data;
using Fanex.Data.Repository;

namespace RewardInfrastructure.Features.AddMsg;

public class AddMsgCriteria : NonQueryCommand
{
    public int? MessageId { get; set; }

    [SpParam(SqlParamName = "Content_en")]
    public string ContentEng { get; set; }

    [SpParam(SqlParamName = "Content_th")]
    public string ContentTh { get; set; }

    [SpParam(SqlParamName = "Content_ch")]
    public string ContentCh { get; set; }

    [SpParam(SqlParamName = "Content_cs")]
    public string ContentCs { get; set; }

    [SpParam(SqlParamName = "Content_id")]
    public string ContentId { get; set; }

    public int? MatchId { get; set; }

    public short MsgType { get; set; } = 1;

    public int MsgContentsType { get; set; }

    public int Creator { get; set; } = Constants.Creator.Id;

    public string CreatorName { get; set; } = Constants.Creator.Name;

    public override string GetSettingKey() => "NAP_SpecialDelete_Message_Insert";

    public override bool IsValid() => !string.IsNullOrEmpty(ContentEng);
}