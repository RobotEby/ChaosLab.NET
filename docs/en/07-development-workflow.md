# 07 · Development workflow

## The cycle

1. **Document the behavior (RDD).** I describe what the change should do in the README or `docs/`, and add its scenarios to the catalog in [03 · Message flow](03-message-flow.md).
2. **Write the tests (TDD).** One failing test per scenario, at the lowest layer that can express it.
3. **Make it pass.** The simplest change that turns the test green.
4. **Refactor.** With the tests green, I clean up without changing behavior.
5. **Update the docs.** Both languages, in the same change.

I don't write production code for a new feature until a failing test exists for it.

## Language rules

- Everything under `docs/` and the main READMEs exists in **English and Brazilian Portuguese**. The two versions carry the same content and the same file names. I update both in the same change, so they never drift.
- Code comments are written in **English only**.
- Commit messages are written in English.

## Comment policy

I keep comments to a minimum. Good names and small methods should make the code readable on their own. A comment earns its place only when the *why* is hard to see from the code: an intricate consumer flow, a subtle ordering constraint, or a specific business rule. A comment that restates what the next line does gets deleted.

## Commits and pull requests

I use Conventional Commits: `docs:`, `test:`, `feat:`, `fix:`, `refactor:`, `chore:`.

A pull request is ready when:

- the behavior is documented in both languages;
- the tests cover the scenarios and pass;
- the code follows the comment policy;
- `docker compose up -d --build` still brings the whole system up.
