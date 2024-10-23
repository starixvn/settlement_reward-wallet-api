namespace RewardWallet.Features.FetchFeedbackTickets;

public class FetchFeedbackTicketsCriteria : CriteriaBase
{
    public int CustId { get; set; }
    public long TransId { get; set; }

    public override string GetSettingKey() => "NAP_SabaWalletTicket_FeedbackSMA_Sel";

    public override bool IsValid() => true;
}