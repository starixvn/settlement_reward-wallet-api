using Fanex.RemotingRef;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using RewardInfrastructure.Constants;
using RewardInfrastructure.Services.ProgramMessage;
using RewardWallet.Contracts;

namespace RewardInfrastructure.Tests.Services.ProgramMessage;

public class ProgramId3TemplateProviderTests
{
    private readonly IRemotingRefService remotingRefService;
    private readonly ProgramId3TemplateProvider provider;

    public ProgramId3TemplateProviderTests()
    {
        remotingRefService = Substitute.For<IRemotingRefService>();
        provider = new ProgramId3TemplateProvider(remotingRefService);
    }

    [Fact]
    public void GetTemplate_WhenAgentLevelAndEnglish_ShouldReturnAgentEnglishTemplate()
    {
        // Arrange
        var ticket = new SabaWalletTicketItem
        {
            TransId = 123L,
            Amount = 100.50m,
            ProgramId = 3,
            CustId = 456,
            Username = "testuser"
        };

        var level = Levels.Agent;
        var languageCode = ContentLanguage.English;
        var programName = "Six Score Challenge";

        remotingRefService.GetResourceDataByRefIdAndResourceId("RewardProgram", "3", languageCode, "ProgramName")
            .Returns(programName);

        // Act
        var result = provider.GetTemplate(level, languageCode, ticket);

        // Assert
        Assert.Contains("Dear valued agents", result);
        Assert.Contains("testuser", result);
        Assert.Contains("100.5", result);
        Assert.Contains(programName, result);
        Assert.DoesNotContain("@program_name", result);
        Assert.DoesNotContain("@member_username", result);
        Assert.DoesNotContain("@amount", result);
    }

    [Fact]
    public void GetTemplate_WhenAgentLevelAndCantonese_ShouldReturnAgentCantoneseTemplate()
    {
        // Arrange
        var ticket = new SabaWalletTicketItem
        {
            TransId = 123L,
            Amount = 200.75m,
            ProgramId = 3,
            CustId = 456,
            Username = "testuser"
        };

        var level = Levels.Agent;
        var languageCode = ContentLanguage.Cantonese;
        var programName = "六分挑戰";

        remotingRefService.GetResourceDataByRefIdAndResourceId("RewardProgram", "3", languageCode, "ProgramName")
            .Returns(programName);

        // Act
        var result = provider.GetTemplate(level, languageCode, ticket);

        // Assert
        Assert.Contains("亲爱的代理伙伴您好", result);
        Assert.Contains("testuser", result);
        Assert.Contains("200.75", result);
        Assert.Contains(programName, result);
    }

    [Fact]
    public void GetTemplate_WhenAgentLevelAndMandarin_ShouldReturnAgentMandarinTemplate()
    {
        // Arrange
        var ticket = new SabaWalletTicketItem
        {
            TransId = 123L,
            Amount = 300.25m,
            ProgramId = 3,
            CustId = 456,
            Username = "testuser"
        };

        var level = Levels.Agent;
        var languageCode = ContentLanguage.Mandarin;
        var programName = "六分挑战";

        remotingRefService.GetResourceDataByRefIdAndResourceId("RewardProgram", "3", languageCode, "ProgramName")
            .Returns(programName);

        // Act
        var result = provider.GetTemplate(level, languageCode, ticket);

        // Assert
        Assert.Contains("親愛的代理夥伴您好", result);
        Assert.Contains("testuser", result);
        Assert.Contains("300.25", result);
        Assert.Contains(programName, result);
    }

    [Fact]
    public void GetTemplate_WhenAgentLevelAndThai_ShouldReturnAgentThaiTemplate()
    {
        // Arrange
        var ticket = new SabaWalletTicketItem
        {
            TransId = 123L,
            Amount = 400.01m,
            ProgramId = 3,
            CustId = 456,
            Username = "testuser"
        };

        var level = Levels.Agent;
        var languageCode = ContentLanguage.Thai;
        var programName = "Six Score Challenge";

        remotingRefService.GetResourceDataByRefIdAndResourceId("RewardProgram", "3", languageCode, "ProgramName")
            .Returns(programName);

        // Act
        var result = provider.GetTemplate(level, languageCode, ticket);

        // Assert
        Assert.Contains("เรียน เอเย่นต์ที่เคารพ", result);
        Assert.Contains("testuser", result);
        Assert.Contains("400.01", result);
        Assert.Contains(programName, result);
    }

