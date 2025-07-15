FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY TaskManagerAPI/ ./TaskManagerAPI/
COPY TaskManager.Data/ ./TaskManager.Data/
COPY TaskManager.Models/ ./TaskManager.Models/
COPY TaskManager.sln ./

RUN dotnet restore TaskManagerAPI/TaskManagerAPI.csproj
RUN dotnet publish TaskManagerAPI/TaskManagerAPI.csproj -c Release -o /src/out

FROM mcr.microsoft.com/dotnet/sdk:9.0
WORKDIR /app

COPY --from=build /src/out ./out

COPY TaskManagerAPI/ ./TaskManagerAPI/
COPY TaskManager.Data/ ./TaskManager.Data/
COPY TaskManager.Models/ ./TaskManager.Models/
COPY TaskManager.sln ./

RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

EXPOSE 80

ENTRYPOINT ["sh", "-c", "dotnet ef database update --project /app/TaskManagerAPI/TaskManagerAPI.csproj && dotnet ./out/TaskManagerAPI.dll"]
