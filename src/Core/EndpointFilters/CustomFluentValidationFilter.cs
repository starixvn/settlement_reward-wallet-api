using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Core.EndpointFilters;

public class CustomFluentValidationFilter(IServiceProvider serviceProvider) : IEndpointFilter
{
    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var argument = context.Arguments.FirstOrDefault(arg => arg != null);

        if (argument == null)
        {
            return Results.BadRequest("Invalid model object");
        }

        var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());

        if (serviceProvider.GetService(validatorType) is not IValidator validator)
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