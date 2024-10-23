using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using RewardInfrastructure.Features;
using RewardInfrastructure.Features.AddMsg;
using RewardInfrastructure.Features.AddTargetMsg;

namespace RewardInfrastructure
{
    public static class RewardInfrastructureExtensions
    {
        public static WebApplicationBuilder AddRewardInfrastructureModule(this WebApplicationBuilder builder)
        {
            builder.Services
                .AddScoped<AddTargetMsgHandler>()
                .AddScoped<AddMsgHandler>();

            return builder;
        }

        public static IEndpointRouteBuilder MapRewardInfrastructureModule(this IEndpointRouteBuilder endpoints)
        {
            return endpoints;
        }

        public static void AddRewardInfrastructureConsumers(this IRegistrationConfigurator configurator)
        {
            configurator.AddConsumer<RewardWalletEventsConsumers>();
        }
    }
}