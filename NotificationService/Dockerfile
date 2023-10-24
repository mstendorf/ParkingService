FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
# COPY ["ShoppingCart/ShoppingCart.csproj", "ShoppingCart/"]
COPY . .
RUN dotnet restore "NotificationService/NotificationService.csproj"
RUN dotnet restore "NotificationPoller/NotificationPoller.csproj"
# COPY . .
WORKDIR "/src/NotificationService"
RUN dotnet build "NotificationService.csproj" -c Release -o /api/build
WORKDIR "/src/NotificationPoller"
RUN dotnet build "NotificationPoller.csproj" -c Release -o /consumer/build
FROM build AS publish
WORKDIR "/src/NotificationService"
RUN dotnet publish "NotificationService.csproj" -c Release -o /api/publish
WORKDIR "/src/NotificationPoller"
RUN dotnet publish "NotificationPoller.csproj" -c Release -o /consumer/publish
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
EXPOSE 80
COPY --from=publish /api/publish ./api
COPY --from=publish /consumer/publish ./consumer
ENTRYPOINT dotnet $STARTUPDLL
