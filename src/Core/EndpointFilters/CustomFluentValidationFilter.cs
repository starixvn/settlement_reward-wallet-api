using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Core.EndpointFilters;

public class CustomFluentValidationFilter : IEndpointFilter
{
    private readonly IServiceProvider serviceProvider;

    public CustomFluentValidationFilter(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var argument = context.Arguments.FirstOrDefault(arg => arg != null);

        if (argument == null)
        {
            return Results.BadRequest("Invalid model object");
        }

        var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
        var validator = serviceProvider.GetService(validatorType) as IValidator;

        if (validator == null)
        {
            return await next(context);
        }

        var ValidationContextType = typeof(ValidationContext<>).MakeGenericType(argument.GetType());
        var validationContext = Activator.CreateInstance(ValidationContextType, argument) as IValidationContext;
        var validationResult = await validator.ValidateAsync(validationContext);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new { e.ErrorCode, e.ErrorMessage });
            return Results.BadRequest(errors.First());
        }

        return await next(context);
    }
}


public static class CustomFluentValidationFilterExtensions
{
    public static RouteGroupBuilder AddCustomFluentValidationFilter(this RouteGroupBuilder builder)
    {
        builder.AddEndpointFilter<CustomFluentValidationFilter>();
        return builder;
    }
}