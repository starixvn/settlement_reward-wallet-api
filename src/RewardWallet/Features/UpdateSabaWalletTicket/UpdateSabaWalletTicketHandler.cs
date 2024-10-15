using FluentValidation;
using RewardWallet.Models;

namespace RewardWallet.Features;

public class UpdateSabaWalletTicketHandler
{
    private readonly IValidator<UpdateSabaWalletTicketCommand> validator;
    private readonly IDynamicRepository repository;

    public UpdateSabaWalletTicketHandler(IValidator<UpdateSabaWalletTicketCommand> validator, IDynamicRepository repository)
    {
        this.validator = validator;
        this.repository = repository;
    }

    public async Task<Result> HandleAsync(UpdateSabaWalletTicketCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await HandleInValidCommand(command, cancellationToken);

        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        var updateSabaWalletTicketCriteria = command.Adapt<UpdateSabaWalletTicketCriteria>();

        await repository.ExecuteAsync(updateSabaWalletTicketCriteria);

        return validationResult;
    }


    private async Task<Result> HandleInValidCommand(UpdateSabaWalletTicketCommand command, CancellationToken cancellationToken)
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
