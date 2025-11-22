FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["VersionManager/VersionManager.csproj", "VersionManager/"]
RUN dotnet restore "VersionManager/VersionManager.csproj"
COPY . .
WORKDIR "/src/VersionManager"
RUN dotnet build "VersionManager.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "VersionManager.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
RUN mkdir -p db
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VersionManager.dll"]
