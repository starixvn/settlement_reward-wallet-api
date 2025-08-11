using Core.EndpointFilters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace RewardWallet.Features.Testing;

public static class TestingExtensions
{
    public static IServiceCollection AddTesting(this IServiceCollection services)
    {
        services
            .AddScoped<TestingHandler>();

        return services;
    }

    public static IEndpointRouteBuilder MapTesting(this IEndpointRouteBuilder endpoints)
    {
        _ = endpoints.MapPost(
            "/testing-cache",
            (TestingHandler handler,
                    CancellationToken cancellation)
                    => handler.HandleAsync(cancellation))
            .AddEndpointFilter<LogRequestFilter>();

        return endpoints;
    }
}