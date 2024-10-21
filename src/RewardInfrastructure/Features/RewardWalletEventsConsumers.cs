using MassTransit;
using Microsoft.Extensions.Logging;
using RewardWallet.Contracts;

namespace RewardInfrastructure.Features;

public class RewardWalletEventsConsumers : IConsumer<RewardWalletAdded>, IConsumer<RewardWalletUpdated>
{
    private readonly ILogger logger;
    private readonly ISabaWalletTicketService sabaWalletTicketService;
    private readonly AddMsgHandler addMsgHandler;
    private readonly AddTargetMsgHandler addTargetMsgHandler;

    public RewardWalletEventsConsumers(
        ILogger<RewardWalletEventsConsumers> logger //,
                                                    //ISabaWalletTicketService sabaWalletTicketService,
                                                    //AddMsgHandler addMsgHandler,
                                                    //AddTargetMsgHandler addTargetMsgHandler
        )
    {
        this.logger = logger;
        //this.sabaWalletTicketService = sabaWalletTicketService;
        //this.addMsgHandler = addMsgHandler;
        //this.addTargetMsgHandler = addTargetMsgHandler;
    }

    public async Task Consume(ConsumeContext<RewardWalletUpdated> context)
    {
        logger.LogTrace("Consuming RewardWalletUpdated event", context.Message);
        //var tickets = await sabaWalletTicketService.GetFeedback(context.Message.CustId, context.Message.TransId);
        //var messageId = await addMsgHandler.HandleAsync(tickets.FirstOrDefault()!, Levels.Member);
        //_ = await addTargetMsgHandler.HandleAsync(tickets.FirstOrDefault()!, messageId ?? 0);
        //logger.LogTrace("Consum succees", context.Message);
    }

    public Task Consume(ConsumeContext<RewardWalletAdded> context)
    {
        logger.LogTrace("Consuming RewardWalletAdded event", context.Message);

        return Task.CompletedTask;
    }

    private bool IsExistMember(IEnumerable<SabaWalletTicketItem> tickets)
    => tickets.Any(x => x.CustId != 0);

    private bool IsExistSMA(IEnumerable<SabaWalletTicketItem> tickets)
        => tickets.Any(x => x.AgentId != 0 || x.MasterId != 0 || x.SuperId != 0);

}
