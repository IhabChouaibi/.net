FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Backend/AuthService/AuthService.csproj", "Backend/AuthService/"]
RUN dotnet restore "Backend/AuthService/AuthService.csproj"
COPY . .
WORKDIR /src/Backend/AuthService
RUN dotnet publish "AuthService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AuthService.dll"]
