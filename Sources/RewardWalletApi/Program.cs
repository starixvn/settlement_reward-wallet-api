using MinimalApi.Endpoint.Extensions;
using RewardWallet.Models;
using RewardWalletApi.Config;
using RewardWalletApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpoints();

builder.Services.AddHealthChecks();

builder.Services.Configure<AppSettings>(builder.Configuration);

builder.Services.AddInitialServices(builder.Configuration);

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

app.MapEndpoints();

app.Run();