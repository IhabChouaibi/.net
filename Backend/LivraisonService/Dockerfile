FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Backend/LivraisonService/LivraisonService.csproj", "Backend/LivraisonService/"]
RUN dotnet restore "Backend/LivraisonService/LivraisonService.csproj"
COPY . .
WORKDIR /src/Backend/LivraisonService
RUN dotnet publish "LivraisonService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LivraisonService.dll"]
