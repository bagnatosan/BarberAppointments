# =========================================================
# ETAPA 1: COMPILACIÓN (Usamos el SDK pesado)
# =========================================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /src

# 1. Copiar el archivo del proyecto y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# 2. Copiar todo el resto del código y compilarlo en modo Release
COPY . ./
RUN dotnet publish -c Release -o /app/out

# =========================================================
# ETAPA 2: PRODUCCIÓN (Usamos el Runtime liviano)
# =========================================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# 3. Traernos solo las DLLs cocinadas de la etapa anterior
COPY --from=build-env /app/out .

# 4. Configurar el puerto y el arranque
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "CaporaleBarberia.dll"]
