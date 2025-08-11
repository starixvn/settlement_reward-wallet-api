using Core.Models;
using MassTransit;
using RewardWallet.Contracts;

namespace RewardWallet.Features.UpdateSabaWalletTicket;

public class UpdateSabaWalletTicketHandler(IDynamicRepository repository, IBus bus)
{
    public async Task<Result> HandleAsync(UpdateSabaWalletTicketCommand command, CancellationToken cancellationToken)
    {
        var updateSabaWalletTicketCriteria = command.Adapt<UpdateSabaWalletTicketCriteria>();

        var beforeIsComplaint = await repository.GetAsync<bool?>(updateSabaWalletTicketCriteria, cancellationToken: cancellationToken);

        var rewardWalletUpdated = command.Adapt<RewardWalletUpdated>() with { BeforeIsComplaint = beforeIsComplaint };

        await bus.Publish(rewardWalletUpdated, cancellationToken);

        return new Result("0", "Success");
    }
}