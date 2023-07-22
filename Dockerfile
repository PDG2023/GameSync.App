#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["GameSync.Api/GameSync.Api.csproj", "GameSync.Api/"]
RUN dotnet restore "GameSync.Api/GameSync.Api.csproj"
COPY . .
WORKDIR "/src/GameSync.Api"
RUN dotnet build "GameSync.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameSync.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM node:slim AS front-install
WORKDIR /src-front
COPY GameSync.Front/package*.json ./
RUN npm install

FROM front-install AS front-build
COPY ./GameSync.Front/ .
RUN npm run build

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=front-build /src-front/dist ./wwwroot/
ENTRYPOINT ["dotnet", "GameSync.Api.dll"]