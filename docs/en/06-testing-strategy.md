# 06 · Testing strategy

I write tests before the code they exercise. The behaviors come from the scenario catalog in [03 · Message flow](03-message-flow.md), so each test traces back to a documented scenario.

> Status: this is the plan. The test projects don't exist yet and are the next step.

## Two situations, two approaches

- **New behavior** follows strict red, green, refactor: I write a failing test, make it pass with the simplest change, then clean up.
- **Existing Phase 1 behavior** has no tests yet. For it I write *characterization tests* that describe what the system does today according to the catalog. They pass from the start and act as a safety net before I refactor or build Phase 2 on top.

## Layers

| Layer | What it checks | Infrastructure | Speed |
|---|---|---|---|
| **Unit** | Business rules in isolation (state transitions, gateway simulation) | None | Milliseconds |
| **Consumer** | A consumer reacting to a message, with its persistence and published events | MassTransit test harness (in-memory transport) and a SQL Server container | Fast |
| **End to end** | Both APIs, RabbitMQ and SQL Server working together, including outbox delivery | Testcontainers: RabbitMQ and SQL Server | Slower |

Right now some rules live inside consumers and endpoints. When I write the characterization tests, I'll move small pieces (for example, the `Pending` to `Paid` or `PaymentFailed` transition) into the entities so they can be unit tested, and I'll do that as a refactor with the tests already green.

## Where each scenario is verified

| Scenarios | Layer |
|---|---|
| ORD-05 to ORD-08, PAY-01 to PAY-04 | Unit for the rule, consumer for the message handling |
| ORD-01, ORD-02, ORD-03, ORD-04, PAY-05 to PAY-07 | Consumer and endpoint tests against a real database |
| ORD-09, FLW-01 to FLW-03 | End to end |

## Layout

```
tests/
  Orders.UnitTests/
  Payments.UnitTests/
  ChaosLab.IntegrationTests/      consumer tests and end-to-end tests, sharing container fixtures
```

Integration tests need the APIs to be reachable from `WebApplicationFactory`, which means exposing `Program` as a partial class in each service. It's a one-line change.

## Tooling

xUnit, NSubstitute for test doubles, Shouldly for assertions, Testcontainers (RabbitMQ and MsSql modules), `MassTransit.Testing` for the harness and `Microsoft.AspNetCore.Mvc.Testing` for hosting the APIs. I picked Shouldly over FluentAssertions to avoid its commercial license.

## Conventions

- Test names follow `Method_Scenario_ExpectedResult`.
- Every test carries the scenario it verifies: `[Trait("Scenario", "PAY-03")]`.
- Containers start once per test run and are shared through xUnit collection fixtures. Tests isolate themselves with unique ids rather than by restarting containers.
- No `Thread.Sleep`. Asynchronous flows are awaited with a polling helper that has a timeout.

## Running

```bash
dotnet test                                        # everything
dotnet test --filter "Scenario=PAY-03"             # one scenario
dotnet test --filter "FullyQualifiedName~UnitTests"
```

Consumer and end-to-end tests require a running Docker daemon, both locally and in CI. GitHub-hosted Linux runners already provide it.
