FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Backend/PaiementService/PaiementService.csproj", "Backend/PaiementService/"]
RUN dotnet restore "Backend/PaiementService/PaiementService.csproj"
COPY . .
WORKDIR /src/Backend/PaiementService
RUN dotnet publish "PaiementService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PaiementService.dll"]
