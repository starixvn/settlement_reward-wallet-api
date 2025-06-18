using Fanex.Data.Repository;
using RewardInfrastructure.Constants;
using RewardInfrastructure.Criteria;
using RewardWallet.Contracts;

namespace RewardInfrastructure.Services;

public interface InsertMessageService
{
    Task<int> CreateMessageAsync(SabaWalletTicketItem sabaWalletTicketItem, string level);
}

public class InsertMessageServiceImp(
    IDynamicRepository repository,
    MessageTemplateProviderFactory templateProviderFactory) : InsertMessageService
{
    public async Task<int> CreateMessageAsync(SabaWalletTicketItem sabaWalletTicketItem, string level)
    {
        var criteria = new InsertMessageCriteria
        {
            ContentEng = CreateMessageContent(sabaWalletTicketItem, ContentLanguage.English, level),
            ContentCh = CreateMessageContent(sabaWalletTicketItem, ContentLanguage.Cantonese, level),
            ContentCs = CreateMessageContent(sabaWalletTicketItem, ContentLanguage.Mandarin, level),
            ContentThai = CreateMessageContent(sabaWalletTicketItem, ContentLanguage.Thai, level)
        };

        return await repository.GetAsync<int>(criteria);
    }

    private string CreateMessageContent(SabaWalletTicketItem sabaWalletTicketItem, string languageCode, string level)
    {
        var provider = templateProviderFactory.GetProvider(sabaWalletTicketItem.ProgramId);
        return provider.GetTemplate(level, languageCode, sabaWalletTicketItem);
    }
}