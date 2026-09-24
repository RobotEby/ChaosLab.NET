# 06 · Estratégia de testes

Escrevo os testes antes do código que eles exercitam. Os comportamentos vêm do catálogo de cenários em [03 · Fluxo de mensagens](03-message-flow.md), então cada teste é rastreável até um cenário documentado.

> Situação: isto é o plano. Os projetos de teste ainda não existem e são o próximo passo.

## Duas situações, duas abordagens

- **Comportamento novo** segue o ciclo estrito vermelho, verde, refatorar: escrevo um teste que falha, faço-o passar com a mudança mais simples e depois limpo o código.
- **Comportamento existente da Fase 1** ainda não tem testes. Para ele escrevo *testes de caracterização*, que descrevem o que o sistema faz hoje segundo o catálogo. Eles passam desde o início e funcionam como rede de segurança antes de eu refatorar ou construir a Fase 2 por cima.

## Camadas

| Camada | O que verifica | Infraestrutura | Velocidade |
|---|---|---|---|
| **Unidade** | Regras de negócio isoladas (transições de estado, simulação do gateway) | Nenhuma | Milissegundos |
| **Consumer** | Um consumer reagindo a uma mensagem, com sua persistência e os eventos publicados | Test harness do MassTransit (transporte em memória) e um container de SQL Server | Rápida |
| **Ponta a ponta** | As duas APIs, o RabbitMQ e o SQL Server funcionando juntos, incluindo a entrega do outbox | Testcontainers: RabbitMQ e SQL Server | Mais lenta |

Hoje algumas regras vivem dentro de consumers e endpoints. Ao escrever os testes de caracterização, vou mover pequenos trechos (por exemplo, a transição de `Pending` para `Paid` ou `PaymentFailed`) para dentro das entidades, para que possam ser testados como unidade, e farei isso como refatoração, já com os testes verdes.

## Onde cada cenário é verificado

| Cenários | Camada |
|---|---|
| ORD-05 a ORD-08, PAY-01 a PAY-04 | Unidade para a regra, consumer para o tratamento da mensagem |
| ORD-01, ORD-02, ORD-03, ORD-04, PAY-05 a PAY-07 | Testes de consumer e de endpoint contra um banco real |
| ORD-09, FLW-01 a FLW-03 | Ponta a ponta |

## Organização

```
tests/
  Orders.UnitTests/
  Payments.UnitTests/
  ChaosLab.IntegrationTests/      testes de consumer e ponta a ponta, compartilhando fixtures de containers
```

Os testes de integração precisam que as APIs sejam acessíveis pelo `WebApplicationFactory`, o que exige expor a classe `Program` como partial em cada serviço. É uma mudança de uma linha.

## Ferramentas

xUnit, NSubstitute para test doubles, Shouldly para asserções, Testcontainers (módulos RabbitMQ e MsSql), `MassTransit.Testing` para o harness e `Microsoft.AspNetCore.Mvc.Testing` para hospedar as APIs. Escolhi Shouldly em vez de FluentAssertions para evitar a licença comercial dele.

## Convenções

- Os nomes dos testes seguem `Method_Scenario_ExpectedResult`.
- Todo teste carrega o cenário que verifica: `[Trait("Scenario", "PAY-03")]`.
- Os containers sobem uma vez por execução e são compartilhados por collection fixtures do xUnit. Os testes se isolam com ids únicos, e não reiniciando containers.
- Nada de `Thread.Sleep`. Fluxos assíncronos são aguardados com um helper de polling que tem timeout.

## Execução

```bash
dotnet test                                        # tudo
dotnet test --filter "Scenario=PAY-03"             # um cenário
dotnet test --filter "FullyQualifiedName~UnitTests"
```

Os testes de consumer e ponta a ponta exigem um daemon do Docker em execução, tanto localmente quanto na CI. Os runners Linux hospedados pelo GitHub já o fornecem.
