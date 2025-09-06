# MotoTrust API

Sistema de locação de motocicletas. API REST que gerencia entregadores, motos e locações.

## Arquitetura

Tentei aplicar algumas boas práticas que aprendi ao longo do tempo:

- **CQRS** com MediatR (separação de comandos e consultas)
- **DDD** (Domain Driven Design) - pelo menos tentei seguir os conceitos
- **Repository Pattern** com Unit of Work
- **Entity Framework Core** com PostgreSQL
- **FluentValidation** para validações
- **AutoMapper** para mapear objetos
- **ILogger** para logs
- **Swagger** para documentar a API
- **MassTransit + RabbitMQ** para eventos assíncronos

## 📁 Estrutura do Projeto

```
MotoTrust/
├── MotoTrust.Domain/           # Camada de Domínio
│   ├── Common/                 # Classes base (EntityBase, ValueObject, DomainEvent)
│   ├── Entities/               # Entidades do domínio (DeliveryPerson, Motorcycle, Rental)
│   ├── ValueObjects/           # Objetos de valor (Money)
│   ├── Enums/                  # Enumerações (MotorcycleStatus, RentalStatus, LicenseType)
│   ├── Events/                 # Eventos de domínio
│   └── Interfaces/             # Contratos dos repositórios
├── MotoTrust.Application/      # Camada de Aplicação
│   ├── Commands/               # Comandos CQRS
│   ├── Queries/                # Consultas CQRS
│   ├── DTOs/                   # Data Transfer Objects
│   └── Validators/             # Validadores FluentValidation
├── MotoTrust.Infrastructure/   # Camada de Infraestrutura
│   ├── Data/                   # DbContext e configurações do EF
│   ├── Repositories/           # Implementações dos repositórios
│   └── Configurations/         # Configurações das entidades
├── MotoTrust.API/              # Camada de Apresentação
│   └── Controllers/            # Controllers da API
└── MotoTrust.Tests/            # Testes Unitários
```

## Tecnologias

- .NET 9.0
- ASP.NET Core Web API
- Entity Framework Core 9.0
- PostgreSQL
- MediatR 13.0
- FluentValidation 12.0
- AutoMapper 15.0
- Microsoft.Extensions.Logging 9.0
- MassTransit 8.1.3 (RabbitMQ)
- RabbitMQ 3-management
- Swagger/OpenAPI
- xUnit (para testes)
- FluentAssertions
- Moq

## 🐳 Docker Setup (Recomendado)

### Iniciar com Docker

```bash
# Iniciar tudo com Docker
docker-compose up --build

# Ou usar o script (Windows)
start-docker.bat
```

**URLs disponíveis:**
- API: http://localhost:5000
- Swagger: http://localhost:5000/swagger
- PostgreSQL: localhost:5432
- RabbitMQ Management: http://localhost:15672 (admin/admin123)

### Desenvolvimento Local

```bash
# 1. Iniciar PostgreSQL e RabbitMQ no Docker
docker-compose up -d postgres rabbitmq

# 2. Executar migrações
dotnet ef database update --project MotoTrust.Infrastructure --startup-project MotoTrust.API

# 3. Executar a aplicação
dotnet run --project MotoTrust.API

# 4. Executar o Consumer (em outro terminal)
dotnet run --project MotoTrust.Consumer
```

**Ou usar o script:**
```bash
start-dev.bat  # Windows
```

## Como rodar

### O que você precisa

- .NET 9.0 SDK
- PostgreSQL (qualquer versão recente)
- Visual Studio ou VS Code

### 1. Clone o repo

```bash
git clone https://github.com/willianalber/MotoTrust.git
cd MotoTrust
```

### 2. Configura o banco

Edita o `appsettings.json` com sua connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=MotoTrust;Username=postgres;Password=postgres"
  }
}
```

### 3. Roda as migrações

```bash
dotnet ef database update --project MotoTrust.Infrastructure --startup-project MotoTrust.API
```

### 4. Executa a aplicação

```bash
dotnet run --project MotoTrust.API
```

A API vai rodar em:
- HTTPS: https://localhost:7288
- HTTP: http://localhost:5055
- Swagger: https://localhost:7288/swagger (abre automaticamente)

## Sistema de Eventos (RabbitMQ)

Implementei um sistema de eventos usando **MassTransit** com **RabbitMQ** para processar algumas ações de forma assíncrona.

### Como funciona

1. Quando uma moto é alugada, o `CreateRentalCommandHandler` publica um `MotorcycleRentedEvent`
2. O evento vai para uma fila do RabbitMQ
3. O `MotoTrust.Consumer` consome o evento e faz o que precisa

### Eventos Disponíveis

- **MotorcycleRentedEvent**: Disparado quando uma moto é alugada

### Por que usei isso?

- **Desacoplamento**: A API não precisa esperar processamento de eventos
- **Escalabilidade**: Posso ter vários consumers processando
- **Resiliência**: Os eventos ficam persistidos no RabbitMQ
- **Extensibilidade**: Fácil adicionar novos consumers

### Exemplo de Uso

```csharp
// No CreateRentalCommandHandler
var @event = new MotorcycleRentedEvent(rental.Id, rental.EntregadorId, rental.MotoId, 
    rental.DataInicio, rental.DataPrevisaoTermino);
