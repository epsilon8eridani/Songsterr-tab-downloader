FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TabDownloader/TabDownloader.csproj", "TabDownloader/"]
RUN dotnet restore "TabDownloader/TabDownloader.csproj"
COPY . .
WORKDIR "/src/TabDownloader"
RUN dotnet build "TabDownloader.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TabDownloader.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TabDownloader.dll"]
