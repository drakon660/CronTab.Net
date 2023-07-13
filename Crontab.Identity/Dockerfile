FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Crontab.Identity/Crontab.Identity.csproj", "Crontab.Identity/"]
RUN dotnet restore "Crontab.Identity/Crontab.Identity.csproj"
COPY . .
WORKDIR "/src/Crontab.Identity"
RUN dotnet build "Crontab.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Crontab.Identity.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Crontab.Identity.dll"]
