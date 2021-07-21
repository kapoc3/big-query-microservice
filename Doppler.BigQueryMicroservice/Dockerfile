#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Doppler.BigQueryMicroservice/Doppler.BigQueryMicroservice.csproj", "Doppler.BigQueryMicroservice/"]
RUN dotnet restore "Doppler.BigQueryMicroservice/Doppler.BigQueryMicroservice.csproj"
COPY . .
WORKDIR "/src/Doppler.BigQueryMicroservice"
RUN dotnet build "Doppler.BigQueryMicroservice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Doppler.BigQueryMicroservice.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Doppler.BigQueryMicroservice.dll"]
