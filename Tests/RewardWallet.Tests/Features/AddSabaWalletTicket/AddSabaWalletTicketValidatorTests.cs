using Fanex.RemotingRef;
using Moq;
using RewardWallet.Features.AddSabaWalletTicket;
using RewardWallet.Features.CheckCustomer;
using Xunit;

namespace RewardWallet.Tests.Features.AddSabaWalletTicket;

public class AddSabaWalletTicketValidatorTests
{
    private readonly Mock<IRemotingRefService> _remotingRefServiceMock;
    private readonly Mock<CheckCustomerHandler> _checkCustomerHandlerMock;
    private readonly AddSabaWalletTicketValidator _validator;

    public AddSabaWalletTicketValidatorTests()
    {
        _remotingRefServiceMock = new Mock<IRemotingRefService>();
        _checkCustomerHandlerMock = new Mock<CheckCustomerHandler>();
        _validator = new AddSabaWalletTicketValidator(_remotingRefServiceMock.Object, _checkCustomerHandlerMock.Object);
    }

    [Fact]
    public void Validate_WhenProgramIdIsZero_ReturnsError()
    {
        // Arrange
        var command = new AddSabaWalletTicketCommand(0, 1, false, new List<Customer>());

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorCode == "1" && e.ErrorMessage == "Invalid Program ID");
    }

    [Fact]
    public void Validate_WhenProgramIdIsNegative_ReturnsError()
    {
        // Arrange
        var command = new AddSabaWalletTicketCommand(-1, 1, false, new List<Customer>());

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorCode == "1" && e.ErrorMessage == "Invalid Program ID");
    }

    [Fact]
    public void Validate_WhenCustomersListIsEmpty_ReturnsError()
    {
        // Arrange
        var command = new AddSabaWalletTicketCommand(1, 1, false, new List<Customer>());

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Customers");
    }

    [Fact]
    public void Validate_WhenCustomerIdIsZero_ReturnsError()
    {
        // Arrange
        var customers = new List<Customer> { new(0, 100) };
        var command = new AddSabaWalletTicketCommand(1, 1, false, customers);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorCode == "2" && e.ErrorMessage == "Invalid Credit CustID or Amount");
    }

    [Fact]
    public void Validate_WhenCustomerAmountIsZero_ReturnsError()
    {
        // Arrange
        var customers = new List<Customer> { new(1, 0) };
        var command = new AddSabaWalletTicketCommand(1, 1, false, customers);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorCode == "2" && e.ErrorMessage == "Invalid Credit CustID or Amount");
    }

    [Fact]
    public void Validate_WhenAllDataIsValid_ReturnsSuccess()
    {
        // Arrange
        var customers = new List<Customer> { new(1, 100) };
        var command = new AddSabaWalletTicketCommand(1, 1, false, customers);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
} 