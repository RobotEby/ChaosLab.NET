# 08 · Roadmap

Each phase is small enough to finish and demonstrate on its own.

## Phase 1 · Foundation (current)

Orders and Payments talking through RabbitMQ, with the outbox and inbox, running under Docker Compose. Next up inside this phase: the documentation, the comment cleanup, and characterization tests.

## Phase 2 · Resilience and observability

- Move the gateway call out of the consumer's transaction.
- Refit clients for a primary and a fallback payment gateway.
- Polly pipeline: retry, circuit breaker and timeout.
- Transparent fallback to the contingency gateway, with an idempotency key on every charge.
- Metrics with OpenTelemetry, Prometheus and Grafana.
- A manual chaos endpoint to trigger failures on demand.

## Phase 3 · Chaos engine

A worker that reads the system's metrics and decides when to inject a failure (latency, gateway failure, simulated database outage). Every experiment has a duration, a cooldown, an automatic abort and a hypothesis written down beforehand. The success criterion is measurable: for example, with 30% latency injected and the primary gateway down, 99% of orders still end up paid.

## Phase 4 · Platform

YARP as the API gateway, Keycloak with JWT validation, gRPC between internal services, a Catalog service with Redis caching, and a saga to coordinate the order process.
