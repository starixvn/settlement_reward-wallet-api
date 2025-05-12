using Fanex.Data.Repository;
using Moq;
using RewardWallet.Features.CheckCustomer;

namespace RewardWallet.Tests.Features.CheckCustomer;

public class CheckCustomerHandlerTests
{
    private readonly Mock<IDynamicRepository> _repositoryMock;
    private readonly CheckCustomerHandler _handler;

    public CheckCustomerHandlerTests()
    {
        _repositoryMock = new Mock<IDynamicRepository>();
        _handler = new CheckCustomerHandler(_repositoryMock.Object);
    }

    [Fact]
    public void IsCustIdsExists_WhenNoInvalidIds_ReturnsTrue()
    {
        // Arrange
        var cusIds = "1,2,3";
        var siteDbGroupId = (short)1;
        _repositoryMock.Setup(x => x.Fetch<string>(It.IsAny<CheckCustomerCriteria>(), It.IsAny<string>()))
            .Returns(new List<string>());

        // Act
        var result = _handler.IsCustIdsExists(cusIds, siteDbGroupId, out var inValidCusIds);

        // Assert
        Assert.True(result);
        Assert.Empty(inValidCusIds);
    }

    [Fact]
    public void IsCustIdsExists_WhenHasInvalidIds_ReturnsFalse()
    {
        // Arrange
        var cusIds = "1,2,3";
        var siteDbGroupId = (short)1;
        var invalidIds = new List<string> { "2" };
        _repositoryMock.Setup(x => x.Fetch<string>(It.IsAny<CheckCustomerCriteria>(), It.IsAny<string>()))
            .Returns(invalidIds);

        // Act
        var result = _handler.IsCustIdsExists(cusIds, siteDbGroupId, out var inValidCusIds);

        // Assert
        Assert.False(result);
        Assert.Equal("2", inValidCusIds);
    }

    [Fact]
    public void IsCustIdsExists_WhenHasMultipleInvalidIds_ReturnsFalse()
    {
        // Arrange
        var cusIds = "1,2,3";
        var siteDbGroupId = (short)1;
        var invalidIds = new List<string> { "2", "3" };
        _repositoryMock.Setup(x => x.Fetch<string>(It.IsAny<CheckCustomerCriteria>(), It.IsAny<string>()))
            .Returns(invalidIds);

        // Act
        var result = _handler.IsCustIdsExists(cusIds, siteDbGroupId, out var inValidCusIds);

        // Assert
        Assert.False(result);
        Assert.Equal("2,3", inValidCusIds);
    }
}