using Core.EndpointFilters;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Moq;

namespace RewardWallet.Tests.Core.EndpointFilters;

public class CustomFluentValidationFilterTests
{
    private readonly Mock<IServiceProvider> _serviceProviderMock;
    private readonly Mock<EndpointFilterDelegate> _nextMock;
    private readonly CustomFluentValidationFilter _filter;

    public CustomFluentValidationFilterTests()
    {
        _serviceProviderMock = new Mock<IServiceProvider>();
        _nextMock = new Mock<EndpointFilterDelegate>();
        _filter = new CustomFluentValidationFilter(_serviceProviderMock.Object);
    }

    [Fact]
    public async Task InvokeAsync_WhenArgumentIsNull_ReturnsBadRequest()
    {
        // Arrange
        var context = CreateEndpointFilterContext(null);

        // Act
        var result = await _filter.InvokeAsync(context, _nextMock.Object);

        // Assert
        Assert.NotNull(result);
        var badRequestResult = Assert.IsType<object>(result);
        Assert.Equal("Invalid model object", badRequestResult.ToString());
    }

    [Fact]
    public async Task InvokeAsync_WhenNoValidatorFound_ProceedsToNext()
    {
        // Arrange
        var model = new TestModel();
        var context = CreateEndpointFilterContext(model);
        _serviceProviderMock.Setup(x => x.GetService(It.IsAny<Type>()))
            .Returns(null);

        // Act
        await _filter.InvokeAsync(context, _nextMock.Object);

        // Assert
        _nextMock.Verify(x => x(context), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_WhenValidationSucceeds_ProceedsToNext()
    {
        // Arrange
        var model = new TestModel();
        var context = CreateEndpointFilterContext(model);
        var validator = new Mock<IValidator>();
        validator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _serviceProviderMock.Setup(x => x.GetService(It.IsAny<Type>()))
            .Returns(validator.Object);

        // Act
        await _filter.InvokeAsync(context, _nextMock.Object);

        // Assert
        _nextMock.Verify(x => x(context), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_WhenValidationFails_ReturnsBadRequest()
    {
        // Arrange
        var model = new TestModel();
        var context = CreateEndpointFilterContext(model);
        var validator = new Mock<IValidator>();
        validator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("Property", "Error message", "1") }));

        _serviceProviderMock.Setup(x => x.GetService(It.IsAny<Type>()))
            .Returns(validator.Object);

        // Act
        var result = await _filter.InvokeAsync(context, _nextMock.Object);

        // Assert
        Assert.NotNull(result);
        var error = result.ToString();
        Assert.Contains("Error message", error);
    }

    private static EndpointFilterInvocationContext CreateEndpointFilterContext(object argument)
    {
        var context = new Mock<EndpointFilterInvocationContext>();
        context.Setup(x => x.Arguments).Returns(new[] { argument });
        return context.Object;
    }

    private class TestModel
    {
        public string Property { get; set; }
    }
}