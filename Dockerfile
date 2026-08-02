# ---------- Etapa 1: build ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia apenas o csproj primeiro (aproveita cache do Docker nas próximas builds)
COPY OS_API/OS_API.csproj OS_API/
RUN dotnet restore OS_API/OS_API.csproj

# Copia o restante do código e publica
COPY OS_API/ OS_API/
WORKDIR /src/OS_API
RUN dotnet publish -c Release -o /app/publish --no-restore

# ---------- Etapa 2: runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Usuário não-root (boa prática de segurança)
RUN adduser --disabled-password --home /app appuser \
    && chown -R appuser /app
USER appuser

COPY --from=build /app/publish .

# Render define a porta via variável de ambiente PORT (padrão 10000).
# O ENTRYPOINT abaixo garante que o Kestrel escute exatamente nessa porta.
ENV DOTNET_RUNNING_IN_CONTAINER=true
EXPOSE 10000
ENTRYPOINT ["sh", "-c", "ASPNETCORE_URLS=http://+:${PORT:-10000} dotnet OS_API.dll"]
