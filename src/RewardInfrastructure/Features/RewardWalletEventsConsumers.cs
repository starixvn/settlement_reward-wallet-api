using MassTransit;
using RewardInfrastructure.Constants;
using RewardInfrastructure.Features.AddMsg;
using RewardInfrastructure.Features.AddTargetMsg;
using RewardWallet.Contracts;

namespace RewardInfrastructure.Features;

public class RewardWalletEventsConsumers(
    ISabaWalletTicketService sabaWalletTicketService,
    AddMsgHandler addMsgHandler,
    AddTargetMsgHandler addTargetMsgHandler)
    : IConsumer<RewardWalletAdded>, IConsumer<RewardWalletUpdated>
{
    public async Task Consume(ConsumeContext<RewardWalletUpdated> context)
    {
        if (context.Message is { IsComplaint: true, BeforeIsComplaint: null })
        {
            var tickets = await sabaWalletTicketService.GetFeedback(context.Message.CustId, context.Message.TransId);

            await HandleCreateMsg(tickets[0], Levels.Agent);
            await HandleCreateMsg(tickets[0], Levels.Master);
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