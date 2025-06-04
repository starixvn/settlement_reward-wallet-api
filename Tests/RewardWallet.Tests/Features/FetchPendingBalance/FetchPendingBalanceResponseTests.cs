using RewardWallet.Features.FetchPendingBalance;

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

    [Fact]
    public void ProgramIds_ShouldBeEmptyArray_WhenProgramsIsNullOrEmpty()
    {
        var response1 = new FetchPendingBalanceResponse { Programs = null };
        var response2 = new FetchPendingBalanceResponse { Programs = "" };
        Assert.Empty(response1.ProgramIds);
        Assert.Empty(response2.ProgramIds);
    }

    [Fact]
    public void ProgramIds_ShouldReturnCorrectArray_WhenProgramsIsValidString()
    {
        var response = new FetchPendingBalanceResponse { Programs = "1,2,100,99" };
        Assert.Equal(new[] { 1, 2, 100, 99 }, response.ProgramIds);
    }

    [Fact]
    public void ProgramIds_ShouldIgnoreInvalidValues()
    {
        var response = new FetchPendingBalanceResponse { Programs = "1,abc,2,,99" };
        Assert.Equal(new[] { 1, 2, 99 }, response.ProgramIds);
    }
}