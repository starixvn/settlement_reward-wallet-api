using System.Reflection;
using Core.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using RewardInfrastructure;
using RewardWallet;
using RewardWallet.Api.Config;
using RewardWallet.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "RewardWallet.Api", Version = "v1" });
});

builder.AddFluentValidationEndpointFilter();
builder.Services
    .AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblies([
        Assembly.GetExecutingAssembly(),
        Assembly.GetAssembly(typeof(RewardWalletModuleExtensions))
    ]);

builder.Services.AddHealthChecks();

builder.Services.Configure<AppSettings>(builder.Configuration);

builder.Services.AddInitialServices(builder.Configuration);

builder
    .AddRewardWalletModule()
    .AddRewardInfrastructureModule();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
    x.AddRewardInfrastructureConsumers();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RewardWallet API V1");
});

app.MapHealthChecks("/health");

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowOrigin");

app.UseAuthorization();

app.MapRewardWalletModule();

await app.RunAsync();