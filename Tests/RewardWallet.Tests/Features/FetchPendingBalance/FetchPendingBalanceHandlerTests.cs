using Fanex.Data.Repository;
using NSubstitute;
using RewardWallet.Features.FetchPendingBalance;

namespace RewardWallet.Tests.Features.FetchPendingBalance;

public class FetchPendingBalanceHandlerTests
{
    private readonly IDynamicRepository repositoryMock;
    private readonly FetchPendingBalanceHandler handler;

    public FetchPendingBalanceHandlerTests()
    {
        repositoryMock = Substitute.For<IDynamicRepository>();
        handler = new FetchPendingBalanceHandler(repositoryMock);
    }

    [Fact]
    public async Task HandleAsync_WhenValidQuery_ReturnsCorrectResponse()
    {
        // Arrange
        var custId = 123;
        var query = new FetchPendingBalanceQuery(custId);
        var expectedResponse = new FetchPendingBalanceResponse
        {
            Amount = 100.50m,
            CurrencyCode = "USD",
            LatestTransDate = DateTime.Now,
            Programs = "1,2,3",
            ProgramIds = [1, 2, 3]
        };

        repositoryMock.GetAsync<FetchPendingBalanceResponse>(
                Arg.Is<FetchPendingBalanceCriteria>(c => c.CustId == custId),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResponse.Amount, result.Amount);
        Assert.Equal(expectedResponse.CurrencyCode, result.CurrencyCode);
        Assert.Equal(expectedResponse.LatestTransDate, result.LatestTransDate);
        Assert.Equal(expectedResponse.Programs, result.Programs);
        Assert.Equal(expectedResponse.ProgramIds, result.ProgramIds);

        await repositoryMock.Received(1).GetAsync<FetchPendingBalanceResponse>(
            Arg.Is<FetchPendingBalanceCriteria>(c => c.CustId == custId),
            cancellationToken: Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenProgramsIsEmpty_ReturnsEmptyProgramIds()
    {
        // Arrange
        var custId = 123;
        var query = new FetchPendingBalanceQuery(custId);
        var responseFromRepository = new FetchPendingBalanceResponse
        {
            Amount = 100.50m,
            CurrencyCode = "USD",
            LatestTransDate = DateTime.Now,
            Programs = "",
            ProgramIds = []
        };

        repositoryMock.GetAsync<FetchPendingBalanceResponse>(
                Arg.Any<FetchPendingBalanceCriteria>(),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(responseFromRepository);

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.ProgramIds);
    }

    [Fact]
    public async Task HandleAsync_WhenProgramsIsNull_ReturnsEmptyProgramIds()
    {
        // Arrange
        var custId = 123;
        var query = new FetchPendingBalanceQuery(custId);
        var responseFromRepository = new FetchPendingBalanceResponse
        {
            Amount = 100.50m,
            CurrencyCode = "USD",
            LatestTransDate = DateTime.Now,
            Programs = null!,
            ProgramIds = []
        };

        repositoryMock.GetAsync<FetchPendingBalanceResponse>(
                Arg.Any<FetchPendingBalanceCriteria>(),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(responseFromRepository);

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.ProgramIds);
    }

    [Fact]
    public async Task HandleAsync_WhenProgramsIsWhitespace_ReturnsEmptyProgramIds()
    {
        // Arrange
        var custId = 123;
        var query = new FetchPendingBalanceQuery(custId);
        var responseFromRepository = new FetchPendingBalanceResponse
        {
            Amount = 100.50m,
            CurrencyCode = "USD",
            LatestTransDate = DateTime.Now,
            Programs = "   ",
            ProgramIds = []
        };

        repositoryMock.GetAsync<FetchPendingBalanceResponse>(
                Arg.Any<FetchPendingBalanceCriteria>(),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(responseFromRepository);

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.ProgramIds);
    }

    [Fact]
    public async Task HandleAsync_WhenProgramsHasValidNumbers_ParsesCorrectly()
    {
        // Arrange
        var custId = 123;
        var query = new FetchPendingBalanceQuery(custId);
        var responseFromRepository = new FetchPendingBalanceResponse
        {
            Amount = 100.50m,
            CurrencyCode = "USD",
            LatestTransDate = DateTime.Now,
            Programs = "1,2,3,4,5",
            ProgramIds = []
        };

        repositoryMock.GetAsync<FetchPendingBalanceResponse>(
                Arg.Any<FetchPendingBalanceCriteria>(),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(responseFromRepository);

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal([1, 2, 3, 4, 5], result.ProgramIds);
    }

    [Fact]
    public async Task HandleAsync_WhenProgramsHasInvalidNumbers_FiltersThemOut()
    {
        // Arrange
        var custId = 123;
        var query = new FetchPendingBalanceQuery(custId);
        var responseFromRepository = new FetchPendingBalanceResponse
        {
            Amount = 100.50m,
            CurrencyCode = "USD",
            LatestTransDate = DateTime.Now,
            Programs = "1,abc,3,def,5",
            ProgramIds = []
        };

        repositoryMock.GetAsync<FetchPendingBalanceResponse>(
                Arg.Any<FetchPendingBalanceCriteria>(),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(responseFromRepository);

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal([1, 3, 5], result.ProgramIds);
    }

    [Fact]
    public async Task HandleAsync_WhenProgramsHasEmptyEntries_RemovesThem()
    {
        // Arrange
        var custId = 123;
        var query = new FetchPendingBalanceQuery(custId);
        var responseFromRepository = new FetchPendingBalanceResponse
        {
            Amount = 100.50m,
            CurrencyCode = "USD",
            LatestTransDate = DateTime.Now,
            Programs = "1,,2,,3",
            ProgramIds = []
        };

        repositoryMock.GetAsync<FetchPendingBalanceResponse>(
                Arg.Any<FetchPendingBalanceCriteria>(),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(responseFromRepository);

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal([1, 2, 3], result.ProgramIds);
    }

    [Fact]
    public async Task HandleAsync_WhenProgramsHasMixedContent_ParsesCorrectly()
    {
        // Arrange
        var custId = 123;
        var query = new FetchPendingBalanceQuery(custId);
        var responseFromRepository = new FetchPendingBalanceResponse
        {
            Amount = 100.50m,
            CurrencyCode = "USD",
            LatestTransDate = DateTime.Now,
            Programs = "1,abc,2,,3,def,4",
            ProgramIds = []
        };

        repositoryMock.GetAsync<FetchPendingBalanceResponse>(
                Arg.Any<FetchPendingBalanceCriteria>(),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(responseFromRepository);

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal([1, 2, 3, 4], result.ProgramIds);
    }

    [Fact]
    public async Task HandleAsync_WhenRepositoryThrowsException_PropagatesException()
    {
        // Arrange
        var custId = 123;
        var query = new FetchPendingBalanceQuery(custId);
        var expectedException = new InvalidOperationException("Database error");

        repositoryMock.GetAsync<FetchPendingBalanceResponse>(
                Arg.Any<FetchPendingBalanceCriteria>(),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(Task.FromException<FetchPendingBalanceResponse>(expectedException));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => handler.HandleAsync(query, CancellationToken.None));

        Assert.Equal(expectedException.Message, exception.Message);
    }

    [Fact]
    public async Task HandleAsync_WhenQueryIsAdapted_CriteriaHasCorrectCustId()
    {
        // Arrange
        var custId = 456;
        var query = new FetchPendingBalanceQuery(custId);
        var responseFromRepository = new FetchPendingBalanceResponse
        {
            Amount = 100.50m,
            CurrencyCode = "USD",
            LatestTransDate = DateTime.Now,
            Programs = "1,2,3",
            ProgramIds = []
        };

        FetchPendingBalanceCriteria capturedCriteria = null!;
        repositoryMock.GetAsync<FetchPendingBalanceResponse>(
                Arg.Any<FetchPendingBalanceCriteria>(),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(responseFromRepository)
            .AndDoes(x => capturedCriteria = x.Arg<FetchPendingBalanceCriteria>());

        // Act
        await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        Assert.NotNull(capturedCriteria);
        Assert.Equal(custId, capturedCriteria.CustId);
        Assert.Equal(2, capturedCriteria.WalletId); // Default value
    }
}