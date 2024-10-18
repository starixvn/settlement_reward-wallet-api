using FluentValidation;

namespace RewardWallet.Features;

public class UpdateSabaWalletTicketValidator : AbstractValidator<UpdateSabaWalletTicketCommand>
{
    public UpdateSabaWalletTicketValidator(CheckCustomerHandler checkCustomerHandler)
    {
        RuleFor(x => x.CustId)
            .GreaterThan(0);

        RuleFor(x => new { x.CustId, x.SiteDbGroupId })
            .Must((x, _, context) =>
            {
                var isValid = checkCustomerHandler.IsCustIdsExists(x.CustId.ToString(), x.SiteDbGroupId, out var invalidCustIds);
                context.MessageFormatter.AppendArgument("InvalidCustIds", string.Join(", ", invalidCustIds));
                return isValid;
            })
            .WithMessage("Customer IDs do not exist: {InvalidCustIds}");

        RuleFor(x => x.TransId).GreaterThan(0);
    }
}
