#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_URLS http://+:80


FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim AS build
WORKDIR /src
COPY ["Imitari/Imitari.csproj", "Imitari/"]
COPY ["Imitari.Apis/Imitari.Apis.csproj", "Imitari.Apis/"]
RUN dotnet restore "Imitari/Imitari.csproj"
COPY . .
WORKDIR "/src/Imitari"
RUN dotnet build "Imitari.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Imitari.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Imitari.dll"]