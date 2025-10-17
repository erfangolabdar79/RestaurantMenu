# مرحله build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "./PonehRestaurantMenu.csproj"
RUN dotnet publish "./PonehRestaurantMenu.csproj" -c Release -o /app/publish

# مرحله runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PonehRestaurantMenu.dll"]
