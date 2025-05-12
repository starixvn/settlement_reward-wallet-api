using RewardInfrastructure.Constants;
using Xunit;

namespace RewardWallet.Tests.Infrastructure.Constants;

public class ContentLanguageTests
{
    [Fact]
    public void ContentLanguage_ValuesAreCorrect()
    {
        // Assert
        Assert.Equal("en", ContentLanguage.English);
        Assert.Equal("id-ID", ContentLanguage.Indo);
        Assert.Equal("zh-TW", ContentLanguage.Cantonese);
        Assert.Equal("zh-CN", ContentLanguage.Mandarin);
    }
} 