# RentIt - Motorcycle Rental API

Sistema de gerenciamento de aluguel de motos e entregadores.

---

## ✅ Descrição do Projeto

Aplicação em .NET 9 que permite:

- Cadastrar e consultar motos.
- Cadastrar entregadores com CNH (base64).
- Realizar locações com planos pré-definidos.
- Calcular devoluções com multa por atraso ou por antecipação.
- Emitir eventos com RabbitMQ ao cadastrar moto.
- Consumir eventos para armazenar motos com ano = 2024.

---

## 🚀 Tecnologias Utilizadas

- ASP.NET Core 9
- Entity Framework Core + Dapper
- PostgreSQL
- RabbitMQ (mensageria)
- FluentValidation
- Docker e Docker Compose
- Testes com xUnit + FluentAssertions

---

## ⚙️ Requisitos para Executar

- Docker e Docker Compose instalados
- .NET 9 SDK

---

## 🐳 Subindo a Aplicação

```bash
# Clonar o repositório
git clone https://github.com/debritoleo/app-aluguel.git

# Subir a infraestrutura (PostgreSQL + RabbitMQ)
docker compose up -d

# Rodar as migrations automaticamente
dotnet run --project src/RentIt.API
```

---

## 📬 Endpoints Principais

Documentação Swagger disponível após executar:
```
http://localhost:5051/swagger
```

---

### 🚗 Motos

| Método | Endpoint             | Ação                       |
|--------|----------------------|----------------------------|
| POST   | `/motos`             | Cadastrar moto             |
| GET    | `/motos`             | Listar motos (com filtro)  |
| GET    | `/motos/{id}`      | Buscar por ID              |

---

### 👤 Entregadores

| Método | Endpoint                      | Ação                        |
|--------|-------------------------------|-----------------------------|
| POST   | `/entregadores`               | Cadastrar entregador        |
| POST   | `/entregadores/{id}/cnh`    | Upload da CNH (base64)      |

---

### 🔄 Locações

| Método | Endpoint             | Ação                            |
|--------|----------------------|---------------------------------|
| POST   | `/locacoes`          | Criar locação                   |
| GET    | `/locacoes/{id}`   | Buscar locação por ID           |
| POST   | `/locacoes/{id}/devolucao` | Calcular valor de devolução  |

---

## 🧪 Executar Testes

```bash
dotnet test
```

---

## 📁 Estrutura

```
src/
├── RentIt.API                # API principal
├── RentIt.Application        # Casos de uso, serviços e comandos
├── RentIt.Domain             # Entidades e regras de domínio
├── RentIt.Infrastructure     # Repositórios, queries, mensageria
├── RentIt.IntegrationEvents  # Eventos de integração
tests/
└── RentIt.Tests              # Testes de integração
```

---

## 🧠 Padrões e Boas Práticas

- Clean Architecture simplificada
- DDD tático aplicado
- Princípios SOLID
- Exceptions globais com middleware
- Injeção de dependência configurada via `AddApplicationServices`
- TimeProvider para consistência nos testes

---

## 📅 Data de Finalização

`21/04/2025`

---

## 🏁 Conclusão

Projeto finalizado com 100% dos requisitos funcionais, integrações via RabbitMQ, validações robustas e testes automatizados.

---
