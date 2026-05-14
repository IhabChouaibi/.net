FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Backend/ColisService/ColisService.csproj", "Backend/ColisService/"]
RUN dotnet restore "Backend/ColisService/ColisService.csproj"
COPY . .
WORKDIR /src/Backend/ColisService
RUN dotnet publish "ColisService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ColisService.dll"]
