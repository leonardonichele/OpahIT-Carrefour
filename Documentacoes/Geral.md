# A solução foi construída seguindo princípios de:

- Clean Architecture
- Domain-Driven Design (DDD)
- CQRS
- Outbox Pattern
- Publisher/Subscriber (RabbitMQ)
- Background Jobs (Hangfire)
- APIs isoladas com SQLite
- Segurança por API Key
- Capacidade de testes automatizados via xUnit

# Tecnologias Utilizadas:

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core (SQLite)
- RabbitMQ
- Hangfire
- MediatR
- FluentValidation
- Swagger (OpenAPI)


# Ambiente:
Docker / Docker Compose

## Na prática o fluxo é composto por:

1. O usuário cria um lançamento no GFICash
2. O handler salva no banco local
3. Gera um evento de domínio
4. Evento é persistido na Outbox
5. O OutboxDispatcher publica para RabbitMQ
6. O GFIReport recebe o evento
7. Atualiza sua base própria de relatórios
8. API de relatórios retorna totais consolidados

# Healthcheck básico implementado por API. Rota /health

## Requisito 5%
1. Uso de Dead Letter Queue (DLQ), onde todas as falhas de processamento são enviadas para uma fila secundária.
2. Prefetch onde é ajustado a quantidade de mensagens simultâneas que o consumidor recebe.
3. Processamento em lotes de até 20 itens e paralelismo 4 workers.