using Core.Models;
using System.Text.Json;

namespace RewardWallet.Features.AddSabaWalletTicket;

public class AddSabaWalletTicketHandler(IDynamicRepository repository)
{
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
            SPName = "NAP_SabaWalletTicket_Ins",
            TrackingValue = JsonSerializer.Serialize(command),
        };

        await repository.ExecuteAsync(criteria);
    }
}