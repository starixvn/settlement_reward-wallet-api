namespace RewardWallet.Tests;

public class LevelsTests
{
    [Fact]
    public void Constants_ValuesAreCorrect()
    {
        // Assert
        Assert.Equal("Reward", Levels.ModuleName);
        Assert.Equal("RewardProgram", Levels.RefIdProgram);
        Assert.Equal("en", Levels.DefaultLanguage);
    }
}