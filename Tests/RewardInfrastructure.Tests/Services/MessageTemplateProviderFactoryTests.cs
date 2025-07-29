using Fanex.RemotingRef;
using NSubstitute;
using RewardInfrastructure.Constants;
using RewardInfrastructure.Services;
using RewardInfrastructure.Services.ProgramMessage;

namespace RewardInfrastructure.Tests.Services;

public class MessageTemplateProviderFactoryTests
{
    private readonly MessageTemplateProviderFactory factory;

    public MessageTemplateProviderFactoryTests()
    {
        var remotingRefService1 = Substitute.For<IRemotingRefService>();
        factory = new MessageTemplateProviderFactoryImp(remotingRefService1);
    }

    [Fact]
    public void GetProvider_WhenProgramIdIsSixScoreChallenge_ShouldReturnProgramId3TemplateProvider()
    {
        // Arrange
        var programId = ProgramId.SixScoreChallenge;

        // Act
        var result = factory.GetProvider(programId);

        // Assert
        Assert.IsType<ProgramId3TemplateProvider>(result);
    }

    [Fact]
    public void GetProvider_WhenProgramIdIsNotSixScoreChallenge_ShouldReturnDefaultTemplateProvider()
    {
        // Arrange
        var programId = 999;

        // Act
        var result = factory.GetProvider(programId);

        // Assert
        Assert.IsType<DefaultTemplateProvider>(result);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(100)]
    public void GetProvider_WithDifferentProgramIds_ShouldReturnDefaultTemplateProvider(int programId)
    {
        // Act
        var result = factory.GetProvider(programId);

        // Assert
        Assert.IsType<DefaultTemplateProvider>(result);
    }

    [Fact]
    public void GetProvider_WhenProgramIdIsZero_ShouldReturnDefaultTemplateProvider()
    {
        // Arrange
        var programId = 0;

        // Act
        var result = factory.GetProvider(programId);

        // Assert
        Assert.IsType<DefaultTemplateProvider>(result);
    }

    [Fact]
    public void GetProvider_WhenProgramIdIsNegative_ShouldReturnDefaultTemplateProvider()
    {
        // Arrange
        var programId = -1;

        // Act
        var result = factory.GetProvider(programId);

        // Assert
        Assert.IsType<DefaultTemplateProvider>(result);
    }

    [Fact]
    public void GetProvider_WhenProgramIdIsSixScoreChallenge_ShouldCreateProviderWithRemotingRefService()
    {
        // Arrange
        var programId = ProgramId.SixScoreChallenge;

        // Act
        var result = factory.GetProvider(programId);

        // Assert
        var programId3Provider = Assert.IsType<ProgramId3TemplateProvider>(result);
        // Note: We can't directly test the internal field, but we can verify the type
        Assert.NotNull(programId3Provider);
    }

    [Fact]
    public void GetProvider_WhenCalledMultipleTimesWithSameProgramId_ShouldReturnSameType()
    {
        // Arrange
        var programId = ProgramId.SixScoreChallenge;

        // Act
        var result1 = factory.GetProvider(programId);
        var result2 = factory.GetProvider(programId);

        // Assert
        Assert.IsType<ProgramId3TemplateProvider>(result1);
        Assert.IsType<ProgramId3TemplateProvider>(result2);
    }

    [Fact]
    public void GetProvider_WhenCalledMultipleTimesWithDifferentProgramIds_ShouldReturnCorrectTypes()
    {
        // Arrange
        var sixScoreProgramId = ProgramId.SixScoreChallenge;
        var otherProgramId = 999;

        // Act
        var sixScoreResult = factory.GetProvider(sixScoreProgramId);
        var otherResult = factory.GetProvider(otherProgramId);

        // Assert
        Assert.IsType<ProgramId3TemplateProvider>(sixScoreResult);
        Assert.IsType<DefaultTemplateProvider>(otherResult);
    }

    [Fact]
    public void GetProvider_WhenProgramIdIsMaxValue_ShouldReturnDefaultTemplateProvider()
    {
        // Arrange
        var programId = int.MaxValue;

        // Act
        var result = factory.GetProvider(programId);

        // Assert
        Assert.IsType<DefaultTemplateProvider>(result);
    }

    [Fact]
    public void GetProvider_WhenProgramIdIsMinValue_ShouldReturnDefaultTemplateProvider()
    {
        // Arrange
        var programId = int.MinValue;

        // Act
        var result = factory.GetProvider(programId);

        // Assert
        Assert.IsType<DefaultTemplateProvider>(result);
    }
}