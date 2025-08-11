namespace RewardWallet.Features.FetchPendingBalance;

using System;
using System.Text.Json.Serialization;

public class FetchPendingBalanceResponse
{
    public decimal Amount { get; set; }

    public string CurrencyCode { get; set; } = string.Empty;

    public DateTime? LatestTransDate { get; set; }

    [JsonIgnore] public string Programs { get; set; }

    public int[] ProgramIds { get; set; }

}