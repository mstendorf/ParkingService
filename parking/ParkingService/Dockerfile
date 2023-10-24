FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
# COPY ["ShoppingCart/ShoppingCart.csproj", "ShoppingCart/"]
COPY . .
RUN dotnet restore "ParkingService.csproj"
# COPY . .
# WORKDIR "/src/ShoppingCart"
RUN dotnet build "ParkingService.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "ParkingService.csproj" -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ParkingService.dll"]
