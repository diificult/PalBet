FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

COPY . ./
RUN dotnet publish PalBet.Docker.sln -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
EXPOSE 8080
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "PalBet.dll"]