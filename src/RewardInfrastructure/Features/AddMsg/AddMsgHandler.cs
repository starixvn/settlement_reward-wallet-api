using Fanex.Data.Repository;
using RewardInfrastructure.Constants;
using RewardWallet.Contracts;

namespace RewardInfrastructure.Features;

public class AddMsgHandler
{
    private readonly IDynamicRepository repository;

    public AddMsgHandler(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task<int?> HandleAsync(SabaWalletTicketItem sabaWalletTicketItem, string level)
    {
        var criteria = new AddMsgCriteria
        {
            ContentEng = CreateMessageContent(sabaWalletTicketItem, ContentLanguage.English, level),
            ContentCh = CreateMessageContent(sabaWalletTicketItem, ContentLanguage.Cantonese, level),
            ContentCs = CreateMessageContent(sabaWalletTicketItem, ContentLanguage.Mandarin, level),
        };

        await repository.ExecuteAsync(criteria);

        return criteria.MessageId;
    }

    private string CreateMessageContent(SabaWalletTicketItem sabaWalletTicketItem, string languageCode, string level)
    {
        var template = GetTemplate(level, languageCode);

        return template
            .Replace("@program_name", "");
    }

    private string GetTemplate(string level, string languageCode) => (level) switch
    {
        (Levels.Member) => GetTemplateForMember(languageCode),
        (Levels.Agent) => GetTemplateForSMA(languageCode),
        _ => string.Empty
    };

    private string GetTemplateForMember(string languageCode) => (languageCode) switch
    {
        ContentLanguage.English => MsgTemplateContent.MemberEng,
        ContentLanguage.Cantonese => MsgTemplateContent.MemberCantonese,
        ContentLanguage.Mandarin => MsgTemplateContent.MemberMandarin,
        //(_, ContentLanguage.Indo) => MsgContent.MemberIndo,
        _ => string.Empty
    };

    private string GetTemplateForSMA(string languageCode) => (languageCode) switch
    {
        ContentLanguage.English => MsgTemplateContent.SMAEng,
        ContentLanguage.Cantonese => MsgTemplateContent.SMACantonese,
        ContentLanguage.Mandarin => MsgTemplateContent.SMAMandarin,
        //(_, ContentLanguage.Indo) => MsgContent.SMAIndo,
        _ => string.Empty
    };
}