# Konzolová PM aplikace — design (projekty + tasky)

- **Datum:** 2026-06-07
- **Stav:** Schváleno (návrh funkcionalit odsouhlasen)
- **Scope:** aktuální iterace — pouze projekty a tasky

## Cíl

Konzolový nástroj pro projektový management nad **jedním projektem na běh**. OOP showcase
pro Layer 2 — důraz na enkapsulaci, doménové invarianty a forward-compatible návrh budgetu.
In-memory, bez perzistence.

## Scope

**In:** založení projektu, přidání tasků, přiřazení tasku (jméno jako string — dočasné),
dokončení tasku, výpis tasků, statistiky, převod `Size → cost` se srážkou z rozpočtu,
ošetření vstupu.

**Out (YAGNI):** perzistence, editace/mazání, více projektů, undo, worker systém
(dodáno jen jako scaffolding + samostatný spec `2026-06-07-workers-and-costing.md`).

## Doménový model

### Project — existuje, doplnit
Hotovo: `Project(name, budget)` s validací, `Tasks`, `CreatedAt`, `AddTask(task)`.

Doplnit:
- `AddTask` **strhne cenu tasku z rozpočtu** (alokace při založení — rozhodnutí R2).
- Spočítané vlastnosti:
  - `AllocatedCost` = součet `Cost` všech tasků
  - `RemainingBudget` = `Budget - AllocatedCost`
  - **Spočítané, ne uložené** — nemohou se rozejít s realitou (klíčová OOP lekce).
- Přečerpání (R3): **povolit** záporný `RemainingBudget`, ale UI varuje. Žádná výjimka.

### TaskItem — existuje
- `Cost` (decimal) — **spočítaná** vlastnost odvozená ze `Size` přes Size→cost tabulku.
- `AssignedTo` (string?) zůstává jako **dočasný placeholder** — nahradí `Worker`
  (viz workers spec).

### Size → cost (R1) — baseline odhad v CZK

| XS   | S    | M    | L     | XL    |
|------|------|------|-------|-------|
| 2000 | 4000 | 8000 | 16000 | 32000 |

> Baseline = „cena, kdyby task dělal **Medior**" (viz workers spec, effort matrix × sazba).
> Po zavedení workerů tuto hodnotu nahradí skutečná cena `hodiny × sazba`.

Implementace mapování: `Dictionary<Size, decimal>` (čitelná tabulka, snadno změnitelná) —
preferováno před `switch`.

## Konzolové UI

Start: založení projektu (název + budget) → pak menu.

Smyčka menu (číselné volby):
1. **Přidat task** (title + Size) — ukáže `Cost` a nový zůstatek; varuje při přečerpání
2. **Přiřadit task osobě** (vyber `#` ze seznamu → zadej jméno)
3. **Označit task hotový** (vyber `#`) — využije existující `Complete()`
4. **Vypsat tasky** (tabulka: `#`, název, Size, Cost, stav, přiřazeno)
5. **Statistiky**
0. **Konec**

Ošetření vstupu (E1): nečíselná volba, prázdný název, neexistující `#`, prázdné jméno →
hláška + opakování, **žádný pád**.

## Statistiky

- **D1 Rozpočet:** celkový / alokovaný / zbývající (+ indikace přečerpání)
- **D2 Stav:** Pending vs Completed + % dokončení
- **D3 Rozložení podle Size** (kolik XS/S/M/L/XL)
- **D4 Workload podle osoby:** počet + souhrnný `Cost` přiřazených tasků

## Testy

`tests/App.Tests` — unit testy na: Size→cost, srážku v `AddTask`,
`AllocatedCost`/`RemainingBudget`, povolené přečerpání, validaci konstruktoru,
invariant `Complete()`.

> Pozn.: `App.Tests` zatím není v `Template.slnx` — doplnit při implementaci, ať ho
> jazykový server i Test Explorer vidí.

## Vztah k worker featuře

`AssignedTo` string je dočasný; `Cost` je baseline odhad. Forward-compatible:
`Cost` i `RemainingBudget` jako **spočítané** vlastnosti → výměna zdroje ceny
(`Size→baseline` za `hodiny×sazba`) nerozbije budget logiku. Detaily v workers specu.
