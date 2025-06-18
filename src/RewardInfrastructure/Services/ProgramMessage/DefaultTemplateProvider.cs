using RewardInfrastructure.Constants;
using RewardWallet.Contracts;

namespace RewardInfrastructure.Services.ProgramMessage;

public class DefaultTemplateProvider() : IMessageTemplateProvider
{
    public string GetTemplate(string level, string languageCode, SabaWalletTicketItem ticket)
    {
        var template = level switch
        {
            Levels.Agent => GetTemplateForAgent(languageCode),
            Levels.Super => GetTemplateForSuperMaster(languageCode),
            _ => string.Empty
        };

        return template
            .Replace("@member_username", ticket.Username)
            .Replace("@amount", $"{ticket.Amount:#,0.##}");
    }

    private static string GetTemplateForAgent(string languageCode)
    {
        return languageCode switch
        {
            ContentLanguage.English => MessageContent.MemberEng,
            ContentLanguage.Cantonese => MessageContent.MemberCantonese,
            ContentLanguage.Mandarin => MessageContent.MemberMandarin,
            _ => string.Empty
        };
    }

    private static string GetTemplateForSuperMaster(string languageCode)
    {
        return languageCode switch
        {
            ContentLanguage.English => MessageContent.SMAEng,
            ContentLanguage.Cantonese => MessageContent.SMACantonese,
            ContentLanguage.Mandarin => MessageContent.SMAMandarin,
            _ => string.Empty
        };
    }
}