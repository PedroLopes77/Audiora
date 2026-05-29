# 🎵 Audiora — Plataforma de Streaming de Áudio

> Plataforma moderna de streaming de áudio desenvolvida com ASP.NET Core 8, Clean Architecture, SQL Server, Docker e aplicativo Android nativo.

## 🏗️ Arquitetura
- Clean Architecture
- SOLID Principles
- Repository Pattern + Unit of Work
- JWT Authentication
- REST API

## 🛠️ Tecnologias
| Camada | Tecnologia |
|--------|------------|
| Backend | ASP.NET Core 8, C# |
| ORM | Entity Framework Core 8 |
| Banco | SQL Server 2022 |
| Auth | JWT Bearer |
| Docs | Swagger/OpenAPI |
| Mobile | Android Java |
| Admin | .NET MAUI |
| DevOps | Docker, GitHub Actions |

## 🚀 Como executar

### Pré-requisitos
- .NET 8 SDK
- SQL Server
- Docker (opcional)

### Rodando a API
\`\`\`bash
cd src/Audiora.API
dotnet run
\`\`\`

A API estará disponível em: https://localhost:5003
Swagger: https://localhost:5003/swagger

## 📁 Estrutura
\`\`\`
src/
  Audiora.Domain/       ← Entidades e regras de negócio
  Audiora.Application/  ← Casos de uso, DTOs, serviços
  Audiora.Infrastructure/ ← EF Core, repositórios, serviços externos
  Audiora.API/          ← Controllers, middleware, configuração
tests/
  Audiora.Tests/        ← Testes unitários e integração
\`\`\`

## 👥 Contribuição
Siga o GitFlow: `feature/*`, `hotfix/*`, `release/*`
