namespace App.Workers;

/// <summary>
/// Pracovní pozice. Task deklaruje, jaká pozice ho smí plnit (eligibility).
/// Default sada — uprav podle reálné domény týmu.
/// PODHOUBÍ — záměrně bez logiky. Viz docs/specs/2026-06-07-workers-and-costing.md
/// </summary>
enum Position
{
    Developer,
    Tester,
    Designer,
    ProjectManager
}
