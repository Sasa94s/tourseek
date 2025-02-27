﻿FROM postgres:12
RUN apt-get update \
    && apt-get install wget -y \
    && apt-get install postgresql-12-postgis-3 -y \
    && apt-get install postgis -y
COPY ./extensions.sql /docker-entrypoint-initdb.d

FROM mcr.microsoft.com/dotnet/sdk:5.0.400-buster-slim AS base
WORKDIR /app
EXPOSE $PORT
ENV ASPNETCORE_URLS=http://*:$PORT

FROM base as build
WORKDIR /src
COPY tourseek-backend.sln ./
COPY tourseek_backend.api/*.csproj ./tourseek_backend.api/
COPY tourseek_backend.domain/*.csproj ./tourseek_backend.domain/
COPY tourseek_backend.repository/*.csproj ./tourseek_backend.repository/
COPY tourseek_backend.services/*.csproj ./tourseek_backend.services/
COPY tourseek_backend.util/*.csproj ./tourseek_backend.util/

RUN dotnet restore "tourseek-backend.sln"

COPY . .

FROM build AS testing
WORKDIR /src/tourseek_backend.api
WORKDIR /src/tourseek_backend.domain
WORKDIR /src/tourseek_backend.repository
WORKDIR /src/tourseek_backend.services
WORKDIR /src/tourseek_backend.util

RUN dotnet build

FROM testing AS publish
WORKDIR /src/tourseek_backend.api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0.9-buster-slim AS runtime
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "tourseek_backend.api.dll", "--launch-profile", "Docker"]