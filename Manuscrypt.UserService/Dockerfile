# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Manuscrypt.UserService/Manuscrypt.UserService.csproj", "Manuscrypt.UserService/"]
COPY ["../Manuscrypt.Shared/Manuscrypt.Shared.csproj", "../Manuscrypt.Shared/"]
RUN dotnet restore "Manuscrypt.UserService/Manuscrypt.UserService.csproj"
COPY . .
WORKDIR /src/Manuscrypt.UserService
RUN dotnet build "Manuscrypt.UserService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/Manuscrypt.UserService
RUN dotnet publish "Manuscrypt.UserService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Manuscrypt.UserService.dll"]