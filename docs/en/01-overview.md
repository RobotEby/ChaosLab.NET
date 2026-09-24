# 01 · Overview

## Why this project exists

I wanted a realistic system to practice resilience on, not a toy that only works when everything is healthy. The long-term goal is a service that injects controlled failures into the system (latency, a broken payment gateway, a database that disappears for a while) and a platform that survives them without the customer noticing.

To get there honestly, I first need a solid base. That base is what this repository contains today.

## Scope of Phase 1

**Included**

- `Orders.Api`: accepts orders and exposes their status.
- `Payments.Api`: charges orders through a gateway abstraction (a simulated one for now) and records the result.
- Asynchronous communication through RabbitMQ, with MassTransit.
- Reliable messaging with the transactional outbox and inbox.
- A reproducible local environment with Docker Compose.

**Not included yet**

- Retry, circuit breaker, timeout and a fallback gateway (Phase 2).
- Metrics and dashboards (Phase 2).
- The chaos engine (Phase 3).
- API gateway, authentication, gRPC and a catalog service (Phase 4).

## How I work

I follow Readme-Driven Development: I describe the behavior in these documents first. Then I write tests that express that behavior, and only after that do I write the code that makes them pass. The full workflow is in [07 · Development workflow](07-development-workflow.md).

## Glossary

- **Event**: a fact that already happened, such as `OrderCreated`. Services publish events and don't know who is listening.
- **Consumer**: the code that reacts to an event when it arrives from the broker.
- **Outbox**: a table that stores outgoing events in the same database transaction as the business data, so an event is never lost or published for data that was rolled back.
- **Inbox**: a table that remembers which messages a service has already processed, so redeliveries are ignored.
- **Idempotency**: handling the same message twice has the same effect as handling it once.
- **Eventual consistency**: the services agree with each other after a short delay, not at the same instant.
