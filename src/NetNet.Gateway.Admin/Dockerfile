FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/NetNet.Gateway.Admin/NetNet.Gateway.Admin.csproj", "src/NetNet.Gateway.Admin/"]
COPY ["src/NetNet.Gateway.Application/NetNet.Gateway.Application.csproj", "src/NetNet.Gateway.Application/"]
COPY ["src/NetNet.Gateway.Yarp/NetNet.Gateway.Yarp.csproj", "src/NetNet.Gateway.Yarp/"]
COPY ["src/NetNet.Gateway.Application.Contracts/NetNet.Gateway.Application.Contracts.csproj", "src/NetNet.Gateway.Application.Contracts/"]
COPY ["src/NetNet.Gateway.Domain.Shared/NetNet.Gateway.Domain.Shared.csproj", "src/NetNet.Gateway.Domain.Shared/"]
COPY ["src/NetNet.Gateway.Domain/NetNet.Gateway.Domain.csproj", "src/NetNet.Gateway.Domain/"]
COPY ["src/NetNet.Gateway.Distributed/NetNet.Gateway.Distributed.csproj", "src/NetNet.Gateway.Distributed/"]
COPY ["src/NetNet.Gateway.EntityFrameworkCore/NetNet.Gateway.EntityFrameworkCore.csproj", "src/NetNet.Gateway.EntityFrameworkCore/"]
COPY ["src/NetNet.Gateway.SwaggerUI.Blazor/NetNet.Gateway.SwaggerUI.Blazor.csproj", "src/NetNet.Gateway.SwaggerUI.Blazor/"]
COPY ["src/NetNet.Gateway.BuildingBlock/NetNet.Gateway.BuildingBlock.csproj", "src/NetNet.Gateway.BuildingBlock/"]
COPY ["src/NetNet.Gateway.Swagger/NetNet.Gateway.Swagger.csproj", "src/NetNet.Gateway.Swagger/"]
RUN dotnet restore "src/NetNet.Gateway.Admin/NetNet.Gateway.Admin.csproj"
COPY . .
WORKDIR "/src/src/NetNet.Gateway.Admin"
RUN dotnet build "NetNet.Gateway.Admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NetNet.Gateway.Admin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetNet.Gateway.Admin.dll"]
