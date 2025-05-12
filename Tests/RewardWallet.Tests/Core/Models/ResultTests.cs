using Core.Models;
using Xunit;

namespace RewardWallet.Tests.Core.Models;

public class ResultTests
{
    [Fact]
    public void IsSuccess_WhenErrorCodeIsEmpty_ReturnsTrue()
    {
        // Arrange
        var result = new Result(string.Empty, "Test message");

        // Act & Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void IsSuccess_WhenErrorCodeIsZero_ReturnsTrue()
    {
        // Arrange
        var result = new Result("0", "Test message");

        // Act & Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void IsSuccess_WhenErrorCodeIsNotEmptyAndNotZero_ReturnsFalse()
    {
        // Arrange
        var result = new Result("1", "Test message");

        // Act & Assert
        Assert.False(result.IsSuccess);
    }
} 