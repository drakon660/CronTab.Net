FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CronTab.Net.Web/CronTab.Net.Web.csproj", "CronTab.Net.Web/"]
RUN dotnet restore "CronTab.Net.Web/CronTab.Net.Web.csproj"
COPY . .
WORKDIR "/src/CronTab.Net.Web"
RUN dotnet build "CronTab.Net.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CronTab.Net.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CronTab.Net.Web.dll"]
