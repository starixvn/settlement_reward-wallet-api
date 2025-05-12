using RewardInfrastructure.Constants;
using Xunit;

namespace RewardWallet.Tests.Infrastructure.Constants;

public class LevelsTests
{
    [Fact]
    public void Levels_ValuesAreCorrect()
    {
        // Assert
        Assert.Equal("Member", Levels.Member);
        Assert.Equal("Agent", Levels.Agent);
        Assert.Equal("Master", Levels.Master);
        Assert.Equal("Super", Levels.Super);
    }
} 