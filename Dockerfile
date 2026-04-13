# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy all project files
COPY QuantityMeasurementAPI/ ./QuantityMeasurementAPI/
COPY QuantityMeasurementAppBusinessLayer/ ./QuantityMeasurementAppBusinessLayer/
COPY QuantityMeasurementAppRepositoryLayer/ ./QuantityMeasurementAppRepositoryLayer/
COPY QuantityMeasurementAppModel/ ./QuantityMeasurementAppModel/

# Restore and publish
WORKDIR /app/QuantityMeasurementAPI
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/QuantityMeasurementAPI/out ./

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "QuantityMeasurementAPI.dll"]