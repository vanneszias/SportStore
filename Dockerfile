# Use the official .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY SportStore.sln ./
COPY SportStore.Domain/*.csproj ./SportStore.Domain/
COPY SportStore.Application/*.csproj ./SportStore.Application/
COPY SportStore.Infrastructure/*.csproj ./SportStore.Infrastructure/
COPY SportStore.WebUI/*.csproj ./SportStore.WebUI/
RUN dotnet restore

# Copy everything else and build
COPY . .
WORKDIR /app/SportStore.WebUI
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/SportStore.WebUI/out .
ENTRYPOINT ["dotnet", "SportStore.WebUI.dll"] 