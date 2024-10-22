using MassTransit;
using Microsoft.Extensions.Logging;
using RewardInfrastructure.Constants;
using RewardWallet.Contracts;

namespace RewardInfrastructure.Features;

public class RewardWalletEventsConsumers : IConsumer<RewardWalletAdded>, IConsumer<RewardWalletUpdated>
{
    private readonly ILogger logger;
    private readonly ISabaWalletTicketService sabaWalletTicketService;
    private readonly AddMsgHandler addMsgHandler;
    private readonly AddTargetMsgHandler addTargetMsgHandler;

    public RewardWalletEventsConsumers(
        ILogger<RewardWalletEventsConsumers> logger,
        ISabaWalletTicketService sabaWalletTicketService,
        AddMsgHandler addMsgHandler,
        AddTargetMsgHandler addTargetMsgHandler)
    {
        this.logger = logger;
        this.sabaWalletTicketService = sabaWalletTicketService;
        this.addMsgHandler = addMsgHandler;
        this.addTargetMsgHandler = addTargetMsgHandler;
    }

    public async Task Consume(ConsumeContext<RewardWalletUpdated> context)
    {
        logger.LogTrace("Consuming RewardWalletUpdated event", context.Message);
        if (context.Message is not null
            && context.Message.IsComplaint == true
            && context.Message.BeforeIsComplaint is null)
        {
            var tickets = await sabaWalletTicketService.GetFeedback(context.Message.CustId, context.Message.TransId);

            await HandleCreateMsg(tickets.First(), Levels.Agent);
            await HandleCreateMsg(tickets.First(), Levels.Master);

            logger.LogTrace("Consum successfulle", tickets.First());
        }

    }

    public Task Consume(ConsumeContext<RewardWalletAdded> context)
    {
        return Task.CompletedTask;
    }

    private async Task HandleCreateMsg(SabaWalletTicketItem item, string level)
    {
        var messageId = await addMsgHandler.HandleAsync(item, Levels.Agent);
        await addTargetMsgHandler.HandleAsync(item, messageId, level);
    }
}
