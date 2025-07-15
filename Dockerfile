# Usar a imagem base do .NET SDK para build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiar os arquivos do projeto
COPY . ./

# Restaurar depend�ncias
RUN dotnet restore

# Compilar o projeto
RUN dotnet publish -c Release -o out

# Usar a imagem base do .NET Runtime para execu��o
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copiar os arquivos publicados
COPY --from=build /app/out .

# Expor a porta padr�o do ASP.NET Core
EXPOSE 80

# Comando para iniciar o aplicativo
ENTRYPOINT ["dotnet", "TaskManagerAPI.dll"]
