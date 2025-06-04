namespace RewardWallet.Tests;

public class ConstantsTests
{
    [Fact]
    public void Constants_ValuesAreCorrect()
    {
        // Assert
        Assert.Equal("Reward", Constants.ModuleName);
        Assert.Equal("RewardProgram", Constants.RefIdProgram);
        Assert.Equal("en", Constants.DefaultLanguage);
    }
}