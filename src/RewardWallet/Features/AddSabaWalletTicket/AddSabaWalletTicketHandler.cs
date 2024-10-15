using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using RewardWallet.Models;

namespace RewardWallet.Features;

public class AddSabaWalletTicketHandler
{
    private readonly IDynamicRepository repository;
    private readonly IValidator<AddSabaWalletTicketCommand> validator;

    public AddSabaWalletTicketHandler(IDynamicRepository repository, IValidator<AddSabaWalletTicketCommand> validator)
    {
        this.repository = repository;
        this.validator = validator;
    }

    public async Task<Results<Ok<Result>, BadRequest<Result>>> HandleAsync(AddSabaWalletTicketCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await HandleInValidCommand(command, cancellationToken);

        if (!string.IsNullOrEmpty(validationResult.ErrorCode))
        {
            return TypedResults.BadRequest(validationResult);
        }

        var addSabaProgramStatusCriteria = command.Adapt<AddSabaProgramStatusCriteria>();
        await repository.ExecuteAsync(addSabaProgramStatusCriteria);

        var addSabaWalletTicketCriteria = new AddSabaWalletTicketCriteria
        {
            ProgramId = command.ProgramId,
            Creator = 8,
            SId = addSabaProgramStatusCriteria.SId,
            ListCust = JsonSerializer.Serialize(command.Customers.Adapt<List<ListCustItem>>()),
        };
        await repository.ExecuteAsync(addSabaWalletTicketCriteria);

        var updateSabaProgramStatusCriteria = addSabaProgramStatusCriteria.Adapt<UpdateSabaProgramStatusCriteria>();
        await repository.ExecuteAsync(updateSabaProgramStatusCriteria);

        return TypedResults.Ok(validationResult);
    }


    private async Task<Result> HandleInValidCommand(AddSabaWalletTicketCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (validationResult.Errors.Any())
        {
            var firstError = validationResult.Errors[0];
            return new Result(firstError.ErrorCode, firstError.ErrorMessage);
        }

        return new Result("0", "Success");
    }
}
