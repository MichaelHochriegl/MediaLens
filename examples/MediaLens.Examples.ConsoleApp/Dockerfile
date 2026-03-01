FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["examples/MediaLens.Examples.ConsoleApp/MediaLens.Examples.ConsoleApp.csproj", "examples/MediaLens.Examples.ConsoleApp/"]
RUN dotnet restore "examples/MediaLens.Examples.ConsoleApp/MediaLens.Examples.ConsoleApp.csproj"
COPY . .
WORKDIR "/src/examples/MediaLens.Examples.ConsoleApp"
RUN dotnet build "./MediaLens.Examples.ConsoleApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MediaLens.Examples.ConsoleApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MediaLens.Examples.ConsoleApp.dll"]
