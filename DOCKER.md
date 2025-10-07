# 🐳 Docker Setup - MotoTrust

## 🚀 Início Rápido

### Opção 1: Tudo no Docker (Recomendado)
```bash
# Iniciar tudo
docker-compose up --build

# Ou no Windows
start-docker.bat
```

### Opção 2: Apenas PostgreSQL no Docker
```bash
# Iniciar PostgreSQL
docker-compose up -d postgres

# Executar aplicação localmente
dotnet run --project MotoTrust.API

# Ou no Windows
start-dev.bat
```

## 📋 URLs Disponíveis

- **API**: http://localhost:5000
- **Swagger**: http://localhost:5000/swagger
- **PostgreSQL**: localhost:5432

## 🛠️ Comandos Úteis

```bash
# Parar containers
docker-compose down

# Ver logs
docker-compose logs -f

# Rebuild completo
docker-compose up --build --force-recreate

# Parar e remover volumes (limpar dados)
docker-compose down -v

# Entrar no container PostgreSQL
docker exec -it mototrust-postgres psql -U postgres -d MotoTrust
```

## 🔧 Configurações

### PostgreSQL
- **Host**: localhost (ou postgres dentro do Docker)
- **Port**: 5432
- **Database**: MotoTrust
- **User**: postgres
- **Password**: postgres

### Aplicação
- **Port**: 5000 (HTTP) / 5001 (HTTPS)
- **Environment**: Development

## 🐛 Troubleshooting

### Porta já em uso
```bash
# Verificar o que está usando a porta
netstat -ano | findstr :5000
netstat -ano | findstr :5432

# Parar containers existentes
docker-compose down
```

### Problemas de permissão
```bash
# No Windows, executar como administrador
# No Linux/Mac, usar sudo se necessário
```

### Limpar tudo
```bash
# Parar e remover tudo
docker-compose down -v --remove-orphans

# Remover imagens
docker rmi $(docker images -q mototrust*)
```

## 📝 Notas

- O PostgreSQL será criado automaticamente
- As migrações do EF Core rodam automaticamente
- Os dados persistem entre reinicializações (volume `postgres_data`)
- Para desenvolvimento, use a Opção 2 para hot reload
