FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Crontab.Net.Terminal/Crontab.Net.Terminal.csproj", "Crontab.Net.Terminal/"]
RUN dotnet restore "Crontab.Net.Terminal/Crontab.Net.Terminal.csproj"
COPY . .
WORKDIR "/src/Crontab.Net.Terminal"
RUN dotnet build "Crontab.Net.Terminal.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Crontab.Net.Terminal.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Crontab.Net.Terminal.dll"]
