﻿FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/NetNet.Gateway.Ingress/NetNet.Gateway.Ingress.csproj", "NetNet.Gateway.Ingress/"]
COPY ["src/NetNet.Gateway.Application/NetNet.Gateway.Application.csproj", "NetNet.Gateway.Application/"]
COPY ["src/NetNet.Gateway.Application.Contracts/NetNet.Gateway.Application.Contracts.csproj", "NetNet.Gateway.Application.Contracts/"]
COPY ["src/NetNet.Gateway.Domain.Shared/NetNet.Gateway.Domain.Shared.csproj", "NetNet.Gateway.Domain.Shared/"]
COPY ["src/NetNet.Gateway.Domain/NetNet.Gateway.Domain.csproj", "NetNet.Gateway.Domain/"]
COPY ["src/NetNet.Gateway.EntityFrameworkCore/NetNet.Gateway.EntityFrameworkCore.csproj", "NetNet.Gateway.EntityFrameworkCore/"]
RUN dotnet restore "src/NetNet.Gateway.Ingress/NetNet.Gateway.Ingress.csproj"
COPY . .
WORKDIR "/src/NetNet.Gateway.Ingress"
RUN dotnet build "NetNet.Gateway.Ingress.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NetNet.Gateway.Ingress.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetNet.Gateway.Ingress.dll"]