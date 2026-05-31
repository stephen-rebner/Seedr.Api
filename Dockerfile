FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Seedr.Api/Seedr.Api.csproj", "src/Seedr.Api/"]

RUN dotnet restore "src/Seedr.Api/Seedr.Api.csproj"

COPY . .

WORKDIR "/src/src/Seedr.Api"
RUN dotnet build "Seedr.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Seedr.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Seedr.Api.dll"]
