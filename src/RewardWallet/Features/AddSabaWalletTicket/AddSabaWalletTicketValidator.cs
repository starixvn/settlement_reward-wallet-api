using Fanex.RemotingRef;
using FluentValidation;

namespace RewardWallet.Features;

public class AddSabaWalletTicketValidator : AbstractValidator<AddSabaWalletTicketCommand>
{
    private readonly IRemotingRefService remotingRefService;
    private readonly CheckCustomerHandler checkCustomerHandler;

    private const string ProgramIdErrorCode = "1";
    private const string ProgramIdErrorMessage = "Invalid Program ID";

    private const string CustIdErrorCode = "2";
    private const string CustIdErrorMessage = "Invalid Credit CustID or Amount";

    //private const string NotExistsCustIdErrorCode = "3";
    //private const string NotExistsCustIdErrorMessage = "Invalid Credit Cust ID: {InvalidCustIds}";

    public AddSabaWalletTicketValidator(IRemotingRefService remotingRefService, CheckCustomerHandler checkCustomerHandler)
    {
        this.remotingRefService = remotingRefService;
        this.checkCustomerHandler = checkCustomerHandler;

        RuleFor(x => x.ProgramId)
            .GreaterThan((short)0)
            .WithErrorCode(ProgramIdErrorCode)
            .WithMessage(ProgramIdErrorMessage);

        RuleFor(x => x.ProgramId)
            .Must(IsProgramExists)
            .WithErrorCode(ProgramIdErrorCode)
            .WithMessage(ProgramIdErrorMessage);

        RuleFor(x => x.Customers)
            .NotEmpty();

        RuleForEach(x => x.Customers).ChildRules(c =>
        {
            c.RuleFor(x => x.CustId).GreaterThan(0)
                .WithErrorCode(CustIdErrorCode)
                .WithMessage(CustIdErrorMessage);
            c.RuleFor(x => x.Amount).GreaterThan(0)
                .WithErrorCode(CustIdErrorCode)
                .WithMessage(CustIdErrorMessage);
        });

        RuleFor(x => x.Customers)
            .Must((x, _, context) =>
            {
                var cusIds = string.Join(",", x.Customers.Select(x => x.CustId).ToArray());
                var isValid = checkCustomerHandler.IsCustIdsExists(cusIds, x.SiteDbGroupId, out var invalidCustIds);
                context.MessageFormatter.AppendArgument("InvalidCustIds", string.Join(", ", invalidCustIds));
                return isValid;
            })
            .Must(IsNotDupCustId)
            .WithErrorCode(CustIdErrorCode)
            .WithMessage(CustIdErrorMessage);

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

    private bool IsNotDupCustId(List<Customer> customers)
    {
        return customers.DistinctBy(x => x.CustId).Count() == customers.Count;
    }
}
