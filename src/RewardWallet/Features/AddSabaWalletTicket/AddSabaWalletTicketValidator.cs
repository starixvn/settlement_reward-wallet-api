using Fanex.RemotingRef;
using FluentValidation;

namespace RewardWallet.Features;

public class AddSabaWalletTicketValidator : AbstractValidator<AddSabaWalletTicketCommand>
{
    private readonly IRemotingRefService remotingRefService;
    private readonly CheckCustomerHandler checkCustomerHandler;
    public AddSabaWalletTicketValidator(IRemotingRefService remotingRefService, CheckCustomerHandler checkCustomerHandler)
    {
        this.remotingRefService = remotingRefService;
        this.checkCustomerHandler = checkCustomerHandler;

        RuleFor(x => x.ProgramId)
            .GreaterThan((short)0);

        RuleFor(x => x.ProgramId)
            .Must(IsProgramExists);

        RuleFor(x => x.Customers)
            .NotEmpty();

        RuleForEach(x => x.Customers).ChildRules(c =>
        {
            c.RuleFor(x => x.CustId).GreaterThan(0);
            c.RuleFor(x => x.Amount).GreaterThan(0);
        });

        RuleFor(x => x.Customers)
            .Must((x, _, context) =>
            {
                var cusIds = string.Join(",", x.Customers.Select(x => x.CustId).ToArray());
                var isValid = checkCustomerHandler.IsCustIdsExists(cusIds, x.SiteDbGroupId, out var invalidCustIds);
                context.MessageFormatter.AppendArgument("InvalidCustIds", string.Join(", ", invalidCustIds));
                return isValid;
            })
            .WithMessage("Customer IDs do not exist: {InvalidCustIds}");

    }


    private bool IsProgramExists(short programId)
    {
        var program = remotingRefService.GetResourceDataByRefIdAndResourceId(
            Constants.RefIdProgram,
            programId.ToString(),
            Constants.DefaultLanguage,
            string.Empty);

        return !string.IsNullOrEmpty(program);
    }
}
