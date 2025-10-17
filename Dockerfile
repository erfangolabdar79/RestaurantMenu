# مرحله Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore "./PonehRestaurantMenu.csproj"
RUN dotnet publish "./PonehRestaurantMenu.csproj" -c Release -o /app/publish

# مرحله Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# پورت پیش‌فرض
EXPOSE 8080

ENTRYPOINT ["dotnet", "PonehRestaurantMenu.dll"]
