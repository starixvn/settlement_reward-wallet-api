namespace RewardWallet.Features.FetchPendingBalance;

using System;
using System.Linq;
using System.Text.Json.Serialization;

public class FetchPendingBalanceResponse
{
    public decimal Amount { get; set; }

    public string CurrencyCode { get; set; } = string.Empty;

    public DateTime? LatestTransDate { get; set; }

    [JsonIgnore] public string Programs { get; set; }

    public int[] ProgramIds
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Programs))
            {
                return [];
            }

            return Programs
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.TryParse(x, out var v) ? v : (int?)null)
                .Where(x => x.HasValue)
                .Select(x => x.Value)
                .ToArray();
        }
    }
}