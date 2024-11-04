namespace RewardWallet.Features.AddSabaWalletTicket;

public record class AddSabaWalletTicketCommand(
    short ProgramId,
    short SiteDbGroupId,
    bool IsTest,
    List<Customer> Customers
    );

public record class Customer(int CustId, decimal Amount);