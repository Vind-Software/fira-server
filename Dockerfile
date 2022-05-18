FROM mcr.microsoft.com/dotnet/sdk:6.0 AS fira-server-webserver-build-env
WORKDIR /app

COPY fira-server.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -o build

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
COPY --from=fira-server-webserver-build-env /app/build .
ENTRYPOINT [ "dotnet", "fira-server.dll" ]