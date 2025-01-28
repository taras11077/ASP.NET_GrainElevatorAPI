# Етап 1: Використовуємо .NET SDK для побудови
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копіюємо проєкти для відновлення залежностей
COPY ["GrainElevatorAPI/GrainElevatorAPI.csproj", "GrainElevatorAPI/"]
COPY ["GrainElevatorAPI.Core/GrainElevatorAPI.Core.csproj", "GrainElevatorAPI.Core/"]
COPY ["GrainElevatorAPI.Storage/GrainElevatorAPI.Storage.csproj", "GrainElevatorAPI.Storage/"]

# Відновлюємо залежності
RUN dotnet restore "GrainElevatorAPI/GrainElevatorAPI.csproj"

# Копіюємо решту коду
COPY . .

# Виконуємо збірку
WORKDIR "/src/GrainElevatorAPI"
RUN dotnet build "GrainElevatorAPI.csproj" -c Release -o /app/build

# Виконуємо публікацію
RUN dotnet publish "GrainElevatorAPI.csproj" -c Release -o /app/publish

# Етап 2: Використовуємо ASP.NET рантайм для виконання
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Копіюємо публікацію з попереднього етапу
COPY --from=build /app/publish .

# Налаштовуємо порт
EXPOSE 5012

# Запускаємо додаток
ENTRYPOINT ["dotnet", "GrainElevatorAPI.dll"]




# # См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# # Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# USER app
# WORKDIR /app
# EXPOSE 8080
# EXPOSE 8081


# # Этот этап используется для сборки проекта службы
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# ARG BUILD_CONFIGURATION=Release
# WORKDIR /src
# COPY ["GrainElevatorAPI/GrainElevatorAPI.csproj", "GrainElevatorAPI/"]
# COPY ["GrainElevatorAPI.Core/GrainElevatorAPI.Core.csproj", "GrainElevatorAPI.Core/"]
# COPY ["GrainElevatorAPI.Storage/GrainElevatorAPI.Storage.csproj", "GrainElevatorAPI.Storage/"]
# RUN dotnet restore "./GrainElevatorAPI/GrainElevatorAPI.csproj"
# COPY . .
# WORKDIR "/src/GrainElevatorAPI"
# RUN dotnet build "./GrainElevatorAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# # Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
# FROM build AS publish
# ARG BUILD_CONFIGURATION=Release
# RUN dotnet publish "./GrainElevatorAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# # Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "GrainElevatorAPI.dll"]