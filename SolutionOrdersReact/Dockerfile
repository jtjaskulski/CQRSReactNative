FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
#EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Instalacja Node.js dla projektu React
RUN curl -fsSL https://deb.nodesource.com/setup_22.x | bash -
RUN apt-get install -y nodejs

# Kopiowanie plików projektu Server
COPY ["SolutionOrdersReact.Server/SolutionOrdersReact.Server.csproj", "SolutionOrdersReact.Server/"]
# Kopiowanie plików projektu Client (esproj)
COPY ["solutionordersreact.client/solutionordersreact.client.esproj", "solutionordersreact.client/"]
COPY ["solutionordersreact.client/package.json", "solutionordersreact.client/"]
COPY ["solutionordersreact.client/package-lock.json", "solutionordersreact.client/"]

# Restore projektu Server
RUN dotnet restore "SolutionOrdersReact.Server/SolutionOrdersReact.Server.csproj"

# Kopiowanie całego kodu
COPY . .

# Instalacja zależności npm dla frontendu
WORKDIR "/src/solutionordersreact.client"
RUN npm install

# Build projektu Server
WORKDIR "/src/SolutionOrdersReact.Server"
RUN dotnet build "SolutionOrdersReact.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/SolutionOrdersReact.Server"
RUN dotnet publish "SolutionOrdersReact.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SolutionOrdersReact.Server.dll"]