    [Fact]
    public void GetTemplate_WhenSuperLevelAndEnglish_ShouldReturnSuperEnglishTemplate()
    {
        // Arrange
        var ticket = new SabaWalletTicketItem
        {
            TransId = 123L,
            Amount = 100.51m,
            ProgramId = 3,
            CustId = 456,
            Username = "testuser"
        };

        var level = Levels.Super;
        var languageCode = ContentLanguage.English;
        var programName = "Six Score Challenge";

        remotingRefService.GetResourceDataByRefIdAndResourceId("RewardProgram", "3", languageCode, "ProgramName")
            .Returns(programName);

        // Act
        var result = provider.GetTemplate(level, languageCode, ticket);

        // Assert
        Assert.Contains("Dear valued agents", result);
        Assert.Contains("testuser", result);
        Assert.Contains("100.51", result);
        Assert.Contains(programName, result);
        Assert.Contains("investigate and ensure", result);
    }

    [Fact]
    public void GetTemplate_WhenSuperLevelAndCantonese_ShouldReturnSuperCantoneseTemplate()
    {
        // Arrange
        var ticket = new SabaWalletTicketItem
        {
            TransId = 123L,
            Amount = 200.75m,
            ProgramId = 3,
            CustId = 456,
            Username = "testuser"
        };

        var level = Levels.Super;
        var languageCode = ContentLanguage.Cantonese;
        var programName = "六分挑戰";

        remotingRefService.GetResourceDataByRefIdAndResourceId("RewardProgram", "3", languageCode, "ProgramName")
            .Returns(programName);

        // Act
        var result = provider.GetTemplate(level, languageCode, ticket);

        // Assert
        Assert.Contains("亲爱的代理伙伴您好", result);
        Assert.Contains("testuser", result);
        Assert.Contains("200.75", result);
        Assert.Contains(programName, result);
    }

    [Fact]
    public void GetTemplate_WhenSuperLevelAndMandarin_ShouldReturnSuperMandarinTemplate()
    {
        // Arrange
        var ticket = new SabaWalletTicketItem
        {
            TransId = 123L,
            Amount = 300.25m,
            ProgramId = 3,
            CustId = 456,
            Username = "testuser"
        };

        var level = Levels.Super;
        var languageCode = ContentLanguage.Mandarin;
        var programName = "六分挑战";

        remotingRefService.GetResourceDataByRefIdAndResourceId("RewardProgram", "3", languageCode, "ProgramName")
            .Returns(programName);

        // Act
        var result = provider.GetTemplate(level, languageCode, ticket);

        // Assert
        Assert.Contains("親愛的代理夥伴您好", result);
        Assert.Contains("testuser", result);
        Assert.Contains("300.25", result);
        Assert.Contains(programName, result);
    }

    [Fact]
    public void GetTemplate_WhenSuperLevelAndThai_ShouldReturnSuperThaiTemplate()
    {
        // Arrange
        var ticket = new SabaWalletTicketItem
        {
            TransId = 123L,
            Amount = 400.01m,
            ProgramId = 3,
            CustId = 456,
            Username = "testuser"
        };

        var level = Levels.Super;
        var languageCode = ContentLanguage.Thai;
        var programName = "Six Score Challenge";

        remotingRefService.GetResourceDataByRefIdAndResourceId("RewardProgram", "3", languageCode, "ProgramName")
            .Returns(programName);

        // Act
        var result = provider.GetTemplate(level, languageCode, ticket);

        // Assert
        Assert.Contains("เรียน เอเย่นต์ที่เคารพ", result);
        Assert.Contains("testuser", result);
        Assert.Contains("400.01", result);
        Assert.Contains(programName, result);
    }

