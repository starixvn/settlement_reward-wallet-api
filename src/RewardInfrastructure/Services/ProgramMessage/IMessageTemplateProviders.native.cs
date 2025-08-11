using RewardWallet.Contracts;

namespace RewardInfrastructure.Services.ProgramMessage;

public interface IMessageTemplateProvider
{
    string GetTemplate(string level, string languageCode, SabaWalletTicketItem ticket);
}