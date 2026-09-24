# 01 · Visão geral

## Por que este projeto existe

Eu queria um sistema realista para praticar resiliência, e não um brinquedo que só funciona quando tudo está saudável. O objetivo de longo prazo é ter um serviço que injeta falhas controladas no sistema (latência, um gateway de pagamento quebrado, um banco de dados que some por um tempo) e uma plataforma que sobrevive a elas sem que o cliente perceba.

Para chegar lá com honestidade, preciso antes de uma base sólida. Essa base é o que este repositório contém hoje.

## Escopo da Fase 1

**Incluído**

- `Orders.Api`: recebe pedidos e expõe o status deles.
- `Payments.Api`: cobra os pedidos por uma abstração de gateway (por enquanto simulado) e registra o resultado.
- Comunicação assíncrona pelo RabbitMQ, com MassTransit.
- Mensageria confiável com outbox e inbox transacionais.
- Ambiente local reproduzível com Docker Compose.

**Ainda fora do escopo**

- Retry, circuit breaker, timeout e gateway de contingência (Fase 2).
- Métricas e dashboards (Fase 2).
- O motor de caos (Fase 3).
- API gateway, autenticação, gRPC e serviço de catálogo (Fase 4).

## Como eu trabalho

Sigo o Readme-Driven Development: descrevo o comportamento nestes documentos primeiro. Depois escrevo testes que expressam esse comportamento e, só então, escrevo o código que os faz passar. O fluxo completo está em [07 · Fluxo de desenvolvimento](07-development-workflow.md).

## Glossário

- **Evento**: um fato que já aconteceu, como `OrderCreated`. Os serviços publicam eventos e não sabem quem está ouvindo.
- **Consumer**: o código que reage a um evento quando ele chega do broker.
- **Outbox**: uma tabela que guarda os eventos de saída na mesma transação de banco dos dados de negócio, de modo que um evento nunca se perde nem é publicado para um dado que sofreu rollback.
- **Inbox**: uma tabela que lembra quais mensagens o serviço já processou, para que reentregas sejam ignoradas.
- **Idempotência**: tratar a mesma mensagem duas vezes tem o mesmo efeito que tratá-la uma vez.
- **Consistência eventual**: os serviços concordam entre si após um pequeno atraso, e não no mesmo instante.
