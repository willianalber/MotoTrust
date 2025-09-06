# MotoTrust API

Sistema de locação de motocicletas desenvolvido para um teste técnico. Basicamente é uma API que gerencia clientes, motos e aluguéis.

## Arquitetura

Tentei seguir algumas boas práticas aqui:

- **CQRS** com MediatR (separação de comandos e consultas)
- **DDD** (Domain Driven Design) - pelo menos tentei
- **Repository Pattern** com Unit of Work
- **Entity Framework Core** com PostgreSQL
- **FluentValidation** para validações
- **AutoMapper** para mapear objetos
- **Serilog** para logs
- **Swagger** para documentar a API

## 📁 Estrutura do Projeto

```
MotoTrust/
├── MotoTrust.Domain/           # Camada de Domínio
│   ├── Common/                 # Classes base (EntityBase, ValueObject, DomainEvent)
│   ├── Entities/               # Entidades do domínio (Customer, Motorcycle, Rental)
│   ├── ValueObjects/           # Objetos de valor (Email, CPF, PhoneNumber, Money)
│   ├── Enums/                  # Enumerações (MotorcycleStatus, RentalStatus, LicenseType)
│   ├── Events/                 # Eventos de domínio
│   └── Interfaces/             # Contratos dos repositórios
├── MotoTrust.Application/      # Camada de Aplicação
│   ├── Commands/               # Comandos CQRS
│   ├── Queries/                # Consultas CQRS
│   ├── DTOs/                   # Data Transfer Objects
│   ├── Mappings/               # Configurações do AutoMapper
│   ├── Validators/             # Validadores FluentValidation
│   └── Common/                 # Classes base da aplicação
├── MotoTrust.Infrastructure/   # Camada de Infraestrutura
│   ├── Data/                   # DbContext e configurações do EF
│   ├── Repositories/           # Implementações dos repositórios
│   └── Configurations/         # Configurações das entidades
├── MotoTrust.API/              # Camada de Apresentação
│   ├── Controllers/            # Controllers da API
│   └── Middleware/             # Middlewares customizados
├── MotoTrust.Tests.Unit/       # Testes Unitários
└── MotoTrust.Tests.Integration/ # Testes de Integração
```

## Tecnologias

- .NET 9.0
- ASP.NET Core Web API
- Entity Framework Core 9.0
- PostgreSQL
- MediatR 13.0
- FluentValidation 12.0
- AutoMapper 15.0
- Serilog 4.0
- Swagger/OpenAPI
- xUnit (para testes)
- FluentAssertions
- Moq

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
- HTTPS: https://localhost:7001
- HTTP: http://localhost:5001
- Swagger: https://localhost:7001/swagger

## Testes

### Testes unitários

```bash
dotnet test MotoTrust.Tests.Unit
```

### Testes de integração

```bash
dotnet test MotoTrust.Tests.Integration
```

### Todos os testes

```bash
dotnet test
```

## Endpoints

### Clientes

- `POST /api/customers` - Criar cliente
- `GET /api/customers/{id}` - Buscar cliente por ID

### Motos

- `POST /api/motorcycles` - Cadastrar moto
- `GET /api/motorcycles` - Listar motos
- `GET /api/motorcycles/{id}` - Buscar moto por ID
- `PUT /api/motorcycles/{id}` - Atualizar moto
- `DELETE /api/motorcycles/{id}` - Deletar moto

### Aluguéis

- `POST /api/rentals` - Criar aluguel
- `GET /api/rentals` - Listar aluguéis
- `GET /api/rentals/{id}` - Buscar aluguel por ID
- `PUT /api/rentals/{id}/complete` - Finalizar aluguel
- `PUT /api/rentals/{id}/cancel` - Cancelar aluguel

## Arquitetura (resumo)

### DDD

- **Entidades**: Customer, Motorcycle, Rental
- **Value Objects**: Email, CPF, PhoneNumber, Money
- **Enums**: MotorcycleStatus, RentalStatus, LicenseType
- **Eventos**: CustomerCreatedEvent, MotorcycleRentedEvent, etc.

### CQRS

- **Commands**: CreateCustomerCommand, CreateMotorcycleCommand, etc.
- **Queries**: GetCustomerByIdQuery, GetAvailableMotorcyclesQuery, etc.
- **Handlers**: CreateCustomerCommandHandler, GetCustomerByIdQueryHandler, etc.

### Repository

- **Interfaces**: ICustomerRepository, IMotorcycleRepository, IRentalRepository
- **Implementações**: CustomerRepository, MotorcycleRepository, RentalRepository
- **Unit of Work**: IUnitOfWork para transações

## Configurações

### Logs

Uso Serilog para logs:
- Console com formatação
- Arquivos com rotação diária
- Logs estruturados

### Validação

- FluentValidation para validações
- Validações de domínio nas entidades
- Validação automática nos controllers

### Mapeamento

- AutoMapper entre DTOs e entidades
- Mapeamento customizado para value objects

## Exemplos

### Criar cliente

```bash
curl -X POST "https://localhost:7001/api/customers" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "João Silva",
    "email": "joao.silva@email.com",
    "phoneNumber": "11999999999",
    "cpf": "12345678901",
    "licenseType": 1,
    "licenseNumber": "123456789"
  }'
```

### Cadastrar moto

```bash
curl -X POST "https://localhost:7001/api/motorcycles" \
  -H "Content-Type: application/json" \
  -d '{
    "brand": "Honda",
    "model": "CB 600F",
    "year": 2023,
    "licensePlate": "ABC1234",
    "color": "Vermelha",
    "engineCapacity": 600,
    "dailyRate": 150.00,
    "currency": "BRL"
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