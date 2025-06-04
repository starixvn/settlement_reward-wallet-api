using Fanex.Data.Repository;
using Fanex.RemotingRef;
using NSubstitute;
using RewardWallet.Features.AddSabaWalletTicket;
using RewardWallet.Features.CheckCustomer;

namespace RewardWallet.Tests.Features.AddSabaWalletTicket;

public class AddSabaWalletTicketValidatorTests
{
    private readonly IRemotingRefService remotingRefServiceSub;
    private readonly AddSabaWalletTicketValidator validator;

    public AddSabaWalletTicketValidatorTests()
    {
        remotingRefServiceSub = Substitute.For<IRemotingRefService>();
        var repository1 = Substitute.For<IDynamicRepository>();
        repository1.Fetch<string>(Arg.Any<CheckCustomerCriteria>()).Returns(new List<string>());
        var checkCustomerHandlerSub1 = Substitute.For<CheckCustomerHandler>(repository1);
        validator = new AddSabaWalletTicketValidator(remotingRefServiceSub, checkCustomerHandlerSub1);
    }

    [Fact]
    public void Validate_WhenProgramIdIsZero_ReturnsError()
    {
        // Arrange
        var command = new AddSabaWalletTicketCommand(0, 1, false, []);

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorCode == "1" && e.ErrorMessage == "Invalid Program ID");
    }

    [Fact]
    public void Validate_WhenProgramIdIsNegative_ReturnsError()
    {
        // Arrange
        var command = new AddSabaWalletTicketCommand(-1, 1, false, []);

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorCode == "1" && e.ErrorMessage == "Invalid Program ID");
    }

    [Fact]
    public void Validate_WhenCustomersListIsEmpty_ReturnsError()
    {
        // Arrange
        var command = new AddSabaWalletTicketCommand(1, 1, false, []);

        // Act
        var result = validator.Validate(command);

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
        var result = validator.Validate(command);

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
        var result = validator.Validate(command);

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
        remotingRefServiceSub.GetResourceDataByRefIdAndResourceId(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
            .Returns("any");

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}