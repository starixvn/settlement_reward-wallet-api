FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder

WORKDIR /src

COPY src/Core/*.csproj ./src/Core/
COPY src/RewardWallet/*.csproj ./src/RewardWallet/
COPY src/RewardWalletApi/*.csproj ./src/RewardWalletApi/


RUN dotnet nuget add source https://nuget.nexdev.net/api/v2
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore ./src/RewardWalletApi/RewardWalletApi.csproj

COPY src/Core/. ./src/Core/
COPY src/RewardWallet/. ./src/RewardWallet/
COPY src/RewardWalletApi/. ./src/RewardWalletApi/

WORKDIR /src/src/RewardWalletApi

RUN --mount=type=cache,target=/root/.nuget/packages \
	dotnet publish -c release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine AS base

ENV COMPlus_EnableDiagnostics=0
WORKDIR /app
COPY --from=builder /app .


# Don’t run as root
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app
USER dotnetuser

ENTRYPOINT ["dotnet", "RewardWalletApi.dll"]