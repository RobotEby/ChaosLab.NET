FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG PROJECT
WORKDIR /src
COPY Directory.Build.props .
COPY src/ src/
RUN dotnet publish src/${PROJECT}/${PROJECT}.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
ARG PROJECT
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://+:8080 APP_DLL=${PROJECT}.dll
ENTRYPOINT ["sh", "-c", "exec dotnet $APP_DLL"]
