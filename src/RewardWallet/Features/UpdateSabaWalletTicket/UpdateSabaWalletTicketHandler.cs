using Core.Models;
using MassTransit;
using RewardWallet.Contracts;

namespace RewardWallet.Features;

public class UpdateSabaWalletTicketHandler
{
    private readonly IDynamicRepository repository;
    private readonly IBus bus;

    public UpdateSabaWalletTicketHandler(IDynamicRepository repository, IBus bus)
    {
        this.repository = repository;
        this.bus = bus;
    }

    public async Task<Result> HandleAsync(UpdateSabaWalletTicketCommand command, CancellationToken cancellationToken)
    {
        var updateSabaWalletTicketCriteria = command.Adapt<UpdateSabaWalletTicketCriteria>();

        var beforeIsComplaint = await repository.GetAsync<bool?>(updateSabaWalletTicketCriteria);

        var rewardWalletUpdated = command.Adapt<RewardWalletUpdated>() with { BeforeIsComplaint = beforeIsComplaint };

        await bus.Publish(rewardWalletUpdated);

        return new Result("0", "Success");
    }
}