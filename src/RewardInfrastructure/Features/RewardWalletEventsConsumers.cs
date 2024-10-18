using MassTransit;
using Microsoft.Extensions.Logging;
using RewardWallet.Contracts;

namespace RewardInfrastructure.Features;

public class RewardWalletEventsConsumers : IConsumer<RewardWalletAdded>, IConsumer<RewardWalletUpdated>
{
    private readonly ILogger logger;

    public RewardWalletEventsConsumers(ILogger<RewardWalletEventsConsumers> logger)
    {
        this.logger = logger;
    }

    public Task Consume(ConsumeContext<RewardWalletUpdated> context)
    {
        logger.LogTrace("Consuming RewardWalletUpdated event", context.Message);
        return Task.CompletedTask;
    }

    public Task Consume(ConsumeContext<RewardWalletAdded> context)
    {
        logger.LogTrace("Consuming RewardWalletAdded event", context.Message);

        return Task.CompletedTask;
    }
}
