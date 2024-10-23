namespace RewardWallet.Features.AddSabaWalletTicket;

public abstract record AddSabaWalletTicketCommand(
    short ProgramId,
    short SiteDbGroupId,
    List<Customer> Customers
    );

public abstract record Customer(int CustId, decimal Amount);