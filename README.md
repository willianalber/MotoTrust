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
│   ├── Entities/               # Entidades do domínio (DeliveryPerson, Motorcycle, Rental, MotorcycleNotification)
│   ├── ValueObjects/           # Objetos de valor (Money)
│   ├── Enums/                  # Enumerações (MotorcycleStatus, RentalStatus, LicenseType, RentalPlan)
│   ├── Events/                 # Eventos de domínio (MotorcycleCreatedEvent, MotorcycleRentedEvent)
│   └── Interfaces/             # Contratos dos repositórios
├── MotoTrust.Application/      # Camada de Aplicação
│   ├── Commands/               # Comandos CQRS
│   ├── Queries/                # Consultas CQRS
│   ├── DTOs/                   # Data Transfer Objects
│   └── Validators/             # Validadores FluentValidation
├── MotoTrust.Infrastructure/   # Camada de Infraestrutura
│   ├── Data/                   # DbContext e configurações do EF
│   ├── Repositories/           # Implementações dos repositórios
│   ├── Services/               # Serviços (ImageStorageService)
│   └── Configurations/         # Configurações das entidades
├── MotoTrust.API/              # Camada de Apresentação
│   └── Controllers/            # Controllers da API
├── MotoTrust.Consumer/         # Consumer RabbitMQ
│   └── Consumers/              # Consumidores de eventos
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

1. Quando uma moto é cadastrada, o `CreateMotorcycleCommandHandler` publica um `MotorcycleCreatedEvent`
2. Quando uma moto é alugada, o `CreateRentalCommandHandler` publica um `MotorcycleRentedEvent`
3. Os eventos vão para filas do RabbitMQ
4. O `MotoTrust.Consumer` consome os eventos e processa conforme necessário

### Eventos Disponíveis

- **MotorcycleCreatedEvent**: Disparado quando uma moto é cadastrada
- **MotorcycleRentedEvent**: Disparado quando uma moto é alugada

### Consumidor Especial - Motos 2024

O sistema possui um consumidor especial que monitora motos do ano 2024:
- Quando uma moto 2024 é cadastrada, uma notificação especial é salva no banco
- Permite consultas futuras sobre motos 2024 cadastradas
- Sistema de notificações para casos especiais

### Por que usei isso?

- **Desacoplamento**: A API não precisa esperar processamento de eventos
- **Escalabilidade**: Posso ter vários consumers processando
- **Resiliência**: Os eventos ficam persistidos no RabbitMQ
- **Extensibilidade**: Fácil adicionar novos consumers

### Exemplo de Uso

