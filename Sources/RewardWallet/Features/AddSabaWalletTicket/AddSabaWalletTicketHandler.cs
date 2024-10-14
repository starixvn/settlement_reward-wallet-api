namespace RewardWallet.Features;

public class AddSabaWalletTicketHandler
{
    private readonly IDynamicRepository repository;

    public AddSabaWalletTicketHandler(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task HandleAsync(AddSabaWalletTicketCommand command, CancellationToken cancellationToken)
    {
        var addSabaProgramStatusCriteria = command.Adapt<AddSabaProgramStatusCriteria>();
        await repository.ExecuteAsync(addSabaProgramStatusCriteria);

        await repository.ExecuteAsync(new AddSabaWalletTicketCriteria
        {
            ProgramId = command.ProgramId,
            Creator = command.AdminId,
            SId = addSabaProgramStatusCriteria.SId,
            //ListCust = JsonSerializer.,
        });

        await repository.ExecuteAsync(new UpdateSabaProgramStatusCriteria
        {
            SId = addSabaProgramStatusCriteria.SId,
            StatusId = 2,

        });
    }
}
