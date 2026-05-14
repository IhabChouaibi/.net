FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Backend/DashboardService/DashboardService.csproj", "Backend/DashboardService/"]
RUN dotnet restore "Backend/DashboardService/DashboardService.csproj"
COPY . .
WORKDIR /src/Backend/DashboardService
RUN dotnet publish "DashboardService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DashboardService.dll"]
