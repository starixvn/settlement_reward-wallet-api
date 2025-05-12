using RewardWallet.Contracts;
using Xunit;

namespace RewardWallet.Tests.Contracts;

public class SabaWalletTicketItemTests
{
    [Fact]
    public void SabaWalletTicketItem_WhenCreated_PropertiesAreSetCorrectly()
    {
        // Arrange & Act
        var item = new SabaWalletTicketItem
        {
            TransId = 123,
            Amount = 100.50m,
            ProgramId = 1,
            APaymentDate = new DateTime(2024, 1, 1),
            IsComplaint = true,
            CustId = 456,
            Username = "testuser",
            AgentId = 789,
            AgentUsername = "agent1",
            MasterId = 101,
            MasterUsername = "master1",
            SuperId = 102,
            SuperUsername = "super1"
        };

        // Assert
        Assert.Equal(123, item.TransId);
        Assert.Equal(100.50m, item.Amount);
        Assert.Equal(1, item.ProgramId);
        Assert.Equal(new DateTime(2024, 1, 1), item.APaymentDate);
        Assert.True(item.IsComplaint);
        Assert.Equal(456, item.CustId);
        Assert.Equal("testuser", item.Username);
        Assert.Equal(789, item.AgentId);
        Assert.Equal("agent1", item.AgentUsername);
        Assert.Equal(101, item.MasterId);
        Assert.Equal("master1", item.MasterUsername);
        Assert.Equal(102, item.SuperId);
        Assert.Equal("super1", item.SuperUsername);
    }

    [Fact]
    public void SabaWalletTicketItem_WhenCreated_DefaultValuesAreSet()
    {
        // Arrange & Act
        var item = new SabaWalletTicketItem();

        // Assert
        Assert.Equal(string.Empty, item.Username);
        Assert.Equal(string.Empty, item.AgentUsername);
        Assert.Equal(string.Empty, item.MasterUsername);
        Assert.Equal(string.Empty, item.SuperUsername);
        Assert.Null(item.IsComplaint);
    }
} 