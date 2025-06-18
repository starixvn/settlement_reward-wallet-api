using Fanex.Caching;
using Fanex.Caching.Redis;
using Fanex.Data.Repository;
using Fanex.Data.SqlServer;
using Fanex.Logging;
using Fanex.Logging.Sentry.RavenClient;
using Fanex.RemotingRef;
using Fanex.Settlement.Converter.Services;
using ONEbook.UM.DependencyInjection;
using StackExchange.Redis;
using ILogger = Fanex.Logging.ILogger;

namespace RewardWallet.Api.Config;

public static class InitializeConfig
{
    public static IServiceCollection AddInitialServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOneBookUmForService(configuration);
        services.AddFanexLogSentry(configuration);
        services.AddFanexSql(configuration);
        services.AddConfigRedisCache(configuration);
        services.AddControllers();
        services.AddCoreServices(configuration);

        RefDataTypeContainer
            .ConnectTo(new Uri(configuration.GetSection("Fanex.RemotingRef.Service.Url").Value)).FromApplication("Settlement");
        services.AddSingleton<IRemotingRefService, RemotingRefService>();

        return services;
    }

    private static void AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddTransient<ILogger, Logger>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<IDynamicRepository, DynamicRepository>();
        services.AddTransient<XmlConvertService, XmlConvertServiceImp>();

        services.Configure<IConfiguration>(configuration);
    }

    private static void AddConfigRedisCache(this IServiceCollection service, IConfiguration configuration)
    {
        var redisConfig = configuration.GetSection("AuthSettings:RedisConfig").Value;
        var applicationName = configuration.GetSection("AuthSettings:ChannelPrefix").Value;
        service.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect($"{redisConfig},ChannelPrefix={applicationName}"));
        service.AddSingleton<ICache>(new RedisCache(new RedisCacheOptions
        {
            ConnectionString = redisConfig,
            ApplicationName = applicationName,
        }));

        service.AddSingleton<ICacheService>(new CacheService());
    }
}