#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["JsonObjectTest/JsonObjectTest.csproj", "JsonObjectTest/"]
RUN dotnet restore "JsonObjectTest/JsonObjectTest.csproj"
COPY . .
WORKDIR "/src/JsonObjectTest"
RUN dotnet build "JsonObjectTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JsonObjectTest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JsonObjectTest.dll"]