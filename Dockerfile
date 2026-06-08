FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Seedr.Api.Service.Api/Seedr.Api.Service.Api.csproj", "src/Seedr.Api.Service.Api/"]
COPY ["src/Seedr.Api.Infrastructure/Seedr.Api.Infrastructure.csproj", "src/Seedr.Api.Infrastructure/"]
COPY ["src/Seedr.Api.Application/Seedr.Api.Application.csproj", "src/Seedr.Api.Application/"]
COPY ["src/Seedr.Api.Core/Seedr.Api.Core.csproj", "src/Seedr.Api.Core/"]

RUN dotnet restore "src/Seedr.Api.Service.Api/Seedr.Api.Service.Api.csproj"

COPY . .

WORKDIR "/src/src/Seedr.Api.Service.Api"
RUN dotnet build "Seedr.Api.Service.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Seedr.Api.Service.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Seedr.Api.Service.Api.dll"]
