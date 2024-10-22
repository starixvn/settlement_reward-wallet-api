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

    public async Task<int> HandleAsync(SabaWalletTicketItem sabaWalletTicketItem, string level)
    {
        var criteria = new AddMsgCriteria
        {
            ContentEng = CreateMessageContent(sabaWalletTicketItem, ContentLanguage.English, level),
            ContentCh = CreateMessageContent(sabaWalletTicketItem, ContentLanguage.Cantonese, level),
            ContentCs = CreateMessageContent(sabaWalletTicketItem, ContentLanguage.Mandarin, level),
        };

        return await repository.GetAsync<int>(criteria);

    }

    private string CreateMessageContent(SabaWalletTicketItem sabaWalletTicketItem, string languageCode, string level)
    {
        var template = GetTemplate(level, languageCode);

        return template
            .Replace("@member_username", sabaWalletTicketItem.Username)
            .Replace("@amount", $"{sabaWalletTicketItem.Amount}");
    }

    private string GetTemplate(string level, string languageCode) => (level) switch
    {
        (Levels.Agent) => GetTemplateForAgent(languageCode),
        (Levels.Super) => GetTemplateForSuperMaster(languageCode),
        _ => string.Empty
    };

    private string GetTemplateForAgent(string languageCode) => (languageCode) switch
    {
        ContentLanguage.English => MsgTemplateContent.MemberEng,
        ContentLanguage.Cantonese => MsgTemplateContent.MemberCantonese,
        ContentLanguage.Mandarin => MsgTemplateContent.MemberMandarin,
        //(_, ContentLanguage.Indo) => MsgContent.MemberIndo,
        _ => string.Empty
    };

    private string GetTemplateForSuperMaster(string languageCode) => (languageCode) switch
    {
        ContentLanguage.English => MsgTemplateContent.SMAEng,
        ContentLanguage.Cantonese => MsgTemplateContent.SMACantonese,
        ContentLanguage.Mandarin => MsgTemplateContent.SMAMandarin,
        //(_, ContentLanguage.Indo) => MsgContent.SMAIndo,
        _ => string.Empty
    };
}