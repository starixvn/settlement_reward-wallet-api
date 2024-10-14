namespace RewardWallet.Features;

public record class AddSabaWalletTicketCommand(
    short ProgramId,
    int AdminId,
    short SiteDBGroupId,
    List<Customer> Customers
    );

public record class Customer(int CustId, decimal Amount);