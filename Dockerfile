# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY HermanosDeLeche.sln ./
COPY HermanosDeLeche.Api/HermanosDeLeche.Api.csproj HermanosDeLeche.Api/
COPY HermanosDeLeche.Domain/HermanosDeLeche.Domain.csproj HermanosDeLeche.Domain/
COPY HermanosDeLeche.Service/HermanosDeLeche.Service.csproj HermanosDeLeche.Service/

RUN dotnet restore HermanosDeLeche.sln

COPY HermanosDeLeche.Api/ HermanosDeLeche.Api/
COPY HermanosDeLeche.Domain/ HermanosDeLeche.Domain/
COPY HermanosDeLeche.Service/ HermanosDeLeche.Service/

RUN dotnet publish HermanosDeLeche.Api/HermanosDeLeche.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "HermanosDeLeche.Api.dll"]
