using Fanex.RemotingRef;
using RewardInfrastructure.Constants;
using RewardInfrastructure.Services.ProgramMessage;

namespace RewardInfrastructure.Services;

public interface MessageTemplateProviderFactory
{
    IMessageTemplateProvider GetProvider(int programId);
}

public class MessageTemplateProviderFactoryImp(IRemotingRefService remotingRefService) : MessageTemplateProviderFactory
{
    public IMessageTemplateProvider GetProvider(int programId)
    {
        var templateProviders = new Dictionary<int, IMessageTemplateProvider>
        {
            { ProgramId.SixScoreChallenge, new ProgramId3TemplateProvider(remotingRefService) }
        };

        return templateProviders.TryGetValue(programId, out var provider) ? provider : new DefaultTemplateProvider();
    }
}