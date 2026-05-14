FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Backend/GatewayService/GatewayService.csproj", "Backend/GatewayService/"]
RUN dotnet restore "Backend/GatewayService/GatewayService.csproj"
COPY . .
WORKDIR /src/Backend/GatewayService
RUN dotnet publish "GatewayService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GatewayService.dll"]
