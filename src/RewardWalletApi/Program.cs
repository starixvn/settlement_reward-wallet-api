using FluentValidation;
using FluentValidation.AspNetCore;
using RewardWallet;
using RewardWallet.Models;
using RewardWalletApi.Config;
using RewardWalletApi.Middleware;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    .AddRewardWalletModule();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health");

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowOrigin");

app.UseAuthorization();

app.MapRewardWalletModule();

await app.RunAsync();