```csharp
// No CreateMotorcycleCommandHandler
var @event = new MotorcycleCreatedEvent(
    motorcycle.Id,
    request.Identificador,
    motorcycle.Year,
    motorcycle.Model,
    motorcycle.LicensePlate,
    motorcycle.CreatedAt
);
await _publishEndpoint.Publish(@event, cancellationToken);

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

- `POST /motos` - Cadastrar moto
- `GET /motos/{id}` - Buscar moto por ID
- `GET /motos?placa={placa}` - Listar motos por placa
- `PUT /motos/{id}/placa` - Atualizar placa da moto
- `DELETE /motos/{id}` - Deletar moto (exclusão lógica)

### Entregadores

- `POST /entregadores` - Cadastrar entregador
- `POST /entregadores/{id}/cnh` - Enviar foto da CNH
- `GET /entregadores/{id}/cnh` - Buscar foto da CNH por ID (em desenvolvimento)
- `GET /entregadores/cnh/{fileName}` - Buscar foto da CNH por nome do arquivo

### Locações

- `POST /locacao` - Criar locação
- `GET /locacao/{id}` - Buscar locação por ID
- `PUT /locacao/{id}/devolucao` - Informar data de devolução (com cálculo de multa)

## Arquitetura (resumo)

### DDD

- **Entidades**: DeliveryPerson, Motorcycle, Rental, MotorcycleNotification
- **Value Objects**: Money
- **Enums**: MotorcycleStatus, RentalStatus, LicenseType, RentalPlan
- **Eventos**: MotorcycleCreatedEvent, MotorcycleRentedEvent

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

## Casos de Uso Implementados

### ✅ Administração de Motos
1. **Cadastro de Moto**: Identificador, Ano, Modelo, Placa (obrigatórios)
2. **Placa Única**: Validação de unicidade no banco de dados
3. **Evento de Cadastro**: Geração automática de `MotorcycleCreatedEvent`
4. **Consumidor Especial**: Notificações automáticas para motos 2024
5. **Consulta com Filtro**: Busca de motos por placa
6. **Modificação de Placa**: Atualização de placa cadastrada incorretamente
7. **Remoção**: Exclusão lógica (sem histórico de locações)

### ✅ Gestão de Entregadores
8. **Cadastro Completo**: Identificador, Nome, CNPJ, Data Nascimento, CNH, Tipo CNH, Imagem CNH
9. **Validações de Unicidade**: CNPJ e número da CNH únicos
10. **Tipos de CNH**: A, B ou AB (ambas)
11. **Storage de Imagens**: CNH em formato PNG/BMP armazenadas em arquivos
12. **Atualização de CNH**: Envio de nova foto da CNH

### ✅ Sistema de Locação
13. **Planos de Locação**: 7, 15, 30, 45, 50 dias com valores específicos
14. **Validação CNH**: Apenas entregadores com CNH tipo A ou AB podem alugar
15. **Validação de Data**: Início obrigatoriamente no primeiro dia após criação
16. **Cálculo de Multas**: 
    - Devolução antecipada: 20% (7 dias) ou 40% (15 dias) sobre diárias não efetivadas
    - Devolução atrasada: R$ 50,00 por dia adicional
17. **Consulta de Valores**: Cálculo detalhado na devolução

## Funcionalidades Implementadas

### Sistema de Locação
- **Planos de Locação**: 7, 15, 30, 45, 50 dias com valores específicos
- **Validação CNH**: Apenas entregadores com CNH tipo A ou AB podem alugar
- **Cálculo de Multas**: 
  - Devolução antecipada: 20% (7 dias) ou 40% (15 dias) sobre diárias não efetivadas
  - Devolução atrasada: R$ 50,00 por dia adicional
- **Validação de Data**: Início obrigatoriamente no primeiro dia após criação

### Sistema de Entregadores
- **Validações de Unicidade**: CNPJ e número da CNH únicos
- **Storage de Imagens**: CNH em formato PNG/BMP armazenadas em arquivos
- **Tipos de CNH**: A, B ou AB (ambas)

### Sistema de Motos
- **Placa Única**: Validação de unicidade no banco de dados
- **Eventos**: Geração de eventos para motos cadastradas
- **Consumidor Especial**: Notificações automáticas para motos 2024

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
- Sistema de eventos com RabbitMQ e MassTransit

## Exemplos

### Cadastrar moto

```bash
curl -X POST "http://localhost:5000/motos" \
  -H "Content-Type: application/json" \
  -d '{
    "identificador": "moto123",
    "ano": 2024,
    "modelo": "Mottu Sport",
    "placa": "CDX-0101"
  }'
```

### Cadastrar entregador

```bash
curl -X POST "http://localhost:5000/entregadores" \
  -H "Content-Type: application/json" \
  -d '{
    "identificador": "entregador123",
    "nome": "João da Silva",
    "cnpj": "12345678901234",
    "data_nascimento": "1990-01-01T00:00:00Z",
    "numero_cnh": "12345678900",
    "tipo_cnh": "A",
    "imagem_cnh": "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8/5+hHgAHggJ/PchI7wAAAABJRU5ErkJggg=="
  }'
```

### Criar locação

```bash
curl -X POST "http://localhost:5000/locacao" \
  -H "Content-Type: application/json" \
  -d '{
    "entregador_id": "entregador123",
    "moto_id": "moto123",
    "data_inicio": "2024-12-26T00:00:00Z",
    "data_termino": "2025-01-02T23:59:59Z",
    "data_previsao_termino": "2025-01-02T23:59:59Z",
    "plano": 7
  }'
```

### Informar devolução (com cálculo de multa)

```bash
curl -X PUT "http://localhost:5000/locacao/{id}/devolucao" \
  -H "Content-Type: application/json" \
  -d '{
    "data_devolucao": "2025-01-01T00:00:00Z"
  }'
```

**Resposta esperada:**
```json
{
  "mensagem": "Data de devolução informada com sucesso",
  "valorTotal": 180.00,
  "valorBase": 150.00,
  "valorMulta": 30.00,
  "diasUtilizados": 5,
  "diasAtraso": 0,
  "diasAntecipacao": 1,
  "tipoCalculo": "antecipado"
}
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