# Використовуємо офіційний образ .NET SDK для побудови та виконання
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копіюємо проєкт і відновлюємо залежності
COPY *.csproj .
RUN dotnet restore

# Копіюємо весь код і будуємо додаток
COPY . .
RUN dotnet publish -c Release -o out

# Використовуємо легкий runtime образ для запуску
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "GrainElevatorBackend.dll"]
