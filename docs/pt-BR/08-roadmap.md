# 08 · Roadmap

Cada fase é pequena o bastante para ser concluída e demonstrada sozinha.

## Fase 1 · Fundação (atual)

Orders e Payments conversando pelo RabbitMQ, com outbox e inbox, rodando no Docker Compose. Os próximos itens dentro desta fase: a documentação, a limpeza dos comentários e os testes de caracterização.

## Fase 2 · Resiliência e observabilidade

- Mover a chamada ao gateway para fora da transação do consumer.
- Clientes Refit para um gateway de pagamento primário e um de contingência.
- Pipeline do Polly: retry, circuit breaker e timeout.
- Fallback transparente para o gateway de contingência, com chave de idempotência em toda cobrança.
- Métricas com OpenTelemetry, Prometheus e Grafana.
- Um endpoint de caos manual para disparar falhas sob demanda.

## Fase 3 · Motor de caos

Um worker que lê as métricas do sistema e decide quando injetar uma falha (latência, falha de gateway, queda simulada do banco). Todo experimento tem duração, cooldown, aborto automático e uma hipótese escrita de antemão. O critério de sucesso é mensurável: por exemplo, com 30% de latência injetada e o gateway primário fora do ar, 99% dos pedidos ainda terminam pagos.

## Fase 4 · Plataforma

YARP como API gateway, Keycloak com validação de JWT, gRPC entre serviços internos, um serviço de Catálogo com cache em Redis e uma saga para coordenar o processo do pedido.
