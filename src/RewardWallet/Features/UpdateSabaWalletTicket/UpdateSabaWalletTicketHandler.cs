using Core.Models;

namespace RewardWallet.Features;

public class UpdateSabaWalletTicketHandler
{
    private readonly IDynamicRepository repository;

    public UpdateSabaWalletTicketHandler(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Result> HandleAsync(UpdateSabaWalletTicketCommand command, CancellationToken cancellationToken)
    {
        var updateSabaWalletTicketCriteria = command.Adapt<UpdateSabaWalletTicketCriteria>();

        await repository.ExecuteAsync(updateSabaWalletTicketCriteria);

        return new Result("0", "Success");
    }
}
