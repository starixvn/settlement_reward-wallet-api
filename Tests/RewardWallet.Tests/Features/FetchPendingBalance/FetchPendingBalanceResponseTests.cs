using RewardWallet.Features.FetchPendingBalance;
using Xunit;

namespace RewardWallet.Tests.Features.FetchPendingBalance;

public class FetchPendingBalanceResponseTests
{
    [Fact]
    public void FetchPendingBalanceResponse_WhenCreated_PropertiesAreSetCorrectly()
    {
        // Arrange & Act
        var response = new FetchPendingBalanceResponse
        {
            Amount = 100.50m,
            CurrencyCode = "USD",
            LatestTransDate = new DateTime(2024, 1, 1)
        };

        // Assert
        Assert.Equal(100.50m, response.Amount);
        Assert.Equal("USD", response.CurrencyCode);
        Assert.Equal(new DateTime(2024, 1, 1), response.LatestTransDate);
    }

    [Fact]
    public void FetchPendingBalanceResponse_WhenCreated_DefaultValuesAreSet()
    {
        // Arrange & Act
        var response = new FetchPendingBalanceResponse();

        // Assert
        Assert.Equal(0, response.Amount);
        Assert.Equal(string.Empty, response.CurrencyCode);
        Assert.Null(response.LatestTransDate);
    }
} 