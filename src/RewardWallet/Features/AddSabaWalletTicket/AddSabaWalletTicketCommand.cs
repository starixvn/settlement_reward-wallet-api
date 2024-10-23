namespace RewardWallet.Features.AddSabaWalletTicket;

public record class AddSabaWalletTicketCommand(
    short ProgramId,
    short SiteDbGroupId,
    List<Customer> Customers
    );

public record class Customer(int CustId, decimal Amount);