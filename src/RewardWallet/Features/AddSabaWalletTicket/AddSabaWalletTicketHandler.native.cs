using Core.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace RewardWallet.Features.AddSabaWalletTicket;

public class AddSabaWalletTicketHandler(IDynamicRepository repository,IOptions<AppSettings> options)
{
    public async Task<Result> HandleAsync(AddSabaWalletTicketCommand command, CancellationToken cancellationToken)
    {
        var addSabaProgramStatusCriteria = command.Adapt<AddSabaProgramStatusCriteria>();
        addSabaProgramStatusCriteria.IsTest = options.Value.EnvironmentName == "UAT" || command.IsTest;

        await repository.ExecuteAsync(addSabaProgramStatusCriteria, cancellationToken: cancellationToken);

        var addSabaWalletTicketCriteria = new AddSabaWalletTicketCriteria
        {
            ProgramId = command.ProgramId,
            Creator = 0,
            SId = addSabaProgramStatusCriteria.SId,
            ListCust = JsonSerializer.Serialize(command.Customers.Adapt<List<ListCustItem>>())
        };
        await repository.ExecuteAsync(addSabaWalletTicketCriteria, cancellationToken: cancellationToken);

        var updateSabaProgramStatusCriteria = addSabaProgramStatusCriteria.Adapt<UpdateSabaProgramStatusCriteria>();
        await repository.ExecuteAsync(updateSabaProgramStatusCriteria, cancellationToken: cancellationToken);

        await WriteLog(command);

        return new Result("0", "Success");
    }

    private async Task WriteLog(AddSabaWalletTicketCommand command)
    {
        var criteria = new WriteUserLogCriteria
        {
            Doing = "AddSabaWalletTicket",
            SpName = "NAP_SabaWalletTicket_Ins",
            TrackingValue = JsonSerializer.Serialize(command)
        };

        await repository.ExecuteAsync(criteria);
    }
}