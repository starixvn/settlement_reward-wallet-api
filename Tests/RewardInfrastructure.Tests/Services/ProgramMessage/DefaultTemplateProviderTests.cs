using RewardInfrastructure.Constants;
using RewardInfrastructure.Services.ProgramMessage;
using RewardWallet.Contracts;

namespace RewardInfrastructure.Tests.Services.ProgramMessage;

public class DefaultTemplateProviderTests
{
    private readonly DefaultTemplateProvider provider;

    public DefaultTemplateProviderTests()
    {
        provider = new DefaultTemplateProvider();
    }

    [Fact]
    public void GetTemplate_WhenAgentLevelAndEnglish_ShouldReturnAgentEnglishTemplate()
    {
        // Arrange
        var ticket = new SabaWalletTicketItem
        {
            TransId = 123L,
            Amount = 100.50m,
            ProgramId = 1,
            CustId = 456,
            Username = "testuser"
        };

        var level = Levels.Agent;
        var languageCode = ContentLanguage.English;

        // Act
        var result = provider.GetTemplate(level, languageCode, ticket);

        // Assert
        Assert.Contains("Dear valued agents", result);
        Assert.Contains("testuser", result);
        Assert.Contains("100.5", result);
    }

    [Fact]
    public void GetTemplate_WhenAgentLevelAndCantonese_ShouldReturnAgentCantoneseTemplate()
    {
        // Arrange
        var ticket = new SabaWalletTicketItem
        {
            TransId = 123L,
            Amount = 200.75m,
            ProgramId = 1,
            CustId = 456,
            Username = "testuser"
        };

        var level = Levels.Agent;
        var languageCode = ContentLanguage.Cantonese;

        // Act
        var result = provider.GetTemplate(level, languageCode, ticket);

        // Assert
        Assert.Contains("尊敬的代理們", result);
        Assert.Contains("testuser", result);
        Assert.Contains("200.75", result);
    }

    [Fact]
    public void GetTemplate_WhenAgentLevelAndMandarin_ShouldReturnAgentMandarinTemplate()
    {
        // Arrange
        var ticket = new SabaWalletTicketItem
        {
            TransId = 123L,
            Amount = 300.25m,
            ProgramId = 1,
            CustId = 456,
            Username = "testuser"
        };

        var level = Levels.Agent;
        var languageCode = ContentLanguage.Mandarin;

        // Act
        var result = provider.GetTemplate(level, languageCode, ticket);

        // Assert
        Assert.Contains("尊敬的代理们", result);
        Assert.Contains("testuser", result);
        Assert.Contains("300.25", result);
    }

    [Fact]
    public void GetTemplate_WhenSuperLevelAndEnglish_ShouldReturnSuperEnglishTemplate()
    {
        // Arrange
        var ticket = new SabaWalletTicketItem
        {
            TransId = 123L,
            Amount = 100.50m,
            ProgramId = 1,
            CustId = 456,
            Username = "testuser"
        };

        var level = Levels.Super;
        var languageCode = ContentLanguage.English;

        // Act
        var result = provider.GetTemplate(level, languageCode, ticket);

        // Assert
        Assert.Contains("Dear valued agents", result);
        Assert.Contains("testuser", result);
        Assert.Contains("100.5", result);
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
            ProgramId = 1,
            CustId = 456,
            Username = "testuser"
        };

        var level = Levels.Super;
        var languageCode = ContentLanguage.Cantonese;

        // Act
        var result = provider.GetTemplate(level, languageCode, ticket);

        // Assert
        Assert.Contains("尊敬的代理們", result);
        Assert.Contains("testuser", result);
        Assert.Contains("200.75", result);
    }

    [Fact]
    public void GetTemplate_WhenSuperLevelAndMandarin_ShouldReturnSuperMandarinTemplate()
    {
        // Arrange
        var ticket = new SabaWalletTicketItem
        {
            TransId = 123L,
            Amount = 300.25m,
            ProgramId = 1,
            CustId = 456,
            Username = "testuser"
        };

        var level = Levels.Super;
        var languageCode = ContentLanguage.Mandarin;

        // Act
        var result = provider.GetTemplate(level, languageCode, ticket);

        // Assert
        Assert.Contains("尊敬的代理们", result);
        Assert.Contains("testuser", result);
        Assert.Contains("300.25", result);
    }

    [Fact]
    public void GetTemplate_WhenUnknownLevel_ShouldReturnEmptyString()
    {
        // Arrange
        var ticket = new SabaWalletTicketItem
        {
            TransId = 123L,
            Amount = 100.50m,
            ProgramId = 1,
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
            ProgramId = 1,
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
    public void GetTemplate_WhenAmountHasDecimalPlaces_ShouldFormatCorrectly()
    {
        // Arrange
        var ticket = new SabaWalletTicketItem
        {
            TransId = 123L,
            Amount = 1234.567m,
            ProgramId = 1,
            CustId = 456,
            Username = "testuser"
        };

        var level = Levels.Agent;
        var languageCode = ContentLanguage.English;

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
            Amount = 0.0m,
            ProgramId = 1,
            CustId = 456,
            Username = "testuser"
        };

        var level = Levels.Agent;
        var languageCode = ContentLanguage.English;

        // Act
        var result = provider.GetTemplate(level, languageCode, ticket);

        // Assert
        Assert.Contains("0", result);
    }

    [Fact]
    public void GetTemplate_WhenUsernameIsEmpty_ShouldHandleCorrectly()
    {
        // Arrange
        var ticket = new SabaWalletTicketItem
        {
            TransId = 123L,
            Amount = 100.50m,
            ProgramId = 1,
            CustId = 456,
            Username = ""
        };

        var level = Levels.Agent;
        var languageCode = ContentLanguage.English;

        // Act
        var result = provider.GetTemplate(level, languageCode, ticket);

        // Assert
        Assert.Contains("", result);
    }

    [Theory]
    [InlineData(Levels.Agent, ContentLanguage.English)]
    [InlineData(Levels.Agent, ContentLanguage.Cantonese)]
    [InlineData(Levels.Agent, ContentLanguage.Mandarin)]
    [InlineData(Levels.Super, ContentLanguage.English)]
    [InlineData(Levels.Super, ContentLanguage.Cantonese)]
    [InlineData(Levels.Super, ContentLanguage.Mandarin)]
    public void GetTemplate_WithValidCombinations_ShouldReplacePlaceholders(string level, string languageCode)
    {
        // Arrange
        var ticket = new SabaWalletTicketItem
        {
            TransId = 123L,
            Amount = 100.50m,
            ProgramId = 1,
            CustId = 456,
            Username = "testuser"
        };

        // Act
        var result = provider.GetTemplate(level, languageCode, ticket);

        // Assert
        Assert.Contains("testuser", result);
        Assert.Contains("100.5", result);
        Assert.DoesNotContain("@member_username", result);
        Assert.DoesNotContain("@amount", result);
    }

    [Fact]
    public void GetTemplate_WhenThaiLanguage_ShouldReturnEmptyString()
    {
        // Arrange
        var ticket = new SabaWalletTicketItem
        {
            TransId = 123L,
            Amount = 100.50m,
            ProgramId = 1,
            CustId = 456,
            Username = "testuser"
        };

        var level = Levels.Agent;
        var languageCode = ContentLanguage.Thai;

        // Act
        var result = provider.GetTemplate(level, languageCode, ticket);

        // Assert
        Assert.Equal(string.Empty, result);
    }
}