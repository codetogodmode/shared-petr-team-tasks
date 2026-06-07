# shared-petr-team-tasks

**Shared OOP drill projekt** — Code to God Mode, 1:1 cadence Martin + Petr.

Postupně rosteme od jednoduché `TaskItem` třídy k reálné mini-appce na správu projektů a týmu. Kód vzniká **společně live na 1:1 sessions**, mezi sessions ho Petr rozšiřuje sám (v Helper AI módu).

## Doména

- **Project** — má jméno, rozpočet, kolekci tasks (composition).
- **TaskItem** — title, t-shirt size, status, kdo je přiřazený.
- **User** *(přijde v drill #2)* — jméno, rate, role(s), seniority. Validace přiřazení podle role.

## Jak začít

```bash
dotnet restore
dotnet build
dotnet run --project src/App
```

## Testy

```bash
dotnet test
```

## Struktura

```
src/App/           — TaskItem, Project, Program
tests/App.Tests/   — unit testy (přijdou)
```

## Cadence drillů

| Drill | Téma | Stav |
|---|---|---|
| #1 (2026-06-07) | OOP fundamentals, `TaskItem` + `Project` + composition | live |
| #2 (TBD) | `User` class + role validation + LINQ | plánováno |
| #3 (TBD) | JSON persistence + file IO | plánováno |
| #4 (TBD) | SQLite + EF Core | plánováno |

## Pravidla

- Petr push priamo do `main` zatím **OK** — Layer 2 progressive governance, branch protection přidáme později.
- Helper AI mode aktivní → AI = sparring partner, ne kopírka. PR description vždy s **proč** (design rozhodnutí).
- Commit messages anglicky.

## Související

- Session card: `ops/curriculum/sessions/petr-1on1-01-kickoff-oop/card.md`
- Helper AI policy: `handbook/ai-policy.md`
