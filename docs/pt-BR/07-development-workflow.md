# 07 · Fluxo de desenvolvimento

## O ciclo

1. **Documentar o comportamento (RDD).** Descrevo o que a mudança deve fazer no README ou em `docs/` e adiciono seus cenários ao catálogo em [03 · Fluxo de mensagens](03-message-flow.md).
2. **Escrever os testes (TDD).** Um teste falhando por cenário, na camada mais baixa capaz de expressá-lo.
3. **Fazer passar.** A mudança mais simples que deixa o teste verde.
4. **Refatorar.** Com os testes verdes, limpo o código sem alterar o comportamento.
5. **Atualizar a documentação.** Nos dois idiomas, na mesma mudança.

Não escrevo código de produção para uma funcionalidade nova enquanto não existir um teste falhando para ela.

## Regras de idioma

- Tudo em `docs/` e os READMEs principais existem em **inglês e português do Brasil**. As duas versões têm o mesmo conteúdo e os mesmos nomes de arquivo. Atualizo as duas na mesma mudança, para que nunca se distanciem.
- Comentários no código são escritos **somente em inglês**.
- Mensagens de commit são escritas em inglês.

## Política de comentários

Mantenho os comentários no mínimo. Bons nomes e métodos pequenos devem deixar o código legível por si só. Um comentário só merece existir quando o *porquê* é difícil de enxergar no código: um fluxo intrincado de consumer, uma restrição sutil de ordem ou uma regra de negócio específica. Um comentário que repete o que a próxima linha faz é apagado.

## Commits e pull requests

Uso Conventional Commits: `docs:`, `test:`, `feat:`, `fix:`, `refactor:`, `chore:`.

Uma pull request está pronta quando:

- o comportamento está documentado nos dois idiomas;
- os testes cobrem os cenários e passam;
- o código segue a política de comentários;
- `docker compose up -d --build` continua subindo o sistema inteiro.
