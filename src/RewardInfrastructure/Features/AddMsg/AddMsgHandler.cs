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

    public async Task<int?> HandleAsync(SabaWalletTicketItem sabaWalletTicketItem, string level, CancellationToken cancellationToken)
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

    private string CreateMessageContent(SabaWalletTicketItem ticket, string languageCode, string level)
    {
        var template = GetTemplate(ticket.ProgramId, level, languageCode);

        return template
            .Replace("@program_name", "");
    }

    private string GetTemplate(int programId, string level, string languageCode) => (programId, level) switch
    {
        (_, Levels.Member) => GetTemplateForMember(1, languageCode),
        (_, Levels.Agent) => GetTemplateForSMA(1, languageCode),
        _ => string.Empty
    };

    private string GetTemplateForMember(int programId, string languageCode) => (programId, languageCode) switch
    {
        (_, ContentLanguage.English) => MsgTemplateContent.MemberEng,
        (_, ContentLanguage.Cantonese) => MsgTemplateContent.MemberCantonese,
        (_, ContentLanguage.Mandarin) => MsgTemplateContent.MemberMandarin,
        //(_, ContentLanguage.Indo) => MsgContent.MemberIndo,
        _ => string.Empty
    };

    private string GetTemplateForSMA(int programId, string languageCode) => (programId, languageCode) switch
    {
        (_, ContentLanguage.English) => MsgTemplateContent.SMAEng,
        (_, ContentLanguage.Cantonese) => MsgTemplateContent.SMACantonese,
        (_, ContentLanguage.Mandarin) => MsgTemplateContent.SMAMandarin,
        //(_, ContentLanguage.Indo) => MsgContent.SMAIndo,
        _ => string.Empty
    };
}