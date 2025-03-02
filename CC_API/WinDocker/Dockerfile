# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
# For more information, please see https://aka.ms/containercompat

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 20010
EXPOSE 20011


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0-nanoserver-1809 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["CC_API/CC_API.csproj", "CC_API/"]
COPY ["CC_Infrastructure/CC_Infrastructure.csproj", "CC_Infrastructure/"]
COPY ["CC_Model/CC_Model.csproj", "CC_Model/"]
COPY ["CC_Services/CC_Services.csproj", "CC_Services/"]
COPY ["CC_Util/CC_Util.csproj", "CC_Util/"]
RUN dotnet restore "./CC_API/CC_API.csproj"
COPY . .
WORKDIR "/src/CC_API"
RUN dotnet build "./CC_API.csproj" -c %BUILD_CONFIGURATION% -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CC_API.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CC_API.dll"]