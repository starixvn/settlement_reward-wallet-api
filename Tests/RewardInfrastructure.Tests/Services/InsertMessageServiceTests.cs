using Fanex.Data.Repository;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using RewardInfrastructure.Constants;
using RewardInfrastructure.Criteria;
using RewardInfrastructure.Services;
using RewardInfrastructure.Services.ProgramMessage;
using RewardWallet.Contracts;

namespace RewardInfrastructure.Tests.Services;

public class InsertMessageServiceTests
{
    private readonly IDynamicRepository repository;
    private readonly MessageTemplateProviderFactory templateProviderFactory;
    private readonly InsertMessageService service;

    public InsertMessageServiceTests()
    {
        repository = Substitute.For<IDynamicRepository>();
        templateProviderFactory = Substitute.For<MessageTemplateProviderFactory>();
        service = new InsertMessageServiceImp(repository, templateProviderFactory);
    }

    [Fact]
    public async Task CreateMessageAsync_WhenValidInput_ShouldCreateMessageSuccessfully()
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
        var expectedMessageId = 789;

        var templateProvider = Substitute.For<IMessageTemplateProvider>();
        templateProvider.GetTemplate(level, ContentLanguage.English, ticket).Returns("English template");
        templateProvider.GetTemplate(level, ContentLanguage.Cantonese, ticket).Returns("Cantonese template");
        templateProvider.GetTemplate(level, ContentLanguage.Mandarin, ticket).Returns("Mandarin template");
        templateProvider.GetTemplate(level, ContentLanguage.Thai, ticket).Returns("Thai template");

        templateProviderFactory.GetProvider(ticket.ProgramId).Returns(templateProvider);
        repository.GetAsync<int>(Arg.Any<InsertMessageCriteria>()).Returns(expectedMessageId);

        // Act
        var result = await service.CreateMessageAsync(ticket, level);

        // Assert
        Assert.Equal(expectedMessageId, result);
        await repository.Received(1).GetAsync<int>(Arg.Is<InsertMessageCriteria>(criteria =>
            criteria.ContentEng == "English template" &&
            criteria.ContentCh == "Cantonese template" &&
            criteria.ContentCs == "Mandarin template" &&
            criteria.ContentThai == "Thai template"));
    }

    [Fact]
    public async Task CreateMessageAsync_WhenSuperLevel_ShouldCreateMessageForSuperLevel()
    {
        // Arrange
        var ticket = new SabaWalletTicketItem
        {
            TransId = 123L,
            Amount = 200.75m,
            ProgramId = 2,
            CustId = 456,
            Username = "testuser"
        };

        var level = Levels.Super;
        var expectedMessageId = 999;

        var templateProvider = Substitute.For<IMessageTemplateProvider>();
        templateProvider.GetTemplate(level, ContentLanguage.English, ticket).Returns("Super English template");
        templateProvider.GetTemplate(level, ContentLanguage.Cantonese, ticket).Returns("Super Cantonese template");
        templateProvider.GetTemplate(level, ContentLanguage.Mandarin, ticket).Returns("Super Mandarin template");
        templateProvider.GetTemplate(level, ContentLanguage.Thai, ticket).Returns("Super Thai template");

        templateProviderFactory.GetProvider(ticket.ProgramId).Returns(templateProvider);
        repository.GetAsync<int>(Arg.Any<InsertMessageCriteria>()).Returns(expectedMessageId);

        // Act
        var result = await service.CreateMessageAsync(ticket, level);

        // Assert
        Assert.Equal(expectedMessageId, result);
        await repository.Received(1).GetAsync<int>(Arg.Is<InsertMessageCriteria>(criteria =>
            criteria.ContentEng == "Super English template" &&
            criteria.ContentCh == "Super Cantonese template" &&
            criteria.ContentCs == "Super Mandarin template" &&
            criteria.ContentThai == "Super Thai template"));
    }

    [Fact]
    public async Task CreateMessageAsync_WhenRepositoryThrowsException_ShouldPropagateException()
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

        var templateProvider = Substitute.For<IMessageTemplateProvider>();
        templateProvider.GetTemplate(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<SabaWalletTicketItem>()).Returns("template");

        templateProviderFactory.GetProvider(ticket.ProgramId).Returns(templateProvider);
        repository.GetAsync<int>(Arg.Any<InsertMessageCriteria>()).ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateMessageAsync(ticket, level));
    }

    [Fact]
    public async Task CreateMessageAsync_WhenTemplateProviderFactoryThrowsException_ShouldPropagateException()
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

        templateProviderFactory.GetProvider(ticket.ProgramId).Throws(new InvalidOperationException("Factory error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateMessageAsync(ticket, level));
    }

    [Theory]
    [InlineData(Levels.Agent)]
    [InlineData(Levels.Super)]
    public async Task CreateMessageAsync_WithDifferentLevels_ShouldCallTemplateProviderCorrectly(string level)
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

        var templateProvider = Substitute.For<IMessageTemplateProvider>();
        templateProvider.GetTemplate(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<SabaWalletTicketItem>()).Returns("template");

        templateProviderFactory.GetProvider(ticket.ProgramId).Returns(templateProvider);
        repository.GetAsync<int>(Arg.Any<InsertMessageCriteria>()).Returns(1);

        // Act
        await service.CreateMessageAsync(ticket, level);

        // Assert
        templateProvider.Received(4).GetTemplate(Arg.Any<string>(), Arg.Any<string>(), ticket);
        templateProvider.Received(1).GetTemplate(level, ContentLanguage.English, ticket);
        templateProvider.Received(1).GetTemplate(level, ContentLanguage.Cantonese, ticket);
        templateProvider.Received(1).GetTemplate(level, ContentLanguage.Mandarin, ticket);
        templateProvider.Received(1).GetTemplate(level, ContentLanguage.Thai, ticket);
    }

    [Fact]
    public async Task CreateMessageAsync_WhenAmountHasDecimalPlaces_ShouldFormatCorrectly()
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

        var templateProvider = Substitute.For<IMessageTemplateProvider>();
        templateProvider.GetTemplate(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<SabaWalletTicketItem>()).Returns("Amount: @amount");

        templateProviderFactory.GetProvider(ticket.ProgramId).Returns(templateProvider);
        repository.GetAsync<int>(Arg.Any<InsertMessageCriteria>()).Returns(1);

        // Act
        await service.CreateMessageAsync(ticket, level);

        // Assert
        templateProvider.Received(4).GetTemplate(Arg.Any<string>(), Arg.Any<string>(), ticket);
    }

    [Fact]
    public async Task CreateMessageAsync_WhenZeroAmount_ShouldHandleCorrectly()
    {
        // Arrange
        var ticket = new SabaWalletTicketItem
        {
            TransId = 123L,
            Amount = 0m,
            ProgramId = 1,
            CustId = 456,
            Username = "testuser"
        };

        var level = Levels.Agent;

        var templateProvider = Substitute.For<IMessageTemplateProvider>();
        templateProvider.GetTemplate(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<SabaWalletTicketItem>()).Returns("Amount: @amount");

        templateProviderFactory.GetProvider(ticket.ProgramId).Returns(templateProvider);
        repository.GetAsync<int>(Arg.Any<InsertMessageCriteria>()).Returns(1);

        // Act
        await service.CreateMessageAsync(ticket, level);

        // Assert
        templateProvider.Received(4).GetTemplate(Arg.Any<string>(), Arg.Any<string>(), ticket);
    }
} 