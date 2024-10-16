using System.Text.Json;
using Core.Models;

namespace RewardWallet.Features;

public class AddSabaWalletTicketHandler
{
    private readonly IDynamicRepository repository;

    public AddSabaWalletTicketHandler(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Result> HandleAsync(AddSabaWalletTicketCommand command, CancellationToken cancellationToken)
    {
        var addSabaProgramStatusCriteria = command.Adapt<AddSabaProgramStatusCriteria>();
        await repository.ExecuteAsync(addSabaProgramStatusCriteria);

        var addSabaWalletTicketCriteria = new AddSabaWalletTicketCriteria
        {
            ProgramId = command.ProgramId,
            Creator = 0,
            SId = addSabaProgramStatusCriteria.SId,
            ListCust = JsonSerializer.Serialize(command.Customers.Adapt<List<ListCustItem>>()),
        };
        await repository.ExecuteAsync(addSabaWalletTicketCriteria);

        var updateSabaProgramStatusCriteria = addSabaProgramStatusCriteria.Adapt<UpdateSabaProgramStatusCriteria>();
        await repository.ExecuteAsync(updateSabaProgramStatusCriteria);

        await WriteLog(command);

        return new Result("0", "Success");
    }

    private async Task WriteLog(AddSabaWalletTicketCommand command)
    {
        var criteria = new WriteUserLogCriteria
        {
            Doing = "AddSabaWalletTicket",
            CustId = 0,
            ActionId = 0,
            SPName = "NAP_SabaWalletTicket_Ins",
            TrackingValue = JsonSerializer.Serialize(command),
        };

        await repository.ExecuteAsync(criteria);
    }
}
