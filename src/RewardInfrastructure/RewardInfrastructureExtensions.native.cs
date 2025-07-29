using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RewardInfrastructure.Features;
using RewardInfrastructure.Services;
using RewardInfrastructure.Services.ProgramMessage;

namespace RewardInfrastructure;

public static class RewardInfrastructureExtensions
{
    public static WebApplicationBuilder AddRewardInfrastructureModule(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<UploadBatchMessageService, UploadBatchMessageServiceImp>()
            .AddScoped<InsertMessageService, InsertMessageServiceImp>()
            .AddScoped<MessageTemplateProviderFactory, MessageTemplateProviderFactoryImp>()
            .AddScoped<IMessageTemplateProvider, DefaultTemplateProvider>()
            .AddScoped<IMessageTemplateProvider, ProgramId3TemplateProvider>();

        return builder;
    }

    public static void AddRewardInfrastructureConsumers(this IRegistrationConfigurator configurator)
    {
        configurator.AddConsumer<RewardWalletEventsConsumers>();
    }
}