FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Backend/VehiculeService/VehiculeService.csproj", "Backend/VehiculeService/"]
RUN dotnet restore "Backend/VehiculeService/VehiculeService.csproj"
COPY . .
WORKDIR /src/Backend/VehiculeService
RUN dotnet publish "VehiculeService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "VehiculeService.dll"]
