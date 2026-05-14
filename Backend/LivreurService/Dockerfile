FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Backend/LivreurService/LivreurService.csproj", "Backend/LivreurService/"]
RUN dotnet restore "Backend/LivreurService/LivreurService.csproj"
COPY . .
WORKDIR /src/Backend/LivreurService
RUN dotnet publish "LivreurService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LivreurService.dll"]
