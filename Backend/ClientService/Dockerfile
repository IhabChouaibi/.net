FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Backend/ClientService/ClientService.csproj", "Backend/ClientService/"]
RUN dotnet restore "Backend/ClientService/ClientService.csproj"
COPY . .
WORKDIR /src/Backend/ClientService
RUN dotnet publish "ClientService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ClientService.dll"]
