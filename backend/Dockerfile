# https://hub.docker.com/_/microsoft-dotnet-sdk/
# Debian 10 based
# FROM mcr.microsoft.com/dotnet/sdk:latest AS build
# Ubuntu 20.04
# FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
FROM mcr.microsoft.com/dotnet/sdk:9.0-noble AS build

WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Directory.Build.props .

# base projects
COPY Base.Domain/*.csproj ./Base.Domain/
COPY Base.Helpers/*.csproj ./Base.Helpers/
COPY Base.Resources/*.csproj ./Base.Resources/
COPY App.BLL/*.csproj ./App.BLL/
COPY App.DTO/*.csproj ./App.DTO/
COPY App.Resources/*.csproj ./App.Resources/
COPY App.DAL.EF/*.csproj ./App.DAL.EF/
COPY App.Domain/*.csproj ./App.Domain/
COPY WebApp/*.csproj ./WebApp/

RUN dotnet restore


# copy everything else and build app
# base projects
COPY Base.Domain/. ./Base.Domain/
COPY Base.Helpers/. ./Base.Helpers/
COPY Base.Resources/. ./Base.Resources/
COPY App.BLL/. ./App.BLL/
COPY App.DTO/. ./App.DTO/
COPY App.Resources/. ./App.Resources/
COPY App.DAL.EF/. ./App.DAL.EF/
COPY App.Domain/. ./App.Domain/
COPY WebApp/. ./WebApp/

# build and publish WebApp
WORKDIR /source/WebApp
RUN dotnet publish -c release -o /app --no-restore


# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:9.0-noble

EXPOSE 8080

# set timezone
# https://serverfault.com/questions/683605/docker-container-time-timezone-will-not-reflect-changes
ENV TZ 'Europe/Tallinn'
RUN echo $TZ > /etc/timezone && \
    apt-get update && apt-get install -y libgdiplus libc6-dev tzdata && \
    rm /etc/localtime && \
    ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && \
    dpkg-reconfigure -f noninteractive tzdata && \
    apt-get clean

WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "WebApp.dll"]
