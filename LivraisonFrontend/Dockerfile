FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["LivraisonFrontend/LivraisonFrontend.csproj", "LivraisonFrontend/"]
RUN dotnet restore "LivraisonFrontend/LivraisonFrontend.csproj"
COPY . .
WORKDIR /src/LivraisonFrontend
RUN dotnet publish "LivraisonFrontend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LivraisonFrontend.dll"]
