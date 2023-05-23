FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Crontab.Net.Api/Crontab.Net.Api.csproj", "Crontab.Net.Api/"]
RUN dotnet restore "Crontab.Net.Api/Crontab.Net.Api.csproj"
COPY . .
WORKDIR "/src/Crontab.Net.Api"
RUN dotnet build "Crontab.Net.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Crontab.Net.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Crontab.Net.Api.dll"]
