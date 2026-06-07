# Worker systém & costing — spec (budoucí feature)

- **Datum:** 2026-06-07
- **Stav:** 🌱 **Naskicováno (scaffolding), NEIMPLEMENTOVÁNO**
- **Vlastník:** kolega (member)
- **Aktuální iterace dodává:** jen kódové podhoubí + tento spec. Žádné zapojení do appky.

## Cíl

Nahradit dočasné přiřazení `string AssignedTo` reálnými pracovníky. Cena tasku se počítá
z toho, **kdo** ho dělá: seniornější zvládne task za méně hodin, ale má vyšší sazbu.

## Koncepty

- **Position** — pracovní pozice. Task deklaruje, jaká pozice ho smí plnit (eligibility).
- **Seniority** — Junior / Medior / Senior. Určuje sazbu i počet hodin.
- **Worker** — `Name`, `Position`, `Seniority`.
- **Hodinová sazba** — odvozená od seniority (příp. pozice).
- **Effort matrix** — kolik hodin zabere daný `Size` které senioritě.
- **Cost** — `EstimateHours(Size, Seniority) × HourlyRate(Worker)`.

## Navržené defaulty (ilustrativní, kolega doladí)

### Hodinové sazby (CZK/h)

| Junior | Medior | Senior |
|--------|--------|--------|
| 700    | 1000   | 1500   |

### Effort matrix (hodiny)

| Size | Junior | Medior | Senior |
|------|--------|--------|--------|
| XS   | 3      | 2      | 1.5    |
| S    | 6      | 4      | 3      |
| M    | 12     | 8      | 6      |
| L    | 24     | 16     | 12     |
| XL   | 48     | 32     | 24     |

> Junior = Medior × 1.5 h, Senior = Medior × 0.75 h.

### Výsledná cena (kontrola)

| Size | Junior | Medior | Senior |
|------|--------|--------|--------|
| XS   | 2100   | **2000** | 2250 |
| S    | 4200   | **4000** | 4500 |
| M    | 8400   | **8000** | 9000 |
| L    | 16800  | **16000**| 18000|
| XL   | 33600  | **32000**| 36000|

> **Medior sloupec = baseline odhad z konzolového specu** (Size→cost:
> 2000/4000/8000/16000/32000). Tj. interim baseline = „cena pro Mediora". Tím jsou oba
> specy konzistentní a výměna výpočtu po zavedení workerů je plynulá.

## Eligibility

`TaskItem` získá `EligiblePositions` (jedna či víc pozic). Přiřazení workera mimo eligible
pozici se **odmítne** (validace v `AssignTo`).

## Migrační plán (string → Worker)

1. `TaskItem.AssignedTo: string?` → `Assignee: Worker?`;
   `AssignTo(string)` → `AssignTo(Worker)` + eligibility check.
2. Zavést implementaci `ICostModel` (effort matrix + sazby).
3. `TaskItem.Cost`:
   - dokud není přiřazen worker → baseline ze `Size` (jako dnes),
   - po přiřazení → `ICostModel.CostFor(Size, Worker)`.
4. `Project.RemainingBudget` se nemění (spočítaná vlastnost) — mění se jen **zdroj** `Cost`.
   Reconciliace odhad → skutečnost proběhne automaticky při přiřazení.

## Kódové podhoubí (co je v repu teď)

| Soubor | Co |
|--------|-----|
| `src/App/Workers/Seniority.cs` | enum Junior/Medior/Senior |
| `src/App/Workers/Position.cs`  | enum Developer/Tester/Designer/ProjectManager (uprav) |
| `src/App/Workers/Worker.cs`    | datová třída (Name, Position, Seniority) |
| `src/App/Workers/ICostModel.cs`| seam pro výpočet ceny (EstimateHours / HourlyRate / CostFor) |

Vše izolované v namespace `App.Workers`, **nezapojené** do `Project`/`TaskItem`, kompiluje.

## Otevřené otázky pro kolegu

- Finální sada pozic (`Position`).
- Závisí sazba i na pozici, nebo jen na senioritě?
- Může mít task víc eligible pozic? Víc workerů na jeden task?
- Co s rozpracovaným taskem při změně/odebrání workera (reconciliace rozpočtu)?
