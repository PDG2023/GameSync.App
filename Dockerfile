#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# all excepts the GameSync.Front and the tests one folder 
# GameSync.Front does not have a cs proj and tests are already ignored with dockerignore
COPY GameSync.*/*.csproj ./

# reproduce the directory hierarchy according to the source folder
# taken from : https://andrewlock.net/optimising-asp-net-core-apps-in-docker-avoiding-manually-copying-csproj-files-part-2/
RUN for file in $(ls *.csproj); do mkdir -p ./${file%.*}/ && mv $file ./${file%.*}/; done

# Restore csprojs
RUN for file in $(ls **/*.csproj); do dotnet restore ${file}; done

# All except GameSync.Front
# Must be manually copied to keep the same directory structures ...
COPY ./GameSync.Api/ ./GameSync.Api/
COPY ./GameSync.Api.Persistence/ ./GameSync.Api.Persistence/
COPY ./GameSync.Business/ ./GameSync.Business/

RUN dotnet build ./GameSync.Api/GameSync.Api.csproj -c Release --no-restore

FROM build AS publish
RUN dotnet publish GameSync.Api/GameSync.Api.csproj -c Release -o /app/publish /p:UseAppHost=false --no-build

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