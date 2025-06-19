namespace RewardWallet.Tests.Infrastructure.Constants;

public class LevelsTests
{
    [Fact]
    public void Levels_ValuesAreCorrect()
    {
        // Assert
        Assert.Equal("Member", RewardInfrastructure.Constants.Levels.Member);
        Assert.Equal("Agent", RewardInfrastructure.Constants.Levels.Agent);
        Assert.Equal("Master", RewardInfrastructure.Constants.Levels.Master);
        Assert.Equal("Super", RewardInfrastructure.Constants.Levels.Super);
    }
}