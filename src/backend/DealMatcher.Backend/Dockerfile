FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /backend

COPY *.sln .
COPY Directory.Build.props ./
COPY Directory.Packages.props ./
COPY src/DealMatcher.Backend.Web/*.csproj src/DealMatcher.Backend.Web/
RUN dotnet restore src/DealMatcher.Backend.Web/DealMatcher.Backend.Web.csproj

COPY src/ src/

WORKDIR /backend/src/DealMatcher.Backend.Web


RUN dotnet publish DealMatcher.Backend.Web.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:90
EXPOSE 90

COPY --from=build /app/publish ./

ENTRYPOINT ["dotnet", "DealMatcher.Backend.Web.dll"]
