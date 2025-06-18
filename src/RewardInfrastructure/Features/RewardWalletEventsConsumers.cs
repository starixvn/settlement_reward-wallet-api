using MassTransit;
using RewardInfrastructure.Constants;
using RewardInfrastructure.Services;
using RewardWallet.Contracts;

namespace RewardInfrastructure.Features;

public class RewardWalletEventsConsumers(
    ISabaWalletTicketService sabaWalletTicketService,
    InsertMessageService insertMessageService,
    UploadBatchMessageService uploadBatchMessageServiceImp)
    : IConsumer<RewardWalletUpdated>
{
    public async Task Consume(ConsumeContext<RewardWalletUpdated> context)
    {
        if (context.Message is { IsComplaint: true, BeforeIsComplaint: null })
        {
            var tickets = await sabaWalletTicketService.GetFeedback(context.Message.CustId, context.Message.TransId);

            await HandleCreateMessage(tickets[0], Levels.Agent);
            await HandleCreateMessage(tickets[0], Levels.Super);
        }
    }

    private async Task HandleCreateMessage(SabaWalletTicketItem item, string level)
    {
        var messageId = await insertMessageService.CreateMessageAsync(item, level);
        await uploadBatchMessageServiceImp.HandleAsync(item, messageId, level);
    }
}