    [Fact]
    public void GetTemplate_WhenUnknownLevel_ShouldReturnEmptyString()
    {
        // Arrange
        var ticket = new SabaWalletTicketItem
        {
            TransId = 123L,
            Amount = 100.50m,
            ProgramId = 3,
            CustId = 456,
            Username = "testuser"
        };

        var level = "UnknownLevel";
        var languageCode = ContentLanguage.English;

        // Act
        var result = provider.GetTemplate(level, languageCode, ticket);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GetTemplate_WhenUnknownLanguage_ShouldReturnEmptyString()
    {
        // Arrange
        var ticket = new SabaWalletTicketItem
        {
            TransId = 123L,
            Amount = 100.50m,
            ProgramId = 3,
            CustId = 456,
            Username = "testuser"
        };

        var level = Levels.Agent;
        var languageCode = "unknown";

        // Act
        var result = provider.GetTemplate(level, languageCode, ticket);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GetTemplate_WhenRemotingRefServiceReturnsNull_ShouldHandleCorrectly()
    {
        // Arrange
        var ticket = new SabaWalletTicketItem
        {
            TransId = 123L,
            Amount = 100.51m,
            ProgramId = 3,
            CustId = 456,
            Username = "testuser"
        };

        var level = Levels.Agent;
        var languageCode = ContentLanguage.English;

        remotingRefService.GetResourceDataByRefIdAndResourceId("RewardProgram", "3", languageCode, "ProgramName")
            .Returns((string)null);

        // Act
        var result = provider.GetTemplate(level, languageCode, ticket);

        // Assert
        Assert.Contains("testuser", result);
        Assert.Contains("100.51", result);
        Assert.Contains("", result); // null program name becomes empty string
    }

    [Fact]
    public void GetTemplate_WhenRemotingRefServiceThrowsException_ShouldPropagateException()
    {
        // Arrange
        var ticket = new SabaWalletTicketItem
        {
            TransId = 123L,
            Amount = 100.50m,
            ProgramId = 3,
            CustId = 456,
            Username = "testuser"
        };

        var level = Levels.Agent;
        var languageCode = ContentLanguage.English;

        remotingRefService.GetResourceDataByRefIdAndResourceId("RewardProgram", "3", languageCode, "ProgramName")
            .Throws(new InvalidOperationException("Service error"));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => provider.GetTemplate(level, languageCode, ticket));
    }

    [Fact]
    public void GetTemplate_WhenAmountHasDecimalPlaces_ShouldFormatCorrectly()
    {
        // Arrange
        var ticket = new SabaWalletTicketItem
        {
            TransId = 123L,
            Amount = 1234.567m,
            ProgramId = 3,
            CustId = 456,
            Username = "testuser"
        };

        var level = Levels.Agent;
        var languageCode = ContentLanguage.English;
        var programName = "Six Score Challenge";

        remotingRefService.GetResourceDataByRefIdAndResourceId("RewardProgram", "3", languageCode, "ProgramName")
            .Returns(programName);

        // Act
        var result = provider.GetTemplate(level, languageCode, ticket);

        // Assert
        Assert.Contains("1,234.57", result);
    }

    [Fact]
    public void GetTemplate_WhenAmountIsZero_ShouldFormatCorrectly()
    {
        // Arrange
        var ticket = new SabaWalletTicketItem
        {
            TransId = 123L,
            Amount = 0m,
            ProgramId = 3,
            CustId = 456,
            Username = "testuser"
        };

        var level = Levels.Agent;
        var languageCode = ContentLanguage.English;
        var programName = "Six Score Challenge";

        remotingRefService.GetResourceDataByRefIdAndResourceId("RewardProgram", "3", languageCode, "ProgramName")
            .Returns(programName);

        // Act
        var result = provider.GetTemplate(level, languageCode, ticket);

        // Assert
        Assert.Contains("0", result);
    }

    [Theory]
    [InlineData(Levels.Agent, ContentLanguage.English)]
    [InlineData(Levels.Agent, ContentLanguage.Cantonese)]
    [InlineData(Levels.Agent, ContentLanguage.Mandarin)]
    [InlineData(Levels.Agent, ContentLanguage.Thai)]
    [InlineData(Levels.Super, ContentLanguage.English)]
    [InlineData(Levels.Super, ContentLanguage.Cantonese)]
    [InlineData(Levels.Super, ContentLanguage.Mandarin)]
    [InlineData(Levels.Super, ContentLanguage.Thai)]
    public void GetTemplate_WithValidCombinations_ShouldReplaceAllPlaceholders(string level, string languageCode)
    {
        // Arrange
        var ticket = new SabaWalletTicketItem
        {
            TransId = 123L,
            Amount = 100.51m,
            ProgramId = 3,
            CustId = 456,
            Username = "testuser"
        };

        var programName = "Test Program";

        remotingRefService.GetResourceDataByRefIdAndResourceId("RewardProgram", "3", languageCode, "ProgramName")
            .Returns(programName);

        // Act
        var result = provider.GetTemplate(level, languageCode, ticket);

        // Assert
        Assert.Contains("testuser", result);
        Assert.Contains("100.51", result);
        Assert.Contains(programName, result);
        Assert.DoesNotContain("@program_name", result);
        Assert.DoesNotContain("@member_username", result);
        Assert.DoesNotContain("@amount", result);
    }

    [Fact]
    public void GetTemplate_WhenUsernameIsEmpty_ShouldHandleCorrectly()
    {
        // Arrange
        var ticket = new SabaWalletTicketItem
        {
            TransId = 123L,
            Amount = 100.51m,
            ProgramId = 3,
            CustId = 456,
            Username = ""
        };

        var level = Levels.Agent;
        var languageCode = ContentLanguage.English;
        var programName = "Six Score Challenge";

        remotingRefService.GetResourceDataByRefIdAndResourceId("RewardProgram", "3", languageCode, "ProgramName")
            .Returns(programName);

        // Act
        var result = provider.GetTemplate(level, languageCode, ticket);

        // Assert
        Assert.Contains("", result);
        Assert.Contains("100.51", result);
        Assert.Contains(programName, result);
    }
}