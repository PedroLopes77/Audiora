FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/Audiora.API/Audiora.API.csproj", "src/Audiora.API/"]
COPY ["src/Audiora.Application/Audiora.Application.csproj", "src/Audiora.Application/"]
COPY ["src/Audiora.Domain/Audiora.Domain.csproj", "src/Audiora.Domain/"]
COPY ["src/Audiora.Infrastructure/Audiora.Infrastructure.csproj", "src/Audiora.Infrastructure/"]

RUN dotnet restore "src/Audiora.API/Audiora.API.csproj"

COPY . .
WORKDIR "/src/src/Audiora.API"
RUN dotnet build "Audiora.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Audiora.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN mkdir -p Uploads/Audio Uploads/Images logs

ENTRYPOINT ["dotnet", "Audiora.API.dll"]