# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 7212
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BayTack.API/BayTack.API.csproj", "BayTack.API/"]
COPY ["BayTack.Application/BayTack.Application.csproj", "BayTack.Application/"]
COPY ["BayTack.Contracts/BayTack.Contracts.csproj", "BayTack.Contracts/"]
COPY ["BayTack.Domain/BayTack.Domain.csproj", "BayTack.Domain/"]
COPY ["BayTack.Infrastructure/BayTack.Infrastructure.csproj", "BayTack.Infrastructure/"]
RUN dotnet restore "./BayTack.API/BayTack.API.csproj"
COPY . .
WORKDIR "/src/BayTack.API"
RUN dotnet build "./BayTack.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BayTack.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BayTack.API.dll"]