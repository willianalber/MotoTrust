# MotoTrust API

Sistema de locação de motocicletas desenvolvido para um teste técnico. API que gerencia entregadores, motos e locações.

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
- HTTPS: https://localhost:7288
- HTTP: http://localhost:5055
- Swagger: https://localhost:7288/swagger (abre automaticamente)

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

Uso Serilog para logs:
- Console com formatação
- Arquivos com rotação diária
- Logs estruturados

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