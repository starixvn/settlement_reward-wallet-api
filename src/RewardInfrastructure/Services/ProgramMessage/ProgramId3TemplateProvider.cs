using Fanex.RemotingRef;
using RewardInfrastructure.Constants;
using RewardWallet.Contracts;

namespace RewardInfrastructure.Services.ProgramMessage;

public class ProgramId3TemplateProvider(IRemotingRefService remotingRefService) : IMessageTemplateProvider
{
    public string GetTemplate(string level, string languageCode, SabaWalletTicketItem ticket)
    {
        var programName = GetProgramName(ticket.ProgramId, languageCode);
        var template = level switch
        {
            Levels.Agent => GetTemplateForAgent(languageCode),
            Levels.Super => GetTemplateForSuperMaster(languageCode),
            _ => string.Empty
        };

        return template
            .Replace("@program_name", programName)
            .Replace("@member_username", ticket.Username)
            .Replace("@amount", $"{ticket.Amount:#,0.##}");
    }

    private string GetProgramName(int programId, string languageCode)
    {
        return remotingRefService.GetResourceDataByRefIdAndResourceId("RewardProgram", programId.ToString(), languageCode, "ProgramName");
    }

    private static string GetTemplateForAgent(string languageCode)
    {
        return languageCode switch
        {
            ContentLanguage.English => SixScoresContent.MemberEng,
            ContentLanguage.Cantonese => SixScoresContent.MemberCantonese,
            ContentLanguage.Mandarin => SixScoresContent.MemberMandarin,
            ContentLanguage.Thai => SixScoresContent.MemberThai,
            _ => string.Empty
        };
    }

    private static string GetTemplateForSuperMaster(string languageCode)
    {
        return languageCode switch
        {
            ContentLanguage.English => SixScoresContent.SMAEng,
            ContentLanguage.Cantonese => SixScoresContent.SMACantonese,
            ContentLanguage.Mandarin => SixScoresContent.SMAMandarin,
            ContentLanguage.Thai => SixScoresContent.SMAThai,
            _ => string.Empty
        };
    }
}