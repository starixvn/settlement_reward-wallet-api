using Core.EndpointFilters;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace RewardWallet.Tests.Core.EndpointFilters;

public class CustomFluentValidationFilterTests
{
    private readonly IServiceProvider serviceProviderSub;
    private readonly EndpointFilterDelegate nextSub;
    private readonly CustomFluentValidationFilter filter;

    public CustomFluentValidationFilterTests()
    {
        serviceProviderSub = Substitute.For<IServiceProvider>();
        nextSub = Substitute.For<EndpointFilterDelegate>();
        filter = new CustomFluentValidationFilter(serviceProviderSub);
    }

    [Fact]
    public async Task InvokeAsync_WhenArgumentIsNull_ReturnsBadRequest()
    {
        // Arrange
        var context = CreateEndpointFilterContext(null);

        // Act
        var result = await filter.InvokeAsync(context, nextSub);

        // Assert
        Assert.NotNull(result);
        Assert.StartsWith("BadRequest", result.GetType().Name);
        var valueProp = result.GetType().GetProperty("Value")?.GetValue(result, null);
        Assert.Equal("Invalid model object", valueProp);
    }

    [Fact]
    public async Task InvokeAsync_WhenNoValidatorFound_ProceedsToNext()
    {
        // Arrange
        var model = new TestModel();
        var context = CreateEndpointFilterContext(model);
        serviceProviderSub.GetService(Arg.Any<Type>()).Returns((object)null);

        // Act
        await filter.InvokeAsync(context, nextSub);

        // Assert
        await nextSub.Received(1).Invoke(context);
    }

    [Fact]
    public async Task InvokeAsync_WhenValidationSucceeds_ProceedsToNext()
    {
        // Arrange
        var model = new TestModel();
        var context = CreateEndpointFilterContext(model);
        var validator = Substitute.For<IValidator>();
        validator.ValidateAsync(Arg.Any<IValidationContext>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        serviceProviderSub.GetService(Arg.Any<Type>()).Returns(validator);

        // Act
        await filter.InvokeAsync(context, nextSub);

        // Assert
        await nextSub.Received(1).Invoke(context);
    }

    [Fact]
    public async Task InvokeAsync_WhenValidationFails_ReturnsBadRequest()
    {
        // Arrange
        var model = new TestModel();
        var context = CreateEndpointFilterContext(model);
        var validator = Substitute.For<IValidator>();
        validator.ValidateAsync(Arg.Any<IValidationContext>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new[] { new ValidationFailure("Property", "Error message", "1") }));

        serviceProviderSub.GetService(Arg.Any<Type>()).Returns(validator);

        // Act
        var result = await filter.InvokeAsync(context, nextSub);

        // Assert
        Assert.NotNull(result);
        Assert.StartsWith("BadRequest", result.GetType().Name);
        var valueProp = result.GetType().GetProperty("Value")?.GetValue(result, null);
        Assert.Contains("Error message", valueProp?.ToString());
    }

    private static EndpointFilterInvocationContext CreateEndpointFilterContext(object argument)
    {
        var context = Substitute.For<EndpointFilterInvocationContext>();
        context.Arguments.Returns(new[] { argument });
        return context;
    }

    private class TestModel
    {
    }
}