await _publishEndpoint.Publish(@event, cancellationToken);
```

### Consumer

O Consumer pode implementar várias coisas:
- Envio de emails de confirmação
- Notificações push
- Atualização de métricas
- Integração com GPS
- Auditoria e logs
- Integração com pagamentos

## Testes

### Testes

```bash
dotnet test
```

## Endpoints

### Motos

- `POST /api/motorcycle` - Cadastrar moto
- `GET /api/motorcycle/{id}` - Buscar moto por ID
- `GET /api/motorcycle?placa={placa}` - Listar motos por placa
- `PUT /api/motorcycle/{id}/placa` - Atualizar placa da moto
- `DELETE /api/motorcycle/{id}` - Deletar moto (exclusão lógica)

### Entregadores

- `POST /api/entregadores` - Cadastrar entregador
- `POST /api/entregadores/{id}/cnh` - Enviar foto da CNH

### Locações

- `POST /api/locacao` - Criar locação
- `GET /api/locacao/{id}` - Buscar locação por ID
- `PUT /api/locacao/{id}/devolucao` - Informar data de devolução

## Arquitetura (resumo)

### DDD

- **Entidades**: DeliveryPerson, Motorcycle, Rental
- **Value Objects**: Money
- **Enums**: MotorcycleStatus, RentalStatus, LicenseType
- **Eventos**: MotorcycleRentedEvent

### CQRS

- **Commands**: CreateMotorcycleCommand, CreateDeliveryPersonCommand, CreateRentalCommand, etc.
- **Queries**: GetMotorcycleByIdQuery, GetMotorcyclesByPlateQuery, GetRentalByIdQuery, etc.
- **Handlers**: CreateMotorcycleCommandHandler, GetMotorcycleByIdQueryHandler, etc.

### Repository

- **Interfaces**: IDeliveryPersonRepository, IMotorcycleRepository, IRentalRepository
- **Implementações**: DeliveryPersonRepository, MotorcycleRepository, RentalRepository
- **Unit of Work**: IUnitOfWork para transações

## Configurações

### Logs

Uso ILogger padrão do .NET:
- Console com formatação
- Debug output
- EventSource logging
- Logs estruturados com parâmetros

### Validação

- FluentValidation para validações
- Validações centralizadas nos Commands
- Método `IsValid()` em cada Command

## Melhorias Implementadas

### Validação com FluentValidation
- Validações movidas dos handlers para os commands
- Método `IsValid()` otimizado que retorna apenas a mensagem de erro
- Validações centralizadas e reutilizáveis

### Estrutura do Projeto
- Remoção de classes não utilizadas (Customer, Value Objects desnecessários)
- Limpeza de arquivos e pastas vazias
- Organização otimizada do código

### Configurações
- Swagger configurado para abrir automaticamente no navegador
- Portas atualizadas para o ambiente de desenvolvimento
- Migração de Serilog para ILogger padrão do .NET

## Exemplos

### Cadastrar moto

```bash
curl -X POST "https://localhost:7288/api/motorcycle" \
  -H "Content-Type: application/json" \
  -d '{
    "identificador": "moto123",
    "ano": 2020,
    "modelo": "Mottu Sport",
    "placa": "CDX-0101"
  }'
```

### Cadastrar entregador

```bash
curl -X POST "https://localhost:7288/api/entregadores" \
  -H "Content-Type: application/json" \
  -d '{
    "identificador": "entregador123",
    "nome": "João da Silva",
    "cnpj": "12345678901234",
    "data_nascimento": "1990-01-01T00:00:00Z",
    "numero_cnh": "12345678900",
    "tipo_cnh": "A",
    "imagem_cnh": "base64string"
  }'
```

### Criar locação

```bash
curl -X POST "https://localhost:7288/api/locacao" \
  -H "Content-Type: application/json" \
  -d '{
    "entregador_id": "entregador123",
    "moto_id": "moto123",
    "data_inicio": "2024-01-01T00:00:00Z",
    "data_termino": "2024-01-07T23:59:59Z",
    "data_previsao_termino": "2024-01-07T23:59:59Z",
    "plano": 7
  }'
```

## Contribuição

1. Fork o projeto
2. Cria uma branch (`git checkout -b feature/nova-feature`)
3. Commit suas mudanças (`git commit -m 'Adiciona nova feature'`)
4. Push para a branch (`git push origin feature/nova-feature`)
5. Abre um Pull Request

## Licença

MIT License - veja o arquivo LICENSE para detalhes.

## Autor

**Willian Alber**

- GitHub: [@willianalber](https://github.com/willianalber)
- LinkedIn: [Willian Alber](https://linkedin.com/in/willianalber)