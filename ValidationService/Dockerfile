FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
# COPY ["ShoppingCart/ShoppingCart.csproj", "ShoppingCart/"]
COPY . .
RUN dotnet restore "ValidationService.csproj"
# COPY . .
# WORKDIR "/src/ShoppingCart"
RUN dotnet build "ValidationService.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "ValidationService.csproj" -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ValidationService.dll"]
