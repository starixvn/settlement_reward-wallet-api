namespace RewardWallet.Contracts;

public class SabaWalletTicketItem
{
    public long TransId { get; set; }

    public decimal Amount { get; set; }

    public short ProgramId { get; set; }

    public DateTime APaymentDate { get; set; }

    public bool? IsComplaint { get; set; }

    #region custom info

    public int CustId { get; set; }

    public string Username { get; set; } = string.Empty;

    public int AgentId { get; set; }

    public string AgentUsername { get; set; } = string.Empty;

    public int MasterId { get; set; }

    public string MasterUsername { get; set; } = string.Empty;

    public int SuperId { get; set; }

    public string SuperUsername { get; set; } = string.Empty;

    #endregion custom info
}