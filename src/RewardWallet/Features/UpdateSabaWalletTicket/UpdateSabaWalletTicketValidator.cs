using FluentValidation;

namespace RewardWallet.Features;

public class UpdateSabaWalletTicketValidator : AbstractValidator<UpdateSabaWalletTicketCommand>
{
    private const string NotExistsCustIdErrorCode = "3";
    private const string NotExistsCustIdErrorMessage = "Not Valid Credit CustId";

    public UpdateSabaWalletTicketValidator(CheckCustomerHandler checkCustomerHandler)
    {
        RuleFor(x => x.CustId)
            .GreaterThan(0)
     .WithErrorCode(NotExistsCustIdErrorCode)
            .WithMessage(NotExistsCustIdErrorMessage);

        RuleFor(x => new { x.CustId, x.SiteDbGroupId })
            .Must((x, _, context) =>
            {
                var isValid = checkCustomerHandler.IsCustIdsExists(x.CustId.ToString(), x.SiteDbGroupId, out var invalidCustIds);
                context.MessageFormatter.AppendArgument("InvalidCustIds", string.Join(", ", invalidCustIds));
                return isValid;
            })
            .WithErrorCode(NotExistsCustIdErrorCode)
            .WithMessage(NotExistsCustIdErrorMessage);

        RuleFor(x => x.TransId).GreaterThan(0);
    